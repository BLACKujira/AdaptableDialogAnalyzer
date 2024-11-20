using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class ChapterSelector_ChapterItem : MonoBehaviour
    {
        [Header("Components")]
        public Button button;
        public Text txtName;
        public Text txtTotal;
        public EquidistantLayoutGenerator elgMentionCount;
        [Header("Settings")]
        public int maxMentionCountPanels = 4;

        public void SetData(ChapterInfo chapterInfo, int total)
        {
            SetData(chapterInfo, total, new KeyValuePair<Color, int>[0]);
        }

        /// <summary>
        /// total为显示在最右边的数字，mentionCounts的x表示角色（的代表色）y表示数字
        /// </summary>
        public void SetData(ChapterInfo chapterInfo, int total, params Vector2Int[] mentionCounts)
        {
            KeyValuePair<Color, int>[] keyValuePairs = mentionCounts
                .Select(m => new KeyValuePair<Color, int>(GlobalConfig.CharacterDefinition[m.x].color, m.y))
                .ToArray();
            SetData(chapterInfo, total, keyValuePairs);
        }

        /// <summary>
        /// total为显示在最右边的数字，mentionCounts的x表示角色（的代表色）y表示数字
        /// </summary>
        //[Obsolete]
        //public void SetData(Chapter chapter, int total, params KeyValuePair<Color, int>[] mentionCounts)
        //{
        //    ChapterInfo chapterInfo = new ChapterInfo(chapter);
        //    chapterInfo.chapterType = chapter.ChapterType;
        //    chapterInfo.chapterID = chapter.ChapterID;
        //    chapterInfo.chapterTitle = chapter.ChapterTitle;
        //    SetData(chapterInfo, total, mentionCounts);
        //}

        /// <summary>
        /// total为显示在最右边的数字，mentionCounts的x表示角色（的代表色）y表示数字
        /// </summary>
        public void SetData(ChapterInfo chapterInfo, int total, params KeyValuePair<Color,int>[] mentionCounts)
        {
            txtName.text = $"{chapterInfo.chapterTitle} [{chapterInfo.chapterID}]";
            txtTotal.text = total.ToString();

            //生成表示提及角色的小UI元素
            elgMentionCount.Generate(Mathf.Min(mentionCounts.Length, maxMentionCountPanels), (gobj, id) =>
            {
                KeyValuePair<Color, int> keyValuePair = mentionCounts[id];

                TextWithIndividualColor textWithIndividualColor = gobj.GetComponent<TextWithIndividualColor>();
                textWithIndividualColor.text.text = keyValuePair.Value.ToString();
                textWithIndividualColor.IndividualColorElement.SetIndividualColor(keyValuePair.Key);
            });

            //如果超过最大显示数量，则添加一个带省略号的小UI元素
            if (mentionCounts.Length > maxMentionCountPanels)
            {
                elgMentionCount.AddItem((gobj) =>
                {
                    TextWithIndividualColor textWithIndividualColor = gobj.GetComponent<TextWithIndividualColor>();
                    textWithIndividualColor.text.text = "...";
                    textWithIndividualColor.IndividualColorElement.SetIndividualColor(Color.white);
                });
            }
        }
    }
}