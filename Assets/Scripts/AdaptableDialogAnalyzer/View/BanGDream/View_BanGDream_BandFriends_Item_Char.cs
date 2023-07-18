using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_BandFriends_Item_Char : MonoBehaviour
    {
        [Header("Components")]
        public Image imgBG;
        public Image imgChar;
        public Text txtCount;
        [Header("Settings")]
        public IndexedColorList textColorList;
        public IndexedSpriteList charSpiteList;

        public void SetData(int speakerId, int count)
        {
            Character character = GlobalConfig.CharacterDefinition[speakerId];
            imgBG.color = character.color;
            imgChar.sprite = charSpiteList[speakerId];
            txtCount.color = textColorList[speakerId];
            txtCount.text = count.ToString();
        }
    }
}