using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 对话编辑器的基类 
    /// </summary>
    public abstract class CountDialogueEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Text txtTip;
        public RectTransform rtScrollContent;

        protected static bool hideUnmatched = true;

        List<SpeechBubbleButton> speechBubbles = new List<SpeechBubbleButton>();
        protected List<SpeechBubbleButton> SpeechBubbles => speechBubbles;

        protected abstract SpeechBubbleButton SpeechBubblePrefab { get; }

        CountMatrix countMatrix;
        protected CountMatrix CountMatrix => countMatrix;

        protected void Initialize(CountMatrix countMatrix)
        {
            this.countMatrix = countMatrix;
            txtTip.text = GetTip();
            Refresh();
            window.OnReShow.AddListener(() => Refresh());
        }

        /// <summary>
        /// 刷新，仅在初始化、重新打开窗口和改变筛选方式时调用
        /// </summary>
        protected void Refresh()
        {
            ClearSpeechBubbles();
            Chapter chapter = countMatrix.Chapter;
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
        /// 手动触发布局计算
        /// </summary>
        /// <returns></returns>
        protected void RefreshLayout()
        {
            StartCoroutine(CoRefreshLayout());
        }

        /// <summary>
        /// 手动触发布局计算
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
        /// 初始化每一个对话气泡元素
        /// </summary>
        protected abstract void InitializeSpeechBubble(BasicTalkSnippet basicTalkSnippet, SpeechBubbleButton speechBubbleButton);

        /// <summary>
        /// 左下角的提示
        /// </summary>
        protected abstract string GetTip();

        /// <summary>
        /// 筛选显示的对话
        /// </summary>
        protected abstract List<BasicTalkSnippet> FilterTalkSnippets(List<BasicTalkSnippet> talkSnippets);
    }
}