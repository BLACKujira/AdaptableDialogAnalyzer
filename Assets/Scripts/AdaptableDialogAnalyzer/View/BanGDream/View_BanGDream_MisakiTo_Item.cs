using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MisakiTo_Item : MonoBehaviour
    {
        [Header("Components")]
        public CanvasGroup canvasGroup;
        public UIFollower uIFollower;
        [Header("Components2")]
        public Image imgEdge;
        public Image imgIcon;
        public Text txtMisaki;
        public Text txtMichelle;
        [Header("Effect")]
        public View_BanGDream_ItemEffect fadeInEffect;
        public IndexedHDRColorList hdrColorList;
        [Header("Settings")]
        public float fadeDuration;
        public IndexedColorList edgeColorList;


        int misakiId = 15;
        int michelleId = 601;
        View_BanGDream_ItemEffect itemEffect;

        public void Initialize(Transform effectTransform)
        {
            itemEffect = Instantiate(fadeInEffect, effectTransform);
            uIFollower.TargetTransform = itemEffect.transform;
        }

        public void SetData(int characterId,int countMisaki,int countMichelle)
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            Character character = characterDefinition[characterId];
            imgEdge.color = edgeColorList[characterId];
            imgIcon.sprite = character.icon;
            txtMisaki.text = countMisaki.ToString();
            txtMichelle.text = countMichelle.ToString();

            float colorLerpT = (float)(characterId - 1) / 34;
            Color particleColor = Color.Lerp(hdrColorList[misakiId], hdrColorList[michelleId],colorLerpT);
            itemEffect.materialController.HDRColor = particleColor;

            canvasGroup.alpha = 0;
        }

        public void FadeIn()
        {
            canvasGroup.DOFade(1, fadeDuration);
            itemEffect.particle.Play();
        }
    }
}