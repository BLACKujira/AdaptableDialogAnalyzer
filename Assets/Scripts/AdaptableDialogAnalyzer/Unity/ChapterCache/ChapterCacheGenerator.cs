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
            Progress = "���ڶ�ȡ�½��ļ�";
            Chapter[] chapters = chapterLoader.Chapters;
            
            Priority = 0.5f;

            for (int i = 0; i < chapters.Length; i++)
            {
                Chapter chapter = chapters[i];
                Progress = $"���ڱ����½ڻ��� {chapter.ChapterID}";
                Priority = 0.5f + (float)i / chapters.Length * 0.5f;

                ChapterCache chapterCache = new ChapterCache(chapter);
                string cache = chapterCache.GetSerializedData();
                string saveFile = Path.Combine(savePath, chapter.ChapterID + ".cc");

                if(File.Exists(saveFile)) 
                {
                    Debug.LogWarning($"�Ѵ���idΪ{saveFile}�ľ��黺�棬֮ǰ���ļ��������ǣ��������ID���ɷ�ʽ����������ظ�ID");
                }
                File.WriteAllText(saveFile, cache);
            }

            Priority = 1;
            Progress = "���";
        }
    }
}