using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    [RequireComponent(typeof(RectTransform))]
    public class View_BanGDream_CharacterMentions_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imgBG;
        public Image imgSdChara;
        public Text txtCount;
        public Text txtPercent;
        public Image imgInitMask;
        public UIFollower uIFollower;
        [Header("Settings")]
        public IndexedSpriteList sdCharaList;
        public IndexedColorList bgColorList;
        public IndexedColorList textColorList;
        public IndexedHDRColorList hdrColorList;
        public float colorFadeTime = 1f;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        View_BanGDream_ItemEffect itemEffect;
        int characterID;

        public void Initialize(View_BanGDream_ItemEffect itemEffect)
        {
            this.itemEffect = itemEffect;
            uIFollower.TargetTransform = itemEffect.transform;
        }

        public void SetData(int characterID, int count, int total)
        {
            this.characterID = characterID;

            imgSdChara.sprite = sdCharaList[characterID];
            txtCount.text = count.ToString();
            txtPercent.text = $"{((float)count / total) * 100:00.00}%";

            imgBG.color = bgColorList[characterID];
            txtCount.color = textColorList[characterID];
            txtPercent.color = textColorList[characterID];
            itemEffect.materialController.HDRColor = hdrColorList[characterID];
            
            imgInitMask.color = Color.white;
        }
        
        public void FadeIn()
        {
            Character character = GlobalConfig.CharacterDefinition[characterID];
            Color endColor = character.color;
            endColor.a = 0;

            itemEffect.particle.Play();
            imgInitMask.DOColor(endColor,colorFadeTime);
        }
    }
}