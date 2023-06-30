using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_RankingSum : MonoBehaviour
    {
        [Header("Components")]
        public List<View_ReStage_Ranking_TriadItem> items;
        [Header("Settings")]
        public bool passZero = true;
        public int padWidth = 3;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        MentionedCountManager mentionedCountManager;

        private void Start()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;

            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            Character[] characters = characterDefinition.Characters;

            List<CharacterMentionStatsPair> characterMentionStatsPairs = mentionedCountManager.GetCharacterMentionStatsPairs(passZero);
            characterMentionStatsPairs = characterMentionStatsPairs.OrderBy(p => -p.Sum).ToList();

            for (int i = 0; i < items.Count; i++)
            {
                View_ReStage_Ranking_TriadItem item = items[i];
                if (i > characterMentionStatsPairs.Count)
                {
                    item.gameObject.SetActive(false);
                    continue;
                }

                CharacterMentionStatsPair characterMentionStatsPair = characterMentionStatsPairs[i];
                int totalAToB = characterMentionStatsPair.StatsAToB.Total;
                int totalBToA = characterMentionStatsPair.StatsBToA.Total;

                if (totalBToA > totalAToB)
                {
                    characterMentionStatsPair.Swap();
                    (totalAToB, totalBToA) = (totalBToA, totalAToB);
                }

                char padChar = '¡¡';

                string namaeA = characterDefinition[characterMentionStatsPair.CharacterAId].Namae.PadRight(padWidth, padChar);
                string namaeB = characterDefinition[characterMentionStatsPair.CharacterBId].Namae.PadRight(padWidth, padChar);
                string colorA = ColorUtility.ToHtmlStringRGB(characterDefinition[characterMentionStatsPair.CharacterAId].color);
                string colorB = ColorUtility.ToHtmlStringRGB(characterDefinition[characterMentionStatsPair.CharacterBId].color);

                item.SetData(
                    characterMentionStatsPair.CharacterAId,
                    characterMentionStatsPair.CharacterBId,
                    characterMentionStatsPair.Sum.ToString(),
                    $"<color=#{colorA}>{namaeA}</color> ¡ú <color=#{colorB}>{namaeB}</color> {totalAToB}",
                    $"<color=#{colorB}>{namaeB}</color> ¡ú <color=#{colorA}>{namaeA}</color> {totalBToA}");
            }
        }
    }
}