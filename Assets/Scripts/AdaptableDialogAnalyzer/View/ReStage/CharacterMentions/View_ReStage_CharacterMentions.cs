using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_CharacterMentions : MonoBehaviour
    {
        [Header("Components")]
        public List<View_ReStage_CharacterMentions_Item> items;
        public View_ReStage_CharacterMentions_ItemTotal itemTotal;
        [Header("Settings")]
        public bool passZero = true;
        public bool passSelf = true;
        public int speakerId = 1;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        MentionedCountManager mentionedCountManager;

        public void Start()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;

            List<CharacterMentionStats> characterMentionStatsList = mentionedCountManager.GetMentionStatsList(speakerId, true, true);

            characterMentionStatsList = characterMentionStatsList
                .OrderBy(cms=>-cms.Total)
                .ToList();

            int mentionTotal = characterMentionStatsList.Sum(cms => cms.Total);
            int serifCount = mentionedCountManager.CountSerif(speakerId);

            for (int i = 0; i < items.Count; i++)
            {
                View_ReStage_CharacterMentions_Item item = items[i];
                if(i>=characterMentionStatsList.Count)
                {
                    item.gameObject.SetActive(false);
                }
                else
                {
                    CharacterMentionStats characterMentionStats = characterMentionStatsList[i];
                    item.SetData(characterMentionStats.MentionedPersonId, characterMentionStats.Total, mentionTotal);
                }
            }

            itemTotal.SetData(speakerId, mentionTotal, serifCount);
        }

    }
}