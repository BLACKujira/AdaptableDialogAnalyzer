using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public abstract class View_BanGDream_MostAnalysis_Item : MonoBehaviour
    {
        [Header("Fade")]
        public float fadeTime = 0.5f;
        [Header("Components")]
        public View_BanGDream_MostAnalysis_Item_InfoBar infoBar;
        public Text txtDescription;
        public GraphicsAlphaController alphaController;

        public void FadeIn()
        {
            infoBar.FadeIn();
            alphaController.DoFade(1, fadeTime);
        }

        public void Initialize(MentionedCountManager mentionedCountManager, int speakerId, bool mainCharacterOnly, bool passSelf, Transform effectTransform)
        {
            infoBar.Initialize(effectTransform);
            Initialize(mentionedCountManager, speakerId, mainCharacterOnly, passSelf);
            alphaController.Alpha = 0;
        }

        protected abstract void Initialize(MentionedCountManager mentionedCountManager, int speakerId, bool mainCharacterOnly, bool passSelf);
    }

    public abstract class View_BanGDream_MostAnalysis_ItemTypeA : View_BanGDream_MostAnalysis_Item
    {
        public Image imgSdCharL;
        public Image imgSdCharR;
        [Header("Settings")]
        public IndexedSpriteList sdCharaList;

        protected void SetSdChara(int characterLID, int characterRID)
        {
            imgSdCharL.sprite = sdCharaList[characterLID];
            imgSdCharR.sprite = sdCharaList[characterRID];
        }
    }

    public abstract class View_BanGDream_MostAnalysis_ItemTypeB : View_BanGDream_MostAnalysis_Item
    {
        public RawImage rimgIcon;
    }
}