using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using AdaptableDialogAnalyzer.Unity;
using System.Collections;
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
        public float fadeTime = 0.2f;
        public float fadeInterval = 0.067f;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        List<GraphicsAlphaController> alphaControllers = new List<GraphicsAlphaController>();
        MentionedCountManager mentionedCountManager;

        public void Start()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;

            List<CharacterMentionStats> characterMentionStatsList = mentionedCountManager.GetMentionStatsList(speakerId, true, true);

            characterMentionStatsList = characterMentionStatsList
                .OrderBy(cms => -cms.Total)
                .ToList();

            int mentionTotal = characterMentionStatsList.Sum(cms => cms.Total);
            int serifCount = mentionedCountManager.CountSerif(speakerId);

            for (int i = 0; i < items.Count; i++)
            {
                View_ReStage_CharacterMentions_Item item = items[i];
                alphaControllers.Add(item.GetComponent<GraphicsAlphaController>());
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
            alphaControllers.Add(itemTotal.GetComponent<GraphicsAlphaController>());

            itemTotal.SetData(speakerId, mentionTotal, serifCount);

            foreach (var alphaController in alphaControllers)
            {
                if (alphaController != null) alphaController.Alpha = 0;
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(CoPlay());
            }
        }

        IEnumerator CoPlay()
        {
            foreach (var alphaController in alphaControllers)
            {
                if (alphaController != null)
                {
                    alphaController.DoFade(1, fadeTime);
                }
                yield return new WaitForSeconds(fadeInterval);
            }
        }
    }
}