using AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount;
using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using AdaptableDialogAnalyzer.Unity.UIElements;
using System;
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
        [Header("Adapter")]
        public ProjectSekai_MasterLoader masterLoader;
        public Pixiv_SearchResponseLoader searchResponseLoader;

        CharacterPostCountManager countManager;
        public CharacterPostCountManager CountManager => countManager;

        Dictionary<int, DateTime> datetimeIndexes;
        Dictionary<DateTime, Action> datetimeActions = new Dictionary<DateTime, Action>();
        DateTime lastDateTime;

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
            Play();
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

        void InitDatetimeActions()
        {
            datetimeActions[ProjectSekaiHelper.anniversary1] = () => effects.PlayAnniversaryEffect(1);
            datetimeActions[ProjectSekaiHelper.anniversary2] = () => effects.PlayAnniversaryEffect(2);
            datetimeActions[ProjectSekaiHelper.anniversary3] = () => effects.PlayAnniversaryEffect(3);
        }
    }
}
