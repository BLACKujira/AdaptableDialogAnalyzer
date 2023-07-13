using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.UIElements;
using AdaptableDialogAnalyzer.Unity;
using AdaptableDialogAnalyzer.View.ReStage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_CharacterMentions : MonoBehaviour
    {
        [Header("Components")]
        public List<View_BanGDream_CharacterMentions_Item> items;
        [Header("Settings")]
        public bool mainCharacterOnly = true;
        public bool passSelf = false;
        public int speakerId = 1;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        MentionedCountManager mentionedCountManager;

        public void Start()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;

            List<CharacterMentionStats> characterMentionStatsList = mentionedCountManager.GetMentionStatsList(speakerId, false, passSelf);

            if (mainCharacterOnly)
            {
                characterMentionStatsList = characterMentionStatsList
                    .Where(cms => BanGDreamHelper.IsMainCharacter(cms.MentionedPersonId))
                    .ToList();
            }

            characterMentionStatsList = characterMentionStatsList
                .OrderBy(cms => -cms.Total)
                .ToList();

            int mentionTotal = characterMentionStatsList.Sum(cms => cms.Total);
            int serifCount = mentionedCountManager.CountSerif(speakerId);

            for (int i = 0; i < items.Count; i++)
            {
                View_BanGDream_CharacterMentions_Item item = items[i];
                if (i >= characterMentionStatsList.Count)
                {
                    item.gameObject.SetActive(false);
                }
                else
                {
                    CharacterMentionStats characterMentionStats = characterMentionStatsList[i];
                    item.SetData(characterMentionStats.MentionedPersonId, characterMentionStats.Total, mentionTotal);
                }
            }
        }
    }
}