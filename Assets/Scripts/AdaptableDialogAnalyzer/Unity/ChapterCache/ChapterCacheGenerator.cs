using System.IO;
using System.Threading;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class ChapterCacheGenerator : TaskWindow
    {
        [Header("Path")]
        public string savePath;
        [Header("Adapter")]
        public ChapterLoader chapterLoader;
        
        private Thread countThread;

        private void Start()
        {
            countThread = new Thread(() => Generate());
            countThread.Start();
        }

        private void OnDestroy()
        {
            countThread.Abort();
        }

        void Generate()
        {
            Progress = "正在读取章节文件";
            Chapter[] chapters = chapterLoader.Chapters;
            
            Priority = 0.5f;

            for (int i = 0; i < chapters.Length; i++)
            {
                Chapter chapter = chapters[i];
                Progress = $"正在保存章节缓存 {chapter.ChapterID}";
                Priority = 0.5f + (float)i / chapters.Length * 0.5f;

                ChapterCache chapterCache = new ChapterCache(chapter);
                string cache = chapterCache.GetSerializedData();
                string saveFile = Path.Combine(savePath, chapter.ChapterID + ".cc");

                if(File.Exists(saveFile)) 
                {
                    Debug.LogWarning($"已存在id为{saveFile}的剧情缓存，之前的文件将被覆盖，请检查剧情ID生成方式，避免出现重复ID");
                }
                File.WriteAllText(saveFile, cache);
            }

            Priority = 1;
            Progress = "完成";
        }
    }
}