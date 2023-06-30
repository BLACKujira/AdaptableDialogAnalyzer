using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_RankingKataomoi : MonoBehaviour
    {
        [Header("Components")]
        public List<View_ReStage_Ranking_TriadItem> items;
        [Header("Settings")]
        public bool passZero = true;
        public bool passSelf = true;
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
            characterMentionStatsPairs = characterMentionStatsPairs.OrderBy(p => -p.GetRatioDifference(mentionedCountManager, passZero, passSelf)).ToList();

            for (int i = 0; i < items.Count; i++)
            {
                View_ReStage_Ranking_TriadItem item = items[i];
                if (i > characterMentionStatsPairs.Count)
                {
                    item.gameObject.SetActive(false);
                    continue;
                }

                CharacterMentionStatsPair characterMentionStatsPair = characterMentionStatsPairs[i];
                float differenceAToB = (float)characterMentionStatsPair.StatsAToB.Total / mentionedCountManager.CountMentionTotal(characterMentionStatsPair.CharacterAId, passZero, passSelf);
                float differenceBToA = (float)characterMentionStatsPair.StatsBToA.Total / mentionedCountManager.CountMentionTotal(characterMentionStatsPair.CharacterBId, passZero, passSelf);

                if (differenceBToA > differenceAToB)
                {
                    characterMentionStatsPair.Swap();
                    (differenceAToB, differenceBToA) = (differenceBToA, differenceAToB);
                }

                char padChar = '　';

                string namaeA = characterDefinition[characterMentionStatsPair.CharacterAId].Namae.PadRight(padWidth, padChar);
                string namaeB = characterDefinition[characterMentionStatsPair.CharacterBId].Namae.PadRight(padWidth, padChar);
                string colorA = ColorUtility.ToHtmlStringRGB(characterDefinition[characterMentionStatsPair.CharacterAId].color);
                string colorB = ColorUtility.ToHtmlStringRGB(characterDefinition[characterMentionStatsPair.CharacterBId].color);

                item.SetData(
                    characterMentionStatsPair.CharacterAId,
                    characterMentionStatsPair.CharacterBId,
                    $"{Mathf.Abs(differenceAToB - differenceBToA) * 100:00.0}%",
                    $"<color=#{colorA}>{namaeA}</color> → <color=#{colorB}>{namaeB}</color> {differenceAToB * 100:00.0}%",
                    $"<color=#{colorB}>{namaeB}</color> → <color=#{colorA}>{namaeA}</color> {differenceBToA * 100:00.0}%");
            }
        }
    }
}