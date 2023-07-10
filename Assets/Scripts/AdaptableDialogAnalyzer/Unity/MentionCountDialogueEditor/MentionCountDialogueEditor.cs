using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// ��ʾ�Ի����ṩ�༭���ܣ��̳д�����ʹ�ò�ͬ��ɸѡ��ʽ�Ͱ�ť����
    /// </summary>
    public abstract class MentionCountDialogueEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Text txtTip;
        public RectTransform rtScrollContent;

        protected static bool hideUnmatched = true;

        MentionedCountMatrix mentionedCountMatrix;
        List<SpeechBubbleButton> speechBubbles = new List<SpeechBubbleButton>();

        protected abstract SpeechBubbleButton SpeechBubblePrefab { get; }
        protected MentionedCountMatrix MentionedCountMatrix => mentionedCountMatrix;

        protected void Initialize(MentionedCountMatrix mentionedCountMatrix)
        {
            this.mentionedCountMatrix = mentionedCountMatrix;
            txtTip.text = GetTip();
            Refresh();
            window.OnReShow.AddListener(() => Refresh());
        }

        /// <summary>
        /// ˢ�£����ڳ�ʼ�������´򿪴��ں͸ı�ɸѡ��ʽʱ����
        /// </summary>
        protected void Refresh()
        {
            ClearSpeechBubbles();
            Chapter chapter = mentionedCountMatrix.Chapter;
            List<BasicTalkSnippet> basicTalkSnippets = new List<BasicTalkSnippet>(chapter.TalkSnippets);

            basicTalkSnippets = FilterTalkSnippets(basicTalkSnippets);

            foreach (var basicTalkSnippet in basicTalkSnippets)
            {
                SpeechBubbleButton speechBubbleButton = Instantiate(SpeechBubblePrefab, rtScrollContent);
                InitializeSpeechBubble(basicTalkSnippet, speechBubbleButton);
                speechBubbles.Add(speechBubbleButton);
            }

            RefreshLayout();
        }

        /// <summary>
        /// �ֶ��������ּ���
        /// </summary>
        /// <returns></returns>
        protected void RefreshLayout()
        {
            StartCoroutine(CoRefreshLayout());
        }

        /// <summary>
        /// �ֶ��������ּ���
        /// </summary>
        /// <returns></returns>
        IEnumerator CoRefreshLayout()
        {
            yield return 1;
            yield return 1;
            rtScrollContent.gameObject.SetActive(false);
            rtScrollContent.gameObject.SetActive(true);
        }

        private void ClearSpeechBubbles()
        {
            foreach (var speechBubble in speechBubbles)
            {
                Destroy(speechBubble.gameObject);
            }
            speechBubbles.Clear();
        }

        /// <summary>
        /// ��ʼ��ÿһ���Ի�����Ԫ��
        /// </summary>
        protected abstract void InitializeSpeechBubble(BasicTalkSnippet basicTalkSnippet, SpeechBubbleButton speechBubbleButton);

        /// <summary>
        /// ���½ǵ���ʾ
        /// </summary>
        protected abstract string GetTip();

        /// <summary>
        /// ɸѡ��ʾ�ĶԻ�
        /// </summary>
        protected abstract List<BasicTalkSnippet> FilterTalkSnippets(List<BasicTalkSnippet> talkSnippets);
    }
}