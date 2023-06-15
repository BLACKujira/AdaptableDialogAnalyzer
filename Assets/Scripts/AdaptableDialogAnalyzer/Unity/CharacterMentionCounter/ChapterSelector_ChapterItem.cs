using AdaptableDialogAnalyzer.UIElements;
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

        /// <summary>
        /// total为显示在最右边的数字，mentionCounts的x表示角色（的代表色）y表示数字
        /// </summary>
        public void SetData(Chapter chapter, int total, Vector2Int[] mentionCounts)
        {
            txtName.text = $"{chapter.chapterTitle} [{chapter.chapterID}]";
            txtTotal.text = total.ToString();

            //生成表示提及角色的小UI元素
            elgMentionCount.Generate(Mathf.Min(mentionCounts.Length, maxMentionCountPanels), (gobj, id) =>
            {
                Vector2Int vector2Int = mentionCounts[id];

                TextWithIndividualColor textWithIndividualColor = gobj.GetComponent<TextWithIndividualColor>();
                textWithIndividualColor.text.text = vector2Int.y.ToString();
                textWithIndividualColor.IndividualColorElement.SetIndividualColor(GlobalConfig.CharacterDefinition.characters[vector2Int.x].color);
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