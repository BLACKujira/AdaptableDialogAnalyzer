using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MostAnalysis_SpineStageArea : MonoBehaviour
    {
        [Header("Components")]
        public List<View_BanGDream_SpineStage> spineStages;

        public void Initialize(MentionedCountManager mentionedCountManager, int speakerId, bool mainCharacterOnly)
        {
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

                spineStage.SetModel(modelCharId);
            }
        }
    }
}