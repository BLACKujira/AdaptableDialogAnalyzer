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
        [Header("Settings")]
        public bool mainCharacterOnly = true;
        public bool passSelf = false;
        public int speakerId = 1;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;
        [Header("Prefab")]
        public View_BanGDream_ItemEffect itemEffectPrefab;

        MentionedCountManager mentionedCountManager;

        private void Start()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;

            spineStageArea.Initialize(mentionedCountManager,speakerId,mainCharacterOnly);
            foreach (var item in items)
            {
                View_BanGDream_ItemEffect itemEffect = Instantiate(itemEffectPrefab, tfUIEffect);
                item.Initialize(mentionedCountManager, speakerId, mainCharacterOnly, passSelf, itemEffect);
            }
        }
    }
}