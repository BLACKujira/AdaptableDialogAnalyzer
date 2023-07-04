using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using AdaptableDialogAnalyzer.Unity;
using System.Collections;
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
        public float fadeTime = 0.3f;
        public float fadeInterval = 0.067f;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        List<GraphicsAlphaController> alphaControllers = new List<GraphicsAlphaController>();
        List<RuntimeScaleController> scaleControllers = new List<RuntimeScaleController>();
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

                char padChar = '��';

                string namaeA = characterDefinition[characterMentionStatsPair.CharacterAId].Namae.PadRight(padWidth, padChar);
                string namaeB = characterDefinition[characterMentionStatsPair.CharacterBId].Namae.PadRight(padWidth, padChar);
                string colorA = ColorUtility.ToHtmlStringRGB(characterDefinition[characterMentionStatsPair.CharacterAId].color);
                string colorB = ColorUtility.ToHtmlStringRGB(characterDefinition[characterMentionStatsPair.CharacterBId].color);

                item.SetData(
                    characterMentionStatsPair.CharacterAId,
                    characterMentionStatsPair.CharacterBId,
                    characterMentionStatsPair.Sum.ToString(),
                    $"<color=#{colorA}>{namaeA}</color> �� <color=#{colorB}>{namaeB}</color> {totalAToB}",
                    $"<color=#{colorB}>{namaeB}</color> �� <color=#{colorA}>{namaeA}</color> {totalBToA}");
            }

            alphaControllers = items.Select(i => i.GetComponent<GraphicsAlphaController>()).ToList();
            scaleControllers = items.Select(i => i.GetComponent<RuntimeScaleController>()).ToList();

            foreach (var alphaController in alphaControllers)
            {
                if (alphaController != null)
                    alphaController.Alpha = 0;
            }
            foreach (var scaleController in scaleControllers)
            {
                if (scaleController != null)
                    scaleController.ScaleRatio = 0;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(CoPlay());
            }
        }

        IEnumerator CoPlay()
        {
            for (int i = 0; i < alphaControllers.Count; i++)
            {
                GraphicsAlphaController alphaController = alphaControllers[i];
                RuntimeScaleController scaleController = scaleControllers[i];
                if (alphaController != null)
                {
                    alphaController.DoFade(1, fadeTime);
                }
                if (scaleController != null)
                {
                    scaleController.DoScale(1, fadeTime);
                }
                yield return new WaitForSeconds(fadeInterval);
            }
        }
    }
}