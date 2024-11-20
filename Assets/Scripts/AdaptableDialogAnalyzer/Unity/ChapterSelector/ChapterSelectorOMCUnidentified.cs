using AdaptableDialogAnalyzer.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 歧义模式选择器
    /// </summary>
    public class ChapterSelectorOMCUnidentified : ChapterSelector
    {
        public Toggle togHideUnmatched;
        [Header("Prefabs")]
        public Window dialogueEditorPrefab;
        [Header("RunTime")]
        public int linesPerFile = 100;
        public string outputPath;
        [Header("Color")]
        public Color colorUnidentified = new Color32(246, 107, 176, 255);

        public void Initialize(ObjectMentionedCountManager mentionedCountManager)
        {
            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });

            base.Initialize(mentionedCountManager);
        }

        protected override List<CountMatrix> FilterCountMatrices(List<CountMatrix> countMatrices)
        {
            if (togHideUnmatched.isOn)
            {
                countMatrices = countMatrices.Where(cm => cm is ObjectMentionedCountMatrix omcm && !omcm.NoUnidentifiedMatch).ToList();
            }

            return countMatrices;
        }

        protected override string GetTip()
        {
            return $"选择剧情 | 歧义模式";
        }

        protected override void InitializeChapterItem(CountMatrix countMatrix, ChapterSelector_ChapterItem chapterItem)
        {
            ObjectMentionedCountMatrix mentionedCountMatrix = countMatrix as ObjectMentionedCountMatrix;

            List<KeyValuePair<Color, int>> panelData = new List<KeyValuePair<Color, int>>();
            if (!mentionedCountMatrix.NoUnidentifiedMatch) panelData.Add(new KeyValuePair<Color, int>(colorUnidentified, mentionedCountMatrix.unidentifiedMentionsRow.Count));
            chapterItem.SetData(countMatrix.chapterInfo, mentionedCountMatrix.Total, panelData.ToArray());

            chapterItem.button.onClick.AddListener(() =>
            {
                ObjectMentionCountDialogueEditorUnidentified dialogueEditor = window.OpenWindow<ObjectMentionCountDialogueEditorUnidentified>(dialogueEditorPrefab);
                dialogueEditor.Initialize(mentionedCountMatrix);
                dialogueEditor.window.OnClose.AddListener(() => Refresh());
            });
        }

        public void OutputMatchedSerifs()
        {
            BasicTalkSnippet[] getTalkSnippets(ObjectMentionedCountMatrix m)
            {
                if (m.NoMatch) return new BasicTalkSnippet[0];
                return m.MatchedRefIdxSet
                    .Select(i => m.Chapter.TalkSnippets.Where(s => s.RefIdx == i).FirstOrDefault())
                    .Where(s => s != null)
                    .ToArray();
            }

            List<string> serifList = CountManager.CountMatrices
                .Select(cm => (ObjectMentionedCountMatrix)cm)
                .OrderBy(m => m.chapterInfo.chapterID)
                .Where(m => !m.NoUnidentifiedMatch)
                .Select(m => (m, getTalkSnippets(m)))
                .SelectMany(t => t.Item2.Select(s => (t.m, s)))
                .Select(t => $"[{t.m.chapterInfo.chapterID}:{t.s.RefIdx}] {t.s.Content.Replace("\n", "")}")
                .ToList();

            int fileCount = (int)Math.Ceiling((double)serifList.Count / linesPerFile);  // 计算需要创建的文件数量

            for (int i = 0; i < fileCount; i++)
            {
                string fileId = (i + 1).ToString("D3");  // 文件ID，使用递增的数字，例如 001、002、003...
                string fileName = $"{fileId}.txt";  // 文件名，例如 serif_001.txt、serif_002.txt...

                // 计算当前文件需要写入的行数范围
                int startLine = i * linesPerFile;
                int endLine = Math.Min(startLine + linesPerFile, serifList.Count);

                // 构建当前文件的内容
                StringBuilder sb = new StringBuilder();
                for (int j = startLine; j < endLine; j++)
                {
                    sb.AppendLine(serifList[j]);
                }

                // 将内容写入文件
                File.WriteAllText(Path.Combine(outputPath, fileName), sb.ToString());
            }

        }
    }
}