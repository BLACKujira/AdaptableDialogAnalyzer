using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class ObjectMentionedCountManagerLoader : MonoBehaviour
    {
        public string saveFolder;
        public ChapterLoader chapterLoader;
        [Header("Settings")]
        public bool onlyLoadExistChapter;
        public bool doNotLoadChapter;

        /// <summary>
        /// 每个统计矩阵对应的文件，value：矩阵保存位置
        /// </summary>
        Dictionary<ObjectMentionedCountMatrix, string> savePathDictionary = new Dictionary<ObjectMentionedCountMatrix, string>();

        ObjectMentionedCountManager mentionedCountManager = null;
        public ObjectMentionedCountManager MentionedCountManager
        {
            get
            {
                if (mentionedCountManager == null) Initialize();
                return mentionedCountManager;
            }
        }

        public int ChangedMatricesCount => mentionedCountManager.mentionedCountMatrices.Where(m => m.HasChanged).Count();

        private void Initialize()
        {
            List<ObjectMentionedCountMatrix> countMatrices = new List<ObjectMentionedCountMatrix>();

            if (!doNotLoadChapter) chapterLoader.Initialize();

            string[] files = Directory.GetFiles(saveFolder);
            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToLower().Equals(".omcm"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (!doNotLoadChapter && onlyLoadExistChapter && !chapterLoader.HasChapter(fileName)) continue;

                    ObjectMentionedCountMatrix countMatrix = ObjectMentionedCountMatrix.LoadAndDeserialize(file);

                    countMatrices.Add(countMatrix);
                    savePathDictionary[countMatrix] = file;
                }
            }

            if (!doNotLoadChapter)
            {
                foreach (var countMatrix in countMatrices)
                {
                    Chapter chapter = chapterLoader.GetChapter(countMatrix.chapterInfo.chapterID);
                    if (chapter == null) Debug.LogWarning($"加载章节{countMatrix.chapterInfo.chapterID}失败，可能文件已不存在");
                    countMatrix.Chapter = chapter;
                }
            }

            mentionedCountManager = new ObjectMentionedCountManager();
            mentionedCountManager.mentionedCountMatrices = countMatrices;
        }

        public int SaveChangedMatrices()
        {
            int count = 0;
            foreach (var countMatrix in MentionedCountManager.mentionedCountMatrices)
            {
                if (!countMatrix.HasChanged) continue;

                countMatrix.RemoveEmptyRows();

                string savePath = savePathDictionary[countMatrix];
                countMatrix.SerializeAndSave(savePath);

                countMatrix.HasChanged = false;
                count++;
            }
            Debug.Log($"{count} 个文件已更改");
            return count;
        }
    }
}