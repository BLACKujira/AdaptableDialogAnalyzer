using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 其他组件通过此组件获取故事章节，可以一次读取篇剧情，可能会通过其存放文件夹判断剧情类型
    /// 需要设置剧情ID、获取标题和判断剧情类型，这对接下来的开发很重要
    /// </summary>
    /// <typeparam name="T">由原始文件生成的对象类型</typeparam>
    public abstract class ChapterLoader : MonoBehaviour
    {
        Chapter[] chapters = null;
        public Chapter[] Chapters
        {
            get
            {
                if (chapters == null)
                {
                    Initialize();
                }
                return chapters;
            }
        }

        /// <summary>
        /// key：剧情ID，value：剧情
        /// </summary>
        Dictionary<string , Chapter> chapterDictionary = new Dictionary<string, Chapter>();

        public void Initialize()
        {
            if(chapters != null)
            {
                Debug.Log("请勿重复调用初始化函数");
                return;
            }

            chapters = InitializeChapters();
            foreach (Chapter chapter in chapters) 
            {
                if(chapterDictionary.ContainsKey(chapter.ChapterID))
                {
                    Debug.LogWarning($"剧情ID\"{chapter.ChapterID}\"已重复，先前加载的相同ID剧情被覆盖");
                }
                chapterDictionary[chapter.ChapterID] = chapter;
            }
            chapters = chapterDictionary.Values.ToArray();
        }

        /// <summary>
        /// 重载此函数以加载Chapter
        /// </summary>
        public abstract Chapter[] InitializeChapters();

        /// <summary>
        /// 通过ID获取剧情
        /// </summary>
        public Chapter GetChapter(string chapterID)
        {
            if(chapters == null) Initialize();
            if(chapterDictionary.ContainsKey(chapterID)) return chapterDictionary[chapterID];
            return null;
        }

        /// <summary>
        /// 是否存在某id的剧情
        /// </summary>
        /// <param name="chapterID"></param>
        /// <returns></returns>
        public bool HasChapter(string chapterID)
        {
            return chapterDictionary.ContainsKey(chapterID);
        }

        /// <summary>
        /// 检查chapterID是否唯一
        /// </summary>
        public void CheckIndependency()
        {
            Dictionary<string, Chapter> usedChapterID = new Dictionary<string, Chapter>();
            foreach (Chapter chapter in chapters)
            {
                if(!usedChapterID.ContainsKey(chapter.ChapterID))
                {
                    usedChapterID.Add(chapter.ChapterID, chapter);
                }
                else
                {
                    Debug.Log($"ChapterID\"{chapter.ChapterID}\" 发生碰撞。");
                }
            }
        }
    }
}