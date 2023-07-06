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
        public bool onlyLoadExistChapter;

        /// <summary>
        /// 每个统计矩阵对应的文件，value：矩阵保存位置
        /// </summary>
        Dictionary<MentionedCountMatrix,string> savePathDictionary = new Dictionary<MentionedCountMatrix, string>();

        MentionedCountManager mentionedCountManager = null;
        public MentionedCountManager MentionedCountManager
        {
            get 
            {
                if(mentionedCountManager == null) Initialize();
                return mentionedCountManager;
            }
        }

        public int ChangedMatricesCount => mentionedCountManager.mentionedCountMatrices.Where(m=>m.HasChanged).Count();

        private void Initialize()
        {
            List<MentionedCountMatrix> countMatrices = new List<MentionedCountMatrix>();
            chapterLoader.Initialize();

            string[] files = Directory.GetFiles(saveFolder);
            foreach (string file in files) 
            {
                if (Path.GetExtension(file).ToLower().Equals(".mcm"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (onlyLoadExistChapter && !chapterLoader.HasChapter(fileName)) continue;

                    string rawMatrix = File.ReadAllText(file);
                    MentionedCountMatrix countMatrix = JsonUtility.FromJson<MentionedCountMatrix>(rawMatrix);
                    countMatrices.Add(countMatrix);
                    savePathDictionary[countMatrix] = file;
                }
            }

            mentionedCountManager = new MentionedCountManager();
            foreach (var countMatrix in countMatrices)
            {
                Chapter chapter = chapterLoader.GetChapter(countMatrix.chapterInfo.chapterID);
                if (chapter == null) Debug.LogWarning($"加载章节{countMatrix.chapterInfo.chapterID}失败，可能文件已不存在");
                countMatrix.Chapter = chapter;

                mentionedCountManager.mentionedCountMatrices.Add(countMatrix);
            }
        }
    
        public int SaveChangedMatrices()
        {
            int count = 0;
            foreach (var countMatrix in MentionedCountManager.mentionedCountMatrices)
            {
                if (!countMatrix.HasChanged) continue;
                
                string savePath = savePathDictionary[countMatrix];
                string saveData = JsonUtility.ToJson(countMatrix);
                File.WriteAllText(savePath, saveData);

                countMatrix.HasChanged = false;
                count++;
            }
            Debug.Log($"{count} 个文件已更改");
            return count;
        }
    }
}