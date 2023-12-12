using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_TimelineTypeA_Bottom : MonoBehaviour
    {
        public View_ProjectSekai_TimelineTypeA timeline;
        [Header("Components")]
        public RectTransform contentTransform;
        [Header("Settings")]
        public float eventBottomLength = 170;
        public float birthdayBottomLength = 80;
        [Header("Prefab")]
        public View_ProjectSekai_TimelineTypeA_BottomLabel bottomLabelPrefab;
        [Header("Adapter")]
        public ProjectSekai_MasterLoader masterLoader;

        public void Initialize()
        {
            timeline.OnGenerate += Timeline_OnGenerate;
        }

        private void Timeline_OnGenerate(View_ProjectSekai_TimelineTypeA_Label label)
        {
            // 标签的颜色，如果为null则不生成标签
            Color? color = null;
            float length = 0;

            if (label is View_ProjectSekai_TimelineTypeA_LabelEvent labelEvent)
            {
                MasterEventStory masterEventStory = masterLoader.EventStories
                    .Where(es => es != null && es.eventId == labelEvent.EventID)
                    .FirstOrDefault();
                if (masterEventStory == null)
                {
                    Debug.Log($"在EventStories中找不到 EventID {labelEvent.EventID} 对应的信息");
                    return;
                }
                if(masterEventStory.bannerGameCharacterUnitId <= 0 || masterEventStory.bannerGameCharacterUnitId > 56)
                {
                    Debug.Log($"EventID {labelEvent.EventID} 不支持的角色：{masterEventStory.bannerGameCharacterUnitId}");
                    return;
                }


                int bannerGameCharacterUnitId = ConstData.MergeVirtualSinger(masterEventStory.bannerGameCharacterUnitId);
                color = GlobalConfig.CharacterDefinition.Characters[bannerGameCharacterUnitId].color;
                length = eventBottomLength;
            }
            else if (label is View_ProjectSekai_TimelineTypeA_LabelBirthday labelBirthday)
            {
                color = GlobalConfig.CharacterDefinition.Characters[labelBirthday.CharacterID].color;
                length = birthdayBottomLength;
            }

            // 如果没有找到对应的颜色，就不生成
            if (color == null) return;

            View_ProjectSekai_TimelineTypeA_BottomLabel bottomLabel = Instantiate(bottomLabelPrefab, contentTransform);
            bottomLabel.RectTransform.anchoredPosition = new Vector2(label.RectTransform.anchoredPosition.x, 0);
            bottomLabel.SetData((Color)color);
            bottomLabel.RectTransform.sizeDelta = new Vector2(length, bottomLabel.RectTransform.sizeDelta.y);
        }
    }
}
