using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_CardIcon : MonoBehaviour
    {
        [Header("Components")]
        public Image imgFrame;
        public Image imgAttribute;
        public RawImage rimgCardPreview;
        [Header("Settings")]
        public IndexedSpriteList frameSpriteList;
        public StringIndexedSpriteList r1frameSpriteList;
        public StringIndexedSpriteList attributeSpriteList;

        public void SetData(MasterCharacterSituation card)
        {
            if (card.Rarity == 1)
            {
                imgFrame.sprite = r1frameSpriteList[card.Attribute];
            }
            else
            {
                imgFrame.sprite = frameSpriteList[(int)card.Rarity];
            }
            imgAttribute.sprite = attributeSpriteList[card.Attribute];
        }

    }
}