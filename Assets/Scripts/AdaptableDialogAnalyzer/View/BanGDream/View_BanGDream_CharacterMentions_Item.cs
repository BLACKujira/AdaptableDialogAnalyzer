using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_CharacterMentions_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imgBG;
        public Image imgSdChara;
        public Text txtCount;
        public Text txtPercent;
        [Header("Settings")]
        public IndexedSpriteList sdCharaList;
        public IndexedColorList bgColorList;
        public IndexedColorList textColorList;

        public void SetData(int characterID, int count, int total)
        {
            imgSdChara.sprite = sdCharaList[characterID];
            txtCount.text = count.ToString();
            txtPercent.text = $"{((float)count / total) * 100:00.00}%";

            imgBG.color = bgColorList[characterID];
            txtCount.color = textColorList[characterID];
            txtPercent.color = textColorList[characterID];
        }
    }
}