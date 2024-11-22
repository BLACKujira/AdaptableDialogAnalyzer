using AdaptableDialogAnalyzer.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    //用于多次物体提及统计的数字输入组件，可以显示原始匹配信息
    public class OMCMInputInt : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Text txtMatch;
        public Text txtSerif;
        public InputField ifOriInt;
        public InputField ifNewInt;
        [Header("Settings")]
        public Color colorMatched;
        public Color colorUndefined;

        Action<int> onApply;

        public void Initialize(string serif, ObjectMentionedCountMutiGrid grid, ObjectMentionedCountMutiGrid gridUndefined, Action<int> onApply)
        {
            this.onApply = onApply;

            txtMatch.text = grid.overrideCount != 0 ? "原始匹配（已弃用）" : "原始匹配";
            txtSerif.text = InsertRichTextToSerif(serif, grid, gridUndefined);
            ifOriInt.text = grid.Count.ToString();
            ifNewInt.text = grid.Count.ToString();
        }

        public void Apply()
        {
            onApply(int.Parse(ifNewInt.text));
            window.Close();
        }

        /// <summary>
        /// 通过富文本标记出匹配的位置
        /// </summary>
        private string InsertRichTextToSerif(string serif, ObjectMentionedCountMutiGrid grid, ObjectMentionedCountMutiGrid gridUndefined)
        {
            // 将匹配信息放入字典中，等待排序
            Dictionary<RegexCapture,bool> captureAndUndefinedDic = new Dictionary<RegexCapture,bool>();
            if (grid != null)
            {
                foreach (var regexCapture in grid.regexCaptures)
                {
                    captureAndUndefinedDic.Add(regexCapture, false);
                }
            }
            if (gridUndefined != null)
            {
                foreach (var regexCapture in gridUndefined.regexCaptures)
                {
                    captureAndUndefinedDic.Add(regexCapture, true);
                }
            }

            // 按startIndex排序匹配信息
            KeyValuePair<RegexCapture, bool>[] captureAndUndefinedArray = captureAndUndefinedDic.OrderBy(c => c.Key.index).ToArray();

            int currentOffset = 0; // 当前已插入的字符数
            string outSerif = serif;

            // 插入富文本标签
            foreach (var captureAndUndefined in captureAndUndefinedArray)
            {
                Color color = captureAndUndefined.Value ? colorUndefined : colorMatched;
                string strStart = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
                string strEnd = "</color>";

                outSerif = outSerif.Insert(captureAndUndefined.Key.index + currentOffset, strStart);
                currentOffset += strStart.Length;
                outSerif = outSerif.Insert(captureAndUndefined.Key.index + captureAndUndefined.Key.length  + currentOffset, strEnd);
                currentOffset += strEnd.Length;
            }

            return outSerif;
        }
    }
}