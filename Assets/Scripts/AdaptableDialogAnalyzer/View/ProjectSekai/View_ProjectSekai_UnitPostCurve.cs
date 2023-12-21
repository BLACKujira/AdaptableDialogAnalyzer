using AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount;
using AdaptableDialogAnalyzer.Unity;
using System;
using System.Linq;
using UnityEngine;
using XCharts.Runtime;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_UnitPostCurve : MonoBehaviour
    {
        [Header("Components")]
        public LineChart lineChart;
        [Header("Adapter")]
        public Pixiv_SearchResponseLoader searchResponseLoader;

        CharacterPostCountManager countManager;
        public CharacterPostCountManager CountManager => countManager;

        DateTime startDateTime;
        DateTime endDateTime;


        private void Awake()
        {
            CharacterGetterByPixivTag characterGetterByPixivTag = new CharacterGetterByPixivTag();

            Extra.Pixiv.SearchResponse.MergedResponse mergedResponse = searchResponseLoader.MergedResponse;
            CharacterPostCounter characterPostCounter = new CharacterPostCounter();

            characterPostCounter.getArtworkCharacters = (artwork) =>
            {
                return characterGetterByPixivTag.GetUnitByTag(artwork.tags);
            };
            characterPostCounter.getNovelCharacters = (novel) =>
            {
                return characterGetterByPixivTag.GetUnitByTag(novel.tags);
            };

            CharacterPostCountManager characterPostCountManager = characterPostCounter.Count(mergedResponse);
            countManager = characterPostCountManager.ToTotalMode();

            startDateTime = countManager.days.Keys.Min(d => d);
            endDateTime = countManager.days.Keys.Max(d => d);

            SetCurve();
        }

        void SetCurve()
        {
            XAxis xAxis = lineChart.GetChartComponent<XAxis>();
            xAxis.data.Clear();

            YAxis yAxis = lineChart.GetChartComponent<YAxis>();
            yAxis.data.Clear();


            for (int i = 0; i < 5; i++)
            {
                int unitId = i + 2;
                float lastValue = 0;
                lineChart.series[i].ClearData();
                for (DateTime date = startDateTime; date <= endDateTime; date = date.AddDays(1))
                {
                    long timeStamp = TimeHelper.DateTimeToUnixTimeTST(date);
                    if(countManager.days.ContainsKey(date) && countManager.days[date].characterTotalPairs.ContainsKey(unitId))
                    {
                        var count = countManager.days[date].characterTotalPairs[unitId].Value;
                        lineChart.series[i].AddXYData(timeStamp, count);
                        lastValue = count;
                    }
                    else
                    {
                        lineChart.series[i].AddXYData(timeStamp, lastValue);
                    }
                }
            }
        }
    }
}
