using AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount;
using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using AdaptableDialogAnalyzer.Unity.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_PixivCharacterPostCount : AutoSortBarChart
    {
        [Header("Components")]
        public View_ProjectSekai_TimelineTypeA timeline;
        public View_ProjectSekai_TimelineTypeA_Effects effects;
        public GraphicColorTransition transitionIn;
        public GraphicColorTransition transitionOut;
        [Header("Time")]
        public float preReleaseDFPS = 10;
        public float year0Duration = 100;
        public float year1Duration = 100;
        public float year2Duration = 100;
        public float transitionOutDelay = 10;
        [Header("Settings")]
        public ColorList yearColorTheme;
        [Header("Adapter")]
        public ProjectSekai_MasterLoader masterLoader;
        public Pixiv_SearchResponseLoader searchResponseLoader;

        CharacterPostCountManager countManager;
        public CharacterPostCountManager CountManager => countManager;

        Dictionary<int, DateTime> datetimeIndexes;
        Dictionary<DateTime, Action> datetimeActions = new Dictionary<DateTime, Action>();
        DateTime lastDateTime;

        int releaseDataFrame;
        float year0DFPS = 3;
        float year1DFPS = 3;
        float year2DFPS = 3;

        private void Awake()
        {
            CharacterGetterByPixivTag characterGetterByPixivTag = new CharacterGetterByPixivTag();

            Extra.Pixiv.SearchResponse.MergedResponse mergedResponse = searchResponseLoader.MergedResponse;
            CharacterPostCounter characterPostCounter = new CharacterPostCounter();

            characterPostCounter.getArtworkCharacters = (artwork) =>
            {
                return characterGetterByPixivTag.GetCharacterByTag(artwork.tags);
            };
            characterPostCounter.getNovelCharacters = (novel) =>
            {
                return characterGetterByPixivTag.GetCharacterByTag(novel.tags);
            };

            CharacterPostCountManager characterPostCountManager = characterPostCounter.Count(mergedResponse);
            countManager = characterPostCountManager.ToTotalMode();
            countManager.CalcDelta();
            countManager.CalcDeltaOfDelta();
            datetimeIndexes = countManager.GetDateTimeIndexes();

            timeline.Initialize(countManager.days.Keys.ToArray());
            InitDatetimeActions();
            CalcDataFramePerSec();
            dataFramePerSec = preReleaseDFPS;

            GlobalColor.SetThemeColor(yearColorTheme[0]);
        }

        protected override List<IAutoSortBarChartData> GetDataFrame(int dataFrame)
        {
            //TODO: Remove
            if (!datetimeIndexes.ContainsKey(dataFrame) || !countManager.days.ContainsKey(datetimeIndexes[dataFrame]))
            {
                Debug.Log(dataFrame);
                Debug.Log(datetimeIndexes[dataFrame]);
            }
            return countManager.days[datetimeIndexes[dataFrame]].GetAutoSortBarChartData();
        }

        protected override int GetTotalDataFrames()
        {
            return datetimeIndexes.Count;
        }

        protected override void PlayFrame(float currentDataFrame)
        {
            base.PlayFrame(currentDataFrame);
            timeline.SetPosition(currentDataFrame);

            // 触发日期事件
            if (currentDataFrame >= GetTotalDataFrames()) return;
            int currentDataFrameInt = (int)currentDataFrame;
            DateTime dateTime = datetimeIndexes[currentDataFrameInt];
            foreach (var keyValuePair in datetimeActions)
            {
                if (keyValuePair.Key > lastDateTime && keyValuePair.Key <= dateTime)
                {
                    keyValuePair.Value();
                }
            }
            lastDateTime = dateTime;

            // 在正式开服前，插值每秒数据帧数
            if (currentDataFrame < releaseDataFrame)
            {
                dataFramePerSec = Mathf.Lerp(preReleaseDFPS, year0DFPS, currentDataFrame / (float)releaseDataFrame);
            }
        }

        protected override AutoSortBarChart_Bar AddBar(IAutoSortBarChartData data)
        {
            AutoSortBarChart_Bar bar = base.AddBar(data);
            if (bar is View_ProjectSekai_PixivCharacterPostCount_Bar pixivCharacterPostCount_Bar)
            {
                pixivCharacterPostCount_Bar.Initialize(data);
            }
            else
            {
                Debug.LogError("错误的条类型");
            }
            return bar;
        }

        protected override void Play()
        {
            base.Play();
            StartCoroutine(CoTransitionOut());
        }

        protected override IEnumerator CoPlay()
        {
            bool keepWaiting = true;
            transitionIn.OnTransitionMiddle += () => keepWaiting = false;
            transitionIn.StartTransition();

            while (keepWaiting)
            {
                yield return 1;
            }

            yield return base.CoPlay();
        }

        IEnumerator CoTransitionOut()
        {
            while (CurrentDataFrame < GetTotalDataFrames())
            {
                yield return 1;
            }
            yield return new WaitForSeconds(transitionOutDelay);
            transitionOut.StartTransition();
        }

        void CalcDataFramePerSec()
        {
            int[] yearDataFrameCount = new int[4];

            yearDataFrameCount[1] = countManager.days
                .Where(kvp => kvp.Key >= ProjectSekaiHelper.anniversary0 && kvp.Key < ProjectSekaiHelper.anniversary1)
                .Count();
            yearDataFrameCount[2] = countManager.days
                .Where(kvp => kvp.Key >= ProjectSekaiHelper.anniversary1 && kvp.Key < ProjectSekaiHelper.anniversary2)
                .Count();
            yearDataFrameCount[3] = countManager.days
                .Where(kvp => kvp.Key >= ProjectSekaiHelper.anniversary2 && kvp.Key < ProjectSekaiHelper.anniversary3)
                .Count();

            year0DFPS = (float)yearDataFrameCount[1] / year0Duration;
            year1DFPS = (float)yearDataFrameCount[2] / year1Duration;
            year2DFPS = (float)yearDataFrameCount[3] / year2Duration;

            releaseDataFrame = countManager.days
                .OrderBy(kvp => kvp.Key)
                .Select((kvp, id) => (kvp, id))
                .First(t => t.kvp.Key >= ProjectSekaiHelper.anniversary0).id;
        }

        void InitDatetimeActions()
        {
            datetimeActions[ProjectSekaiHelper.anniversary0] = () =>
            {
                dataFramePerSec = year0DFPS;
                foreach (var barManager in activeBars)
                {
                    View_ProjectSekai_PixivCharacterPostCount_Bar bar = (View_ProjectSekai_PixivCharacterPostCount_Bar)barManager.bar;
                    bar.FadeIncrease(1);
                    bar.ToggleGlow(true);
                }
            };
            datetimeActions[ProjectSekaiHelper.anniversary1] = () =>
            {
                effects.PlayAnniversaryEffect(1);
                dataFramePerSec = year1DFPS;
                GlobalColor.SetThemeColor(yearColorTheme[1]);
            };
            datetimeActions[ProjectSekaiHelper.anniversary2] = () =>
            {
                effects.PlayAnniversaryEffect(2);
                dataFramePerSec = year2DFPS;
                GlobalColor.SetThemeColor(yearColorTheme[2]);
            };
            datetimeActions[ProjectSekaiHelper.anniversary3] = () =>
            {
                effects.PlayAnniversaryEffect(3);
                GlobalColor.SetThemeColor(yearColorTheme[3]);
            };
            datetimeActions[countManager.days.Max(kvp => kvp.Key)] = () =>
            {
                foreach (var barManager in activeBars)
                {
                    View_ProjectSekai_PixivCharacterPostCount_Bar bar = (View_ProjectSekai_PixivCharacterPostCount_Bar)barManager.bar;
                    bar.FadeIncrease(0);
                    bar.ToggleGlow(false);
                }
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Play();
            }
        }
    }
}
