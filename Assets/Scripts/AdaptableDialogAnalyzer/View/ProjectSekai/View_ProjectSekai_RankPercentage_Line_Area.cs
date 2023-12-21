using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    [RequireComponent(typeof(RectTransform))]
    public class View_ProjectSekai_RankPercentage_Line_Area : MonoBehaviour
    {
        [Header("Components")]
        public Image imgBg;
        public Text txtTitle;
        public Text txtPercent;
        [Header("Settings")]
        public float minDisplayPercent = 0.12f;

        RectTransform rectTransform = null;
        public RectTransform RectTransform => rectTransform ??= GetComponent<RectTransform>();

        public void SetData(float width, int characterId, float percent, int rank = -1)
        {
            imgBg.color = GlobalConfig.CharacterDefinition.Characters[characterId].color;
            txtPercent.text = $"{percent * 100f:F2}%";

            Vector2 sizeDelta = RectTransform.sizeDelta;
            sizeDelta.x = width;
            RectTransform.sizeDelta = sizeDelta;

            if (rank >= 1)
            {
                txtTitle.text = $"第{rank}名持续时间占比";
            }
            else
            {
                txtTitle.gameObject.SetActive(false);
            }

            if (percent < minDisplayPercent)
            {
                txtPercent.gameObject.SetActive(false);
            }
        }
    }
}
