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

        [Header("Adapter")]
        public ProjectSekai_MasterLoader masterLoader;
        public Pixiv_SearchResponseLoader searchResponseLoader;

        CharacterPostCountManager countManager;
        public CharacterPostCountManager CountManager => countManager;

        Dictionary<int, DateTime> datetimeIndexes;

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
            Play();
        }

        protected override List<IAutoSortBarChartData> GetDataFrame(int dataFrame)
        {
            //TODO: Remove
            if(!datetimeIndexes.ContainsKey(dataFrame) || !countManager.days.ContainsKey(datetimeIndexes[dataFrame]))
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
        }

        protected override AutoSortBarChart_Bar AddBar(IAutoSortBarChartData data)
        {
            AutoSortBarChart_Bar bar = base.AddBar(data);
            if(bar is View_ProjectSekai_PixivCharacterPostCount_Bar pixivCharacterPostCount_Bar)
            {
                pixivCharacterPostCount_Bar.Initialize(data);
            }
            else
            {
                Debug.LogError("错误的条类型");
            }
            return bar;
        }
    }
}
