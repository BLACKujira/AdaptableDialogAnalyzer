using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.UIElements;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MostAnalysis_SpineStageArea : MonoBehaviour
    {
        [Header("Components")]
        public List<View_BanGDream_SpineStage> spineStages;
        public RawImage rimgSpine;
        public UIFollower uIFollowerStar;
        public GraphicsAlphaController alphaController;
        [Header("Settings")]
        public IndexedHDRColorList bandColorList;
        public float fadeTime = 0.5f;
        [Header("Prefab")]
        public View_BanGDream_ItemEffect itemEffectPrefab;

        public void Initialize(MentionedCountManager mentionedCountManager, int speakerId, bool mainCharacterOnly, Transform spineTransform, Transform uiEffectTransform)
        {
            foreach (var spineStage in spineStages)
            {
                spineStage.Initialize(uiEffectTransform);
            }

            List<CharacterMentionStats> characterMentionStatsList = mentionedCountManager.GetMentionStatsList(speakerId, false, true);

            if (mainCharacterOnly)
            {
                characterMentionStatsList = characterMentionStatsList
                    .Where(cms => BanGDreamHelper.IsMainCharacter(cms.MentionedPersonId))
                    .ToList();
            }

            characterMentionStatsList = characterMentionStatsList
                .OrderBy(cms => -cms.Total)
                .ToList();

            for (int i = 0; i < spineStages.Count; i++)
            {
                View_BanGDream_SpineStage spineStage = spineStages[i];

                int modelCharId;

                if (i == 0)
                {
                    modelCharId = speakerId;
                }
                else
                {
                    CharacterMentionStats characterMentionStats = characterMentionStatsList[i - 1];
                    modelCharId = characterMentionStats.MentionedPersonId;
                }

                spineStage.SetModel(modelCharId, spineTransform);
            }

            View_BanGDream_ItemEffect view_BanGDream_ItemEffect = Instantiate(itemEffectPrefab, uiEffectTransform);
            uIFollowerStar.targetTransform = view_BanGDream_ItemEffect.transform;
            view_BanGDream_ItemEffect.materialController.HDRColor = bandColorList[(int)BanGDreamHelper.GetCharacterBand(speakerId)];

            alphaController.Alpha = 0;
            rimgSpine.color = new Color(1, 1, 1, 0);
        }
    
        public void FadeIn()
        {
            alphaController.DoFade(1, fadeTime);
            rimgSpine.DOFade(1, fadeTime);
            foreach (var spineStage in spineStages)
            {
                spineStage.FadeIn();
            }
        }
    }
}