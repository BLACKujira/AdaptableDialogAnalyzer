using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MostAnalysis : MonoBehaviour
    {
        [Header("Components")]
        public View_BanGDream_MostAnalysis_SpineStageArea spineStageArea;
        [Header("Settings")]
        public bool mainCharacterOnly = true;
        public bool passSelf = false;
        public int speakerId = 1;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        MentionedCountManager mentionedCountManager;

        private void Start()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;

            spineStageArea.Initialize(mentionedCountManager,speakerId,mainCharacterOnly);
        }
    }
}