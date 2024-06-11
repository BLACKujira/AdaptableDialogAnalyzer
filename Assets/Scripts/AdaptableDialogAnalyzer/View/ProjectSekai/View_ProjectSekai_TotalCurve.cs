﻿using AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount;
using AdaptableDialogAnalyzer.Games.ProjectSekai;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_TotalCurve : MonoBehaviour
    {
        [Header("Components")]
        public LineChart lineChart;
        public List<Text> labels;
        [Header("Settings")]
        public SupportLanguage language = SupportLanguage.zhs;

        public enum SupportLanguage { zhs, jp, en };

        CharacterPostCountManager countManager;

        public void Initialize(CharacterPostCountManager countManager)
        {
            this.countManager = countManager;

            Initialize_LineChart();
            Initialize_Labels();
        }

        private void Initialize_LineChart()
        {
            var data = new List<(DateTime date, int count)>();
            data = countManager.days
                .Select(kvp => (kvp.Key, (int)kvp.Value.characterTotalPairs[1].Value))
                .OrderBy(t => t.Key)
                .ToList();

            XAxis xAxis = lineChart.GetChartComponent<XAxis>();
            xAxis.data.Clear();

            YAxis yAxis = lineChart.GetChartComponent<YAxis>();
            yAxis.data.Clear();

            lineChart.series[0].ClearData();
            foreach (var (date, count) in data)
            {
                long timeStamp = TimeHelper.DateTimeToUnixTimeTST(date);
                lineChart.series[0].AddXYData(timeStamp, count);
            }
        }

        private void Initialize_Labels()
        {
            int[] countDatas = new int[4];

            countDatas[0] = (int)countManager.days
                .Where(kvp => kvp.Key < ProjectSekaiHelper.anniversary0)
                .Max(kvp => kvp.Value.characterTotalPairs[1].Value);

            countDatas[1] = (int)countManager.days
                .Where(kvp => kvp.Key >= ProjectSekaiHelper.anniversary0 && kvp.Key < ProjectSekaiHelper.anniversary1)
                .Max(kvp => kvp.Value.characterTotalPairs[1].Value);

            countDatas[2] = (int)countManager.days
                .Where(kvp => kvp.Key >= ProjectSekaiHelper.anniversary1 && kvp.Key < ProjectSekaiHelper.anniversary2)
                .Max(kvp => kvp.Value.characterTotalPairs[1].Value);

            countDatas[3] = (int)countManager.days
                .Where(kvp => kvp.Key >= ProjectSekaiHelper.anniversary2)
                .Max(kvp => kvp.Value.characterTotalPairs[1].Value);

            switch (language)
            {
                case SupportLanguage.zhs:
                    Initialize_Labels_Zhs(countDatas);
                    break;
                case SupportLanguage.jp:
                    Initialize_Labels_Jp(countDatas);
                    break;
                case SupportLanguage.en:
                    Initialize_Labels_En(countDatas);
                    break;
            }
        }

        void Initialize_Labels_Zhs(int[] countDatas)
        {
            labels[0].text = $"开服前\n作品数: {countDatas[0]}";
            labels[1].text = $"第一年\n作品数: {countDatas[1] - countDatas[0]}";
            labels[2].text = $"第二年\n作品数: {countDatas[2] - countDatas[1]}";
            labels[3].text = $"第三年\n作品数: {countDatas[3] - countDatas[2]}";
        }

        void Initialize_Labels_Jp(int[] countDatas)
        {
            labels[0].text = $"オープン前\n投稿数: {countDatas[0]}";
            labels[1].text = $"1年目\n投稿数: {countDatas[1] - countDatas[0]}";
            labels[2].text = $"2年目\n投稿数: {countDatas[2] - countDatas[1]}";
            labels[3].text = $"3年目\n投稿数: {countDatas[3] - countDatas[2]}";
        }

        void Initialize_Labels_En(int[] countDatas)
        {
            labels[0].text = $"Before start\nWorks: {countDatas[0]}";
            labels[1].text = $"First year\nWorks: {countDatas[1] - countDatas[0]}";
            labels[2].text = $"Second year\nWorks: {countDatas[2] - countDatas[1]}";
            labels[3].text = $"Third year\nWorks: {countDatas[3] - countDatas[2]}";
        }
    }
}
