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

       Dictionary<string, int> tagCharacterIdPair;
       Dictionary<int, DateTime> datetimeIndexes;

        private void Awake()
        {
            tagCharacterIdPair = GlobalConfig.CharacterDefinition.Characters
                .Where(c => c.id >= 1 && c.id <= 20)
                .ToDictionary(c => c.name.Replace(" ", string.Empty), c => c.id);

            Extra.Pixiv.SearchResponse.MergedResponse mergedResponse = searchResponseLoader.MergedResponse;
            CharacterPostCounter characterPostCounter = new CharacterPostCounter();

            characterPostCounter.getArtworkCharacters = (artwork) =>
            {
                return GetCharacterByTag(artwork.tags);
            };
            characterPostCounter.getNovelCharacters = (novel) =>
            {
                return GetCharacterByTag(novel.tags);
            };

            CharacterPostCountManager characterPostCountManager = characterPostCounter.Count(mergedResponse);
            countManager = characterPostCountManager.ToTotalMode();
            countManager.CalcDelta();
            datetimeIndexes = countManager.GetDateTimeIndexes();

            timeline.Initialize(this);
            Play();
        }

        /// <summary>
        /// 统计函数用，根据标签获取角色ID
        /// </summary>
        int[] GetCharacterByTag(List<string> tags)
        {
            List<int> characterIds = new List<int>();
            foreach (var tag in tags)
            {
                if (tagCharacterIdPair.ContainsKey(tag))
                {
                    characterIds.Add(tagCharacterIdPair[tag]);
                }
            }
            return characterIds.ToArray();
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
    }
}
