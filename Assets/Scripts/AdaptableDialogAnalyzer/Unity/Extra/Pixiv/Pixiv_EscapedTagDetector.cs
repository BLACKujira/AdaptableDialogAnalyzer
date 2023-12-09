using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class Pixiv_EscapedTagDetector : TaskWindow
    {
        [Header("Settings")]
        public string tagFolder;

        int checkedCount = 0;

        private void Start()
        {
            CheckTagFolder(tagFolder);
            Priority = 1;
            Progress = $"完成，共检查了{checkedCount}个作品";
        }

        void CheckTagFolder(string folderPath)
        {
            string[] tags = Path.GetFileName(folderPath).Split(' ');
            string illustFolder = Path.Combine(folderPath, "illust");
            string mangaFolder = Path.Combine(folderPath, "manga");
            string novelFolder = Path.Combine(folderPath, "novel");

            foreach (var file in Directory.GetFiles(illustFolder))
            {
                CheckIllustrations(file, tags);
            }
            foreach (var file in Directory.GetFiles(mangaFolder))
            {
                CheckManga(file, tags);
            }
            foreach (var file in Directory.GetFiles(novelFolder))
            {
                CheckNovels(file, tags);
            }
        }

        void CheckIllustrations(string filePath, string[] tags)
        {
            string json = File.ReadAllText(filePath);
            IllustrationsRoot illustrationsRoot = JsonUtility.FromJson<IllustrationsRoot>(json);
            foreach (var dataItem in illustrationsRoot.body.illust.data)
            {
                if(!CheckTags(dataItem.tags, tags))
                {
                    Debug.Log($"插画{dataItem.id} 可能含有转义标签，作品标签[{string.Join(",", dataItem.tags)}]，目标标签[{string.Join(",", tags)}]");
                }
            }
        }

        void CheckManga(string filePath, string[] tags)
        {
            string json = File.ReadAllText(filePath);
            MangaRoot mangaRoot = JsonUtility.FromJson<MangaRoot>(json);
            foreach (var dataItem in mangaRoot.body.manga.data)
            {
                if (!CheckTags(dataItem.tags, tags))
                {
                    Debug.Log($"漫画{dataItem.id} 可能含有转义标签，作品标签[{string.Join(",", dataItem.tags)}]，目标标签[{string.Join(",", tags)}]");
                }
            }
        }

        void CheckNovels(string filePath, string[] tags)
        {
            string json = File.ReadAllText(filePath);
            NovelsRoot novelsRoot = JsonUtility.FromJson<NovelsRoot>(json);
            foreach (var dataItem in novelsRoot.body.novel.data)
            {
                if (!CheckTags(dataItem.tags, tags))
                {
                    Debug.Log($"小说{dataItem.id} 可能含有转义标签，作品标签[{string.Join(",", dataItem.tags)}]，目标标签[{string.Join(",", tags)}]");
                }
            }
        }

        /// <summary>
        /// 检查workTags是否包含所有searchTags
        /// </summary>
        bool CheckTags(List<string> workTags, string[] searchTags)
        {
            checkedCount++;
            foreach(string searchTag in searchTags)
            {
                if(!workTags.Contains(searchTag))
                {
                    return false;
                }
            }
            return true;
        }
    }
}