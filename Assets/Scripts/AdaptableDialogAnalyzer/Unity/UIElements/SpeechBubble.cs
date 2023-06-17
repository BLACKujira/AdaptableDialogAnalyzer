using AdaptableDialogAnalyzer.Unity;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace AdaptableDialogAnalyzer.UIElements
{

    public class SpeechBubble : MonoBehaviour
    {
        [Header("Component")]
        public IndividualColorElement iceName;
        public IndividualColorElement iceContent;
        public Image imgIcon;
        public Text txtName;
        public Text txtContent;
        [Header("Color")]
        public Color defaultBGColor = new Color32(240, 240, 240, 255);
        [Header("Settings")]
        public int maxTextLengthPerLine = 40;

        BasicTalkSnippet basicTalkSnippet;
        public BasicTalkSnippet BasicTalkSnippet => basicTalkSnippet;

        /// <summary>
        /// 设置对话显示内容
        /// </summary>
        /// <param name="basicTalkSnippet"></param>
        /// <param name="setColor"></param>
        public void SetData(BasicTalkSnippet basicTalkSnippet, bool setNameBGColor = true, bool setContentBGColor = true)
        {
            if (basicTalkSnippet == null)
            {
                Debug.LogError("Invalid BasicTalkSnippet.");
                return;
            }

            string content = basicTalkSnippet.Content;
            //只有在原文中没有换行符的情况下才会自动换行
            if (maxTextLengthPerLine > 0 && !content.Contains("\n"))
            {
                StringBuilder stringBuilder = new StringBuilder(content);

                int index = maxTextLengthPerLine;
                while (index < stringBuilder.Length)
                {
                    stringBuilder.Insert(index, "\n");
                    index += maxTextLengthPerLine + "\n".Length;
                }

                content = stringBuilder.ToString();
            }

            txtName.text = string.IsNullOrEmpty(basicTalkSnippet.DisplayName) ? GlobalConfig.CharacterDefinition[basicTalkSnippet.SpeakerId].name : basicTalkSnippet.DisplayName;
            txtContent.text = content;
            imgIcon.sprite = GlobalConfig.CharacterDefinition[basicTalkSnippet.SpeakerId].icon;

            if (setNameBGColor) { SetNameBGColor(GlobalConfig.CharacterDefinition[basicTalkSnippet.SpeakerId].color); }
            else { SetNameBGColor(defaultBGColor); }

            if (setContentBGColor) { SetContentBGColor(GlobalConfig.CharacterDefinition[basicTalkSnippet.SpeakerId].color); }
            else { SetContentBGColor(defaultBGColor); }
        }

        /// <summary>
        /// 设置显示名称的背景色
        /// </summary>
        /// <param name="color"></param>
        public void SetNameBGColor(Color color)
        {
            iceName.SetIndividualColor(color);
        }

        /// <summary>
        /// 设置显示文字的背景色
        /// </summary>
        /// <param name="color"></param>
        public void SetContentBGColor(Color color)
        {
            iceContent.SetIndividualColor(color);
        }
    }
}
