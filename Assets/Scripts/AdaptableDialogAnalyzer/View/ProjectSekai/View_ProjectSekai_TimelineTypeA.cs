using AdaptableDialogAnalyzer.Games.ProjectSekai;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_TimelineTypeA : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform timeLineTransform;
        [Header("Prefabs")]
        public View_ProjectSekai_TimelineTypeA_LabelMonth prefabLabelMonth;
        public View_ProjectSekai_TimelineTypeA_LabelEvent prefabLabelEvent;
        public View_ProjectSekai_TimelineTypeA_LabelBirthday prefabLabelBirthday;
        public View_ProjectSekai_TimelineTypeA_LabelAnniversary prefabLabelAnniversary;
        [Header("Settings")]
        [Tooltip("第一张非重名作品/角色图片的发布时间")]
        public string startDateTimeString = "2020/10/1 00:00:00";
        public float dataFrameWidth = 3f;

        DateTime startDateTime;
        DateTime endDateTime;
        Dictionary<DateTime, int> datetimeIndexes = new Dictionary<DateTime, int>();
        List<View_ProjectSekai_TimelineTypeA_Label> labels = new List<View_ProjectSekai_TimelineTypeA_Label>();
        ProjectSekai_MasterLoader masterLoader;

        public void Initialize(View_ProjectSekai_PixivCharacterPostCount pixivCharacterPostCount)
        {
            masterLoader = pixivCharacterPostCount.masterLoader;

            datetimeIndexes = pixivCharacterPostCount.CountManager.days
                .Select(d => d.Key)
                .OrderBy(d => d)
                .Select((d, i) => new { d, i })
                .ToDictionary(pair => pair.d, pair => pair.i);

            startDateTime = DateTime.Parse(startDateTimeString);
            endDateTime = datetimeIndexes
                .Select(kvp => kvp.Key)
                .OrderByDescending(datetime => datetime)
                .First();

            InstantiateEventLabel();
            InstantiateBirthdayLabel();
            InstantiateMonthLabel();
        }

        DateTime GetNearestDate(DateTime dateTime)
        {
            while (dateTime < endDateTime)
            {
                if (datetimeIndexes.ContainsKey(dateTime.Date))
                {
                    return dateTime.Date;
                }
                dateTime.AddDays(1);
            }
            return DateTime.MinValue;
        }

        void InstantiateMonthLabel()
        {
            DateTime[] dateTimes = datetimeIndexes
                .Select(kvp => kvp.Key)
                .OrderBy(datetime => datetime)
                .ToArray();

            DateTime lastYearMouth = new DateTime();
            foreach (var dateTime in dateTimes)
            {
                if (dateTime < startDateTime) continue;

                // 每个月只显示一次
                if (dateTime.Year != lastYearMouth.Year || dateTime.Month != lastYearMouth.Month)
                {
                    string labelText = dateTime.ToString("yyyy.MM");
                    View_ProjectSekai_TimelineTypeA_LabelMonth labelMonth = Instantiate(prefabLabelMonth, timeLineTransform);
                    labelMonth.SetText(labelText);
                    labelMonth.RectTransform.anchoredPosition = new Vector2(datetimeIndexes[dateTime] * dataFrameWidth, 0f);
                    labels.Add(labelMonth);

                    lastYearMouth = dateTime;
                }
            }
        }

        void InstantiateEventLabel()
        {
            foreach (var masterEvent in masterLoader.MasterEvent)
            {
                DateTime selectedDate = GetNearestDate(masterEvent.StartTime);

                View_ProjectSekai_TimelineTypeA_LabelEvent labelEvent = Instantiate(prefabLabelEvent, timeLineTransform);
                labelEvent.SetData(masterEvent.id);
                labelEvent.RectTransform.anchoredPosition = new Vector2(datetimeIndexes[selectedDate] * dataFrameWidth, 0f);

                labels.Add(labelEvent);
            }
        }

        void InstantiateBirthdayLabel()
        {
            int currentYear = startDateTime.Year;
            while (currentYear <= endDateTime.Year)
            {
                for (int i = 1; i < 21; i++)
                {
                    ProjectSekaiHelper.CharacterInfo characterInfo = ProjectSekaiHelper.characters[i];
                    DateTime birthday = new DateTime(currentYear, characterInfo.birthday.month, characterInfo.birthday.day);

                    if (birthday < startDateTime || birthday > endDateTime) continue;
                    DateTime selectedDate = GetNearestDate(birthday);
                    View_ProjectSekai_TimelineTypeA_LabelBirthday labelBirthday = Instantiate(prefabLabelBirthday, timeLineTransform);
                    labelBirthday.SetData(i);
                    labelBirthday.RectTransform.anchoredPosition = new Vector2(datetimeIndexes[selectedDate] * dataFrameWidth, 0f);

                    labels.Add(labelBirthday);
                }
                currentYear++;
            }
        }
    }
}
