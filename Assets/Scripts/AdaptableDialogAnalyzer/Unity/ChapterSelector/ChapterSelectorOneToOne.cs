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
    public class ChapterSelectorOneToOne : ChapterSelector
    {
        public Toggle togHideUnmatched;
        [Header("Prefabs")]
        public Window dialogueEditorPrefab;
        [Header("RunTime")]
        public int linesPerFile = 100;
        public string outputPath;

        int speakerId;
        int mentionedPersonId;

        public void Initialize(MentionedCountManager mentionedCountManager, int speakerId, int mentionedPersonId)
        {
            this.speakerId = speakerId;
            this.mentionedPersonId = mentionedPersonId;

            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });

            Initialize(mentionedCountManager);
        }

        protected override List<MentionedCountMatrix> FilterCountMatrices(List<MentionedCountMatrix> countMatrices)
        {
            countMatrices = countMatrices.Where(cm => cm[speakerId] != null && cm[speakerId].serifCount > 0).ToList();

            if (togHideUnmatched.isOn)
            {
                countMatrices = countMatrices.Where(cm => cm[speakerId, mentionedPersonId] != null && cm[speakerId, mentionedPersonId].Count > 0).ToList();
            }

            return countMatrices;
        }

        protected override string GetTip()
        {
            return $"选择剧情 | 单角色模式 | {GlobalConfig.CharacterDefinition[speakerId].name} | {GlobalConfig.CharacterDefinition[mentionedPersonId].name}";
        }

        protected override void InitializeChapterItem(MentionedCountMatrix countMatrix, ChapterSelector_ChapterItem chapterItem)
        {
            Vector2Int vector2Int = new Vector2Int(mentionedPersonId, countMatrix[speakerId, mentionedPersonId]?.Count ?? 0);

            if (vector2Int.y > 0) chapterItem.SetData(countMatrix.Chapter, countMatrix[speakerId].serifCount, vector2Int);
            else chapterItem.SetData(countMatrix.Chapter, countMatrix[speakerId].serifCount);

            chapterItem.button.onClick.AddListener(() =>
            {
                MentionCountDialogueEditorOneToOne dialogueEditor = window.OpenWindow<MentionCountDialogueEditorOneToOne>(dialogueEditorPrefab);
                dialogueEditor.Initialize(countMatrix, speakerId, mentionedPersonId);
                dialogueEditor.window.OnClose.AddListener(() => Refresh());
            });
        }

        public void OutputMatchedSerifs()
        {
            Func<MentionedCountMatrix, BasicTalkSnippet[]> getTalkSnippets = m => m[speakerId, mentionedPersonId].matchedIndexes
            .Select(i => m.Chapter.TalkSnippets.Where(s => s.RefIdx == i).FirstOrDefault())
            .Where(s => s != null)
            .ToArray();

            List<string> serifList = MentionedCountManager.mentionedCountMatrices
                .OrderBy(m => m.chapterInfo.chapterID)
                .Where(m => m[speakerId, mentionedPersonId] != null)
                .Select(m => (m, getTalkSnippets(m)))
                .SelectMany(t => t.Item2.Select(s => (t.m, s)))
                .Select(t => $"[{t.m.chapterInfo.chapterID}:{t.s.RefIdx}] {t.s.Content.Replace("\n", "")}")
                .ToList();

            int fileCount = (int)Math.Ceiling((double)serifList.Count / linesPerFile);  // 计算需要创建的文件数量

            for (int i = 0; i < fileCount; i++)
            {
                string fileId = (i + 1).ToString("D3");  // 文件ID，使用递增的数字，例如 001、002、003...
                string fileName = $"{speakerId:D2}_{mentionedPersonId:D2}_{fileId}.txt";  // 文件名，例如 serif_001.txt、serif_002.txt...

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