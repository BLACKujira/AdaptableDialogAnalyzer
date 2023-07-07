using AdaptableDialogAnalyzer.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class DialogueViewer : MonoBehaviour
    {
        [Header("Component")]
        public RectTransform rtScrollContent;
        [Header("Prefab")]
        public SpeechBubble speechBubblePrefab;
        [Header("Adapter")]
        public SimpleChapterLoader chapterLoader;

        Chapter chapter;
        List<SpeechBubble> speechBubbles = new List<SpeechBubble>();

        public void Start()
        {
            chapter = chapterLoader.GetChapter();
            BasicTalkSnippet[] basicTalkSnippets = chapter.TalkSnippets;
            foreach (var basicTalkSnippet in basicTalkSnippets)
            {
                SpeechBubble speechBubble = Instantiate(speechBubblePrefab, rtScrollContent);
                speechBubble.SetData(basicTalkSnippet, true, true);
                speechBubbles.Add(speechBubble);
            }
        }
    }
}