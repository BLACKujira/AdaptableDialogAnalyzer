using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using AdaptableDialogAnalyzer.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_RankingOshi : MonoBehaviour
    {
        [Header("Components")]
        public List<View_ReStage_RankingOshi_Item> items;
        [Header("Settings")]
        public bool passZero = true;
        public bool passSelf = true;
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

            Dictionary<int, int> metionTotalMap = new Dictionary<int, int>();

            List<CharacterMentionStats> characterMentionStatsList = new List<CharacterMentionStats>();
            foreach (Character character in characters)
            {
                if (passZero && character.id == 0) continue;
                List<CharacterMentionStats> characterMentionStats = mentionedCountManager.GetMentionStatsList(character.id, passZero, passSelf);

                metionTotalMap[character.id] = mentionedCountManager.CountMentionTotal(character.id, passZero, passSelf);

                characterMentionStats = characterMentionStats
                    .OrderBy(s => -(float)s.Total / metionTotalMap[character.id])
                    .ToList();
                characterMentionStatsList.Add(characterMentionStats[0]);
            }

            characterMentionStatsList = characterMentionStatsList
                .OrderBy(s => -(float)s.Total / metionTotalMap[s.SpeakerId])
                .ToList();

            for (int i = 0; i < items.Count; i++)
            {
                View_ReStage_RankingOshi_Item item = items[i];
                if (i >= characterMentionStatsList.Count)
                {
                    item.gameObject.SetActive(false);
                    continue;
                }

                CharacterMentionStats characterMentionStats = characterMentionStatsList[i];
                int mentionCount = characterMentionStats.Total;
                int mentionTotal = metionTotalMap[characterMentionStats.SpeakerId];

                item.SetData(
                    characterMentionStats.SpeakerId,
                    characterMentionStats.MentionedPersonId,
                    mentionCount,
                    mentionTotal);
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
                if(scaleController != null)
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