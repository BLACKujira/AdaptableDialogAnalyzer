using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MostAnalysis : MonoBehaviour
    {
        [Header("Components")]
        public View_BanGDream_MostAnalysis_SpineStageArea spineStageArea;
        public List<View_BanGDream_MostAnalysis_Item> items;
        public Transform tfUIEffect;
        public Transform tfSpine;
        [Header("Settings")]
        public bool mainCharacterOnly = true;
        public bool passSelf = false;
        public int speakerId = 1;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        MentionedCountManager mentionedCountManager;

        public void Initialize()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;

            spineStageArea.Initialize(mentionedCountManager, speakerId, mainCharacterOnly, tfSpine, tfUIEffect);
            foreach (var item in items)
            {
                item.Initialize(mentionedCountManager, speakerId, mainCharacterOnly, passSelf, tfUIEffect);
            }
        }

        public void FadeIn()
        {
            spineStageArea.FadeIn();
            foreach (var item in items)
            {
                item.FadeIn();
            }
        }
    }
}