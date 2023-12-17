using AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount;
using AdaptableDialogAnalyzer.Unity;
using AdaptableDialogAnalyzer.Unity.UIElements;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_PixivCharacterPostCount_Bar : AutoSortBarChart_Bar
    {
        [Header("Components")]
        public Image imgIcon;
        public ManualGrowWidthLabel lblTotal;
        public IndividualColorElement individualColorElement;
        public CanvasGroup cgIncrease;
        [Header("Optional Components")]
        public RectTransform nsfwBarTransform;
        public ManualGrowWidthLabel lblDelta;
        public Text txtName;
        public View_ProjectSekai_PixivCharacterPostCount_Bar_Light lightGlow;
        [Header("Settings")]
        public SpriteList iconList;
        public float nsfwBarMultiple = 3;
        public StringList nameList;
        public IndexedHDRColorList glowLightList;
        public float increaseFadeDuration = 1;
        public float lightFadeDuration = 1;

        CharacterPostCountDayItem characterPostCountDayItem => (CharacterPostCountDayItem)CurrentData;
        float glowLimitation = 1;

        public void Initialize(IAutoSortBarChartData data, bool glowOn = false)
        {
            if (data is CharacterPostCountDayItem characterPostCountDayItem)
            {
                imgIcon.sprite = iconList[characterPostCountDayItem.characterId];
                individualColorElement.SetIndividualColor(GlobalConfig.CharacterDefinition.Characters[characterPostCountDayItem.characterId].color);
                if (lightGlow)
                {
                    lightGlow.hDRColorTexture.HDRColor = glowLightList[characterPostCountDayItem.characterId];
                }
                glowLimitation = glowOn ? 1 : 0;
            }
            else
            {
                Debug.LogError("数据类型错误");
                return;
            }
        }

        public override void SetData(IAutoSortBarChartData data, float valueMax)
        {
            base.SetData(data, valueMax);
            if (data is CharacterPostCountDayItem characterPostCountDayItem)
            {
                // 设置NSFW条长度
                if (nsfwBarTransform)
                {
                    Vector2 sizeDelta = barTransform.sizeDelta;
                    if (direction == Direction2.Horizontal)
                    {
                        float x = characterPostCountDayItem.total == 0 ? 0 : sizeDelta.x * characterPostCountDayItem.nsfwCount / characterPostCountDayItem.total;
                        x *= nsfwBarMultiple;
                        nsfwBarTransform.sizeDelta = new Vector2(x, nsfwBarTransform.sizeDelta.y);
                    }
                    else
                    {
                        float y = characterPostCountDayItem.total == 0 ? 0 : sizeDelta.y * characterPostCountDayItem.nsfwCount / characterPostCountDayItem.total;
                        y *= nsfwBarMultiple;
                        nsfwBarTransform.sizeDelta = new Vector2(nsfwBarTransform.sizeDelta.x, y);
                    }
                }

                if (lblDelta)
                {
                    lblDelta.Text = $"+{characterPostCountDayItem.delta:0}";
                }

                if (txtName)
                {
                    txtName.text = nameList[characterPostCountDayItem.characterId];
                }

                if (lightGlow)
                {
                    Color glowColor = lightGlow.image.color;
                    glowColor.a = GetGlowAlpha() * glowLimitation;
                    lightGlow.image.color = glowColor;
                }

                lblTotal.Text = ((int)characterPostCountDayItem.total).ToString();
            }
            else
            {
                Debug.LogError("数据类型错误");
                return;
            }
        }

        float GetGlowAlpha()
        {
            if (characterPostCountDayItem == null)
            {
                return 0;
            }

            float deltaOfDelta = Mathf.Max(0, characterPostCountDayItem.deltaOfDelta);

            // 以增加的百分比计算亮度
            float increasePercent = deltaOfDelta / characterPostCountDayItem.Value;
            float mulValue0 = 20f;
            float mulValue1 = Mathf.Min((deltaOfDelta - 5) / 10f, 10) * 0.1f; // 如果增加的数量大于5，那么达到最大亮度
            mulValue1 = Mathf.Max(0, mulValue1);
            float mulValue2 = (CurrentValueMax / characterPostCountDayItem.Value + 1) * .8f; // 补偿低人气角色的亮度

            float alphaPercent = increasePercent * mulValue0 * mulValue1 * mulValue2;

            // 以增加的数量 计算亮度
            float addValue = (deltaOfDelta - 20) * 0.01f;

            float alphaDelta = addValue;

            // 按一定比例混合
            alphaPercent = Mathf.Clamp01(alphaPercent);
            alphaDelta = Mathf.Clamp01(alphaDelta);
            float alpha = Mathf.Clamp01(alphaPercent * .3f + alphaDelta * .7f);

            return alpha;
        }

        public void FadeIncrease(float alpha)
        {
            cgIncrease.DOFade(alpha, increaseFadeDuration);
        }

        public void ToggleIncrease(float alpha)
        {
            cgIncrease.alpha = alpha;
        }

        public void ToggleGlow(bool on)
        {
            if (on)
            {
                DOTween.To(() => glowLimitation, value => glowLimitation = value, 1f, lightFadeDuration);
            }
            else
            {
                DOTween.To(() => glowLimitation, value => glowLimitation = value, 0f, lightFadeDuration);
            }
        }
    }
}
