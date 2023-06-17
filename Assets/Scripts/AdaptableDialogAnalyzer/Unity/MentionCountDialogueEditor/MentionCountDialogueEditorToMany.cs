using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public abstract class MentionCountDialogueEditorToMany : MentionCountDialogueEditor
    {
        public Toggle togHideUnmatched;
        [Header("Settings")]
        public Color colorUnmatched = new Color(240, 240, 240, 255);
        public Color colorMatched = new Color(240, 240, 240, 255);
        public Color colorUnidentified = new Color(240, 240, 240, 255);
        [Header("Prefabs")]
        public SpeechBubbleButton speechBubblePrefab;

        protected override SpeechBubbleButton SpeechBubblePrefab => speechBubblePrefab;

        protected override void InitializeSpeechBubble(BasicTalkSnippet basicTalkSnippet, SpeechBubbleButton speechBubbleButton)
        {
            List<Character> characters = GlobalConfig.CharacterDefinition.characters;
            speechBubbleButton.defaultBGColor = colorUnmatched;
            speechBubbleButton.SetData(basicTalkSnippet, false, false);

            //标记此矩阵已被修改
            MentionedCountMatrix.HasChanged = true;
        }
    }
}