using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace AdaptableDialogAnalyzer.UIElements
{
    public class SpeechBubble : MonoBehaviour
    {
        [Header("Component")]
        public Image imgIcon;
        public Image imgNameBG;
        public Text txtName;
        public Image imgContentBG;
        public Text txtContent;
        [Header("Component")]
        public Color defaultBGColor = new Color32(240, 240, 240, 255);
        public Color colorWhite = Color.white;
        public Color colorBlack = new Color32(68, 68, 102, 255);

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

            txtName.text = basicTalkSnippet.DisplayName;
            txtContent.text = basicTalkSnippet.Content;
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
            imgNameBG.color = color;
            txtName.color = APCA.GetMostVisibleColor(color, colorWhite, colorBlack);
        }

        /// <summary>
        /// 设置显示文字的背景色
        /// </summary>
        /// <param name="color"></param>
        public void SetContentBGColor(Color color)
        {
            imgContentBG.color = color;
            txtContent.color = APCA.GetMostVisibleColor(color, colorWhite, colorBlack);
        }
    }
}
