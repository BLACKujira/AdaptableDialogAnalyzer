﻿using AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount;
using AdaptableDialogAnalyzer.Unity;
using AdaptableDialogAnalyzer.Unity.UIElements;
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
        [Header("Optional Components")]
        public RectTransform nsfwBarTransform;
        public ManualGrowWidthLabel lblDelta;
        public Text txtName;
        [Header("Settings")]
        public SpriteList iconList;
        public float nsfwBarMultiple = 3;
        public StringList nameList;

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

                if(lblDelta)
                {
                    lblDelta.Text = $"+{characterPostCountDayItem.delta:0}";
                }

                if(txtName)
                {
                    txtName.text = nameList[characterPostCountDayItem.characterId];
                }

                imgIcon.sprite = iconList[characterPostCountDayItem.characterId];
                lblTotal.Text = ((int)characterPostCountDayItem.total).ToString();
                individualColorElement.SetIndividualColor(GlobalConfig.CharacterDefinition.Characters[characterPostCountDayItem.characterId].color);
            }
            else
            {
                Debug.LogError("数据类型错误");
                return;
            }
        }
    }
}
