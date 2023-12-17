using AdaptableDialogAnalyzer.Games.ProjectSekai;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_TimelineTypeA : MonoBehaviour
    {
        public enum LabelType { Month, Event, Birthday, Anniversary }

        [Header("Components")]
        public RectTransform timeLineTransform;
        [Header("Prefabs")]
        public View_ProjectSekai_TimelineTypeA_LabelMonth prefabLabelMonth;
        public View_ProjectSekai_TimelineTypeA_LabelEvent prefabLabelEvent;
        public View_ProjectSekai_TimelineTypeA_LabelBirthday prefabLabelBirthday;
        public View_ProjectSekai_TimelineTypeA_LabelAnniversary prefabLabelAnniversary;
        [Header("Optional Components")]
        public View_ProjectSekai_TimelineTypeA_Bottom bottom;
        [Header("Settings")]
        [Tooltip("第一张非重名作品/角色图片的发布时间")]
        public string startDateTimeString = "2020/10/1 00:00:00";
        public float dataFrameWidth = 3f;
        [Header("Adapter")]
        public ProjectSekai_MasterLoader masterLoader;

        DateTime startDateTime;
        DateTime endDateTime;
        Dictionary<DateTime, int> datetimeIndexes = new Dictionary<DateTime, int>();
        List<View_ProjectSekai_TimelineTypeA_Label> labels = new List<View_ProjectSekai_TimelineTypeA_Label>();

        public event Action<View_ProjectSekai_TimelineTypeA_Label> OnGenerate;

        public void Initialize(DateTime[] days)
        {
            bottom.Initialize();

            datetimeIndexes = days
                .OrderBy(d => d)
                .Select((d, i) => new { d, i })
                .ToDictionary(pair => pair.d, pair => pair.i);

            startDateTime = DateTime.Parse(startDateTimeString);
            endDateTime = datetimeIndexes
                .Select(kvp => kvp.Key)
                .OrderByDescending(datetime => datetime)
                .First();

            InstantiateAnniversaryLabel();
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

                    OnGenerate?.Invoke(labelMonth);

                    lastYearMouth = dateTime;
                }
            }
        }

        void InstantiateEventLabel()
        {
            foreach (var masterEvent in masterLoader.Events)
            {
                if (masterEvent.StartTime > endDateTime) return;

                DateTime selectedDate = GetNearestDate(masterEvent.StartTime);

                View_ProjectSekai_TimelineTypeA_LabelEvent labelEvent = Instantiate(prefabLabelEvent, timeLineTransform);
                labelEvent.SetData(masterEvent.id);
                labelEvent.RectTransform.anchoredPosition = new Vector2(datetimeIndexes[selectedDate] * dataFrameWidth, 0f);

                OnGenerate?.Invoke(labelEvent);

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

                    OnGenerate.Invoke(labelBirthday);

                    labels.Add(labelBirthday);
                }
                currentYear++;
            }
        }

        void InstantiateAnniversaryLabel()
        {
            DateTime[] anniversaries = new DateTime[] { ProjectSekaiHelper.anniversary1, ProjectSekaiHelper.anniversary2, ProjectSekaiHelper.anniversary3 };
            for (int i = 0; i < anniversaries.Length; i++)
            {
                if (anniversaries[i] < startDateTime || anniversaries[i] > endDateTime) continue;
                DateTime selectedDate = GetNearestDate(anniversaries[i]);
                View_ProjectSekai_TimelineTypeA_LabelAnniversary labelAnniversary = Instantiate(prefabLabelAnniversary, timeLineTransform);
                labelAnniversary.SetData(i + 1);
                labelAnniversary.RectTransform.anchoredPosition = new Vector2(datetimeIndexes[selectedDate] * dataFrameWidth, 0f);
            }
        }

        public void SetPosition(float dataFrame)
        {
            float targetPosition = -dataFrame * dataFrameWidth;
            timeLineTransform.anchoredPosition = new Vector2(targetPosition, 0f);
        }
    }
}
