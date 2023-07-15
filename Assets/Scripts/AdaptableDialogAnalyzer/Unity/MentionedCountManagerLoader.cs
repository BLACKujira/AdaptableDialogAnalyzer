using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 通过此组件加载统计数据，并与剧情关联
    /// </summary>
    public class MentionedCountManagerLoader : MonoBehaviour
    {
        public string saveFolder;
        public ChapterLoader chapterLoader;
        [Header("Settings")]
        public SerializeType serializeType;
        public bool onlyLoadExistChapter;
        public bool doNotLoadChapter;

        /// <summary>
        /// 每个统计矩阵对应的文件，value：矩阵保存位置
        /// </summary>
        Dictionary<MentionedCountMatrix, string> savePathDictionary = new Dictionary<MentionedCountMatrix, string>();

        MentionedCountManager mentionedCountManager = null;
        public MentionedCountManager MentionedCountManager
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
            List<MentionedCountMatrix> countMatrices = new List<MentionedCountMatrix>();

            if (!doNotLoadChapter) chapterLoader.Initialize();

            string[] files = Directory.GetFiles(saveFolder);
            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToLower().Equals(".mcm"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (!doNotLoadChapter && onlyLoadExistChapter && !chapterLoader.HasChapter(fileName)) continue;

                    MentionedCountMatrix countMatrix = MentionedCountMatrix.Deserialize(file, serializeType);

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

            mentionedCountManager = new MentionedCountManager();
            mentionedCountManager.mentionedCountMatrices = countMatrices;
        }

        public int SaveChangedMatrices()
        {
            int count = 0;
            foreach (var countMatrix in MentionedCountManager.mentionedCountMatrices)
            {
                if (!countMatrix.HasChanged) continue;

                string savePath = savePathDictionary[countMatrix];
                countMatrix.Serialize(savePath, serializeType);

                countMatrix.HasChanged = false;
                count++;
            }
            Debug.Log($"{count} 个文件已更改");
            return count;
        }
    }
}