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
    /// 单角色模式选择器
    /// </summary>
    public class ChapterSelectorOMCChara : ChapterSelector
    {
        public Toggle togHideUnmatched;
        [Header("Prefabs")]
        public Window dialogueEditorPrefab;
        [Header("RunTime")]
        public int linesPerFile = 100;
        public string outputPath;

        int speakerId;

        public void Initialize(ObjectMentionedCountManager mentionedCountManager, int speakerId)
        {
            this.speakerId = speakerId;

            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });

            Initialize(mentionedCountManager);
        }

        protected override List<CountMatrix> FilterCountMatrices(List<CountMatrix> countMatrices)
        {
            countMatrices = countMatrices.Where(cm => cm is ObjectMentionedCountMatrix omcm && omcm[speakerId] != null && omcm[speakerId].serifCount > 0).ToList();

            if (togHideUnmatched.isOn)
            {
                countMatrices = countMatrices.Where(cm => cm is ObjectMentionedCountMatrix omcm && omcm[speakerId] != null && omcm[speakerId].matchedIndexes.Count > 0).ToList();
            }

            return countMatrices;
        }

        protected override string GetTip()
        {
            return $"选择剧情 | 单角色模式 | {GlobalConfig.CharacterDefinition[speakerId].name}";
        }

        protected override void InitializeChapterItem(CountMatrix countMatrix, ChapterSelector_ChapterItem chapterItem)
        {
            ObjectMentionedCountMatrix mentionedCountMatrix = countMatrix as ObjectMentionedCountMatrix;

            Vector2Int vector2Int = new Vector2Int(speakerId, mentionedCountMatrix[speakerId]?.Count ?? 0);

            if (vector2Int.y > 0) chapterItem.SetData(countMatrix.Chapter, mentionedCountMatrix[speakerId].serifCount, vector2Int);
            else chapterItem.SetData(countMatrix.Chapter, mentionedCountMatrix[speakerId].serifCount);

            chapterItem.button.onClick.AddListener(() =>
            {
                ObjectMentionCountDialogueEditorChara dialogueEditor = window.OpenWindow<ObjectMentionCountDialogueEditorChara>(dialogueEditorPrefab);
                dialogueEditor.Initialize(mentionedCountMatrix, speakerId);
                dialogueEditor.window.OnClose.AddListener(() => Refresh());
            });
        }

        public void OutputMatchedSerifs()
        {
            BasicTalkSnippet[] getTalkSnippets(MentionedCountMatrix m)
            {
                if (m[speakerId] == null) return new BasicTalkSnippet[0];
                ObjectMentionedCountMatrix omcm = (ObjectMentionedCountMatrix)m;

                return omcm[speakerId].matchedIndexes
                    .Select(i => m.Chapter.TalkSnippets.Where(s => s.RefIdx == i).FirstOrDefault())
                    .Where(s => s != null)
                    .ToArray();
            }

            List<string> serifList = CountManager.CountMatrices
                .Select(cm => (MentionedCountMatrix)cm)
                .OrderBy(m => m.chapterInfo.chapterID)
                .Where(m => m[speakerId] != null)
                .Select(m => (m, getTalkSnippets(m)))
                .SelectMany(t => t.Item2.Select(s => (t.m, s)))
                .Select(t => $"[{t.m.chapterInfo.chapterID}:{t.s.RefIdx}] {t.s.Content.Replace("\n", "")}")
                .ToList();

            int fileCount = (int)Math.Ceiling((double)serifList.Count / linesPerFile);  // 计算需要创建的文件数量

            for (int i = 0; i < fileCount; i++)
            {
                string fileId = (i + 1).ToString("D3");  // 文件ID，使用递增的数字，例如 001、002、003...
                string fileName = $"{speakerId:D2}_{fileId}.txt";  // 文件名，例如 serif_001.txt、serif_002.txt...

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