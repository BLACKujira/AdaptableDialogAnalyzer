using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{

    public class View_ProjectSekai_RankPercentage_Line : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform rtBar;
        [Header("Settings")]
        public float minDisplayPercent = 0.1f;
        public SpriteList iconList;
        [Header("Prefabs")]
        public View_ProjectSekai_RankPercentage_Line_Area areaPrefab;
        public Image iconPrefab;

        public void SetData(int rank, List<KeyValuePair<int, float>> dataList)
        {
            dataList = dataList
                .Where(kvp => kvp.Value > minDisplayPercent)
                .OrderByDescending(kvp => kvp.Value)
                .ToList();

            float addPercent = 0;
            foreach (var data in dataList)
            {
                int characterId = data.Key;
                float percent = data.Value;

                View_ProjectSekai_RankPercentage_Line_Area area = Instantiate(areaPrefab, rtBar);
                area.SetData(rtBar.sizeDelta.x * percent, characterId, percent, addPercent == 0 ? rank : -1);
                area.RectTransform.anchoredPosition = new Vector2(rtBar.sizeDelta.x * addPercent, area.RectTransform.anchoredPosition.y);

                Image icon = Instantiate(iconPrefab, transform);
                icon.sprite = iconList[data.Key];

                RectTransform iconTransform = icon.GetComponent<RectTransform>();
                iconTransform.anchoredPosition = new Vector2(rtBar.sizeDelta.x * addPercent, iconTransform.anchoredPosition.y);

                addPercent += percent;
            }
        }
    }
}
