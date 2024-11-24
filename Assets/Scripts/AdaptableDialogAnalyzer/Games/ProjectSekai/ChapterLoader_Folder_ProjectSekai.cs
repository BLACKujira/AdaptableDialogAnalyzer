using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    public class ChapterLoader_Folder_ProjectSekai : ChapterLoader
    {
        public string assetBundleFolder;
        public ProjectSekai_MasterLoader masterLoader;
        [Header("Settings")]
        public bool loadUnitStory = true;
        public bool loadEventStory = true;
        public bool loadCardStory = true;
        public bool loadMapTalk = true;
        public bool loadLiveTalk = true;
        public bool loadOtherStory = true;

        #region 获取剧情文件
        // 是否是.json（从SV上下载或使用AS提取）或.asset（从SV上下载）文件
        bool IsSampleFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension.Equals(".json") || extension.Equals(".asset");
        }

        public string[] GetUnitStoryFiles_Server(string folder_Sample)
        {
            string folderBase = $"{folder_Sample}/scenario/unitstory";
            string[] folders = Directory.GetDirectories(folderBase);
            string[] files = folders.SelectMany(folder => Directory.GetFiles(folder)).Where(IsSampleFile).ToArray();
            return files;
        }

        public string[] GetEventStoryFiles_Server(string folder_Sample)
        {
            string folderBase = $"{folder_Sample}/event_story";
            string[] folders = Directory.GetDirectories(folderBase);
            string[] files = folders.SelectMany(folder =>
            {
                string[] folderSubs = new string[]
                {
                    Path.Combine(folder, "scenario"),
                    Path.Combine(folder, "scenario_rip")
                };
                foreach (var folderSub in folderSubs)
                {
                    if (Directory.Exists(folderSub))
                        return Directory.GetFiles(folderSub);
                }
                return new string[0];
            }).Where(IsSampleFile).ToArray();
            return files;
        }

        public string[] GetCardStoryFiles_Server(string folder_Sample)
        {
            string folderBase = $"{folder_Sample}/character/member";
            string[] folders = Directory.GetDirectories(folderBase);
            string[] files = folders.SelectMany(folder => Directory.GetFiles(folder)).Where(IsSampleFile).ToArray();
            return files;
        }

        public string[] GetMapTalkFiles_Server(string folder_Sample)
        {
            string folderBase = $"{folder_Sample}/scenario/actionset";
            string[] folders = Directory.GetDirectories(folderBase);
            string[] files = folders.SelectMany(folder => Directory.GetFiles(folder)).Where(IsSampleFile).ToArray();
            return files;
        }

        public string[] GetLiveTalkFiles_Server(string folder_Sample)
        {
            string folderBase = $"{folder_Sample}/virtual_live/mc/scenario";
            string[] folders = Directory.GetDirectories(folderBase);
            string[] files = folders.SelectMany(folder => Directory.GetFiles(folder)).Where(IsSampleFile).ToArray();
            return files;
        }

        public string[] GetOtherStoryFiles_Server(string folder_Sample)
        {
            List<string> files = new List<string>();
            for (int i = 1; i < 27; i++)
            {
                files.Add($"{folder_Sample}/scenario/profile_rip/self_{(Character)i}.json");
            }

            string folderBase = $"{folder_Sample}/scenario/special";
            string[] folders = Directory.GetDirectories(folderBase);
            IEnumerable<string> spStories = folders.SelectMany(folder => Directory.GetFiles(folder)).Where(IsSampleFile);

            files.AddRange(spStories);
            return files.ToArray();
        }

        #endregion

        public List<Chapter> LoadChapter_Scenario(string[] files, string type)
        {
            List<Chapter> chapters = new List<Chapter>();
            foreach (string file in files)
            {
                Chapter chapter = Chapter_ProjectSekai_Scenario.LoadText(File.ReadAllText(file));
                chapter.ChapterID = Path.GetFileNameWithoutExtension(file);
                chapter.ChapterType = type;
                chapters.Add(chapter);
            }
            return chapters;
        }

        public List<Chapter> LoadChapter_Ceremony(string[] files, string type)
        {
            List<Chapter> chapters = new List<Chapter>();
            foreach (string file in files)
            {
                Chapter chapter = Chapter_ProjectSekai_Ceremony.LoadText(File.ReadAllText(file), masterLoader.MasterCharacter3Ds);
                chapter.ChapterID = Path.GetFileNameWithoutExtension(file);
                chapter.ChapterType = type;
                chapters.Add(chapter);
            }
            return chapters;
        }

        public override Chapter[] InitializeChapters()
        {
            List<Chapter> chapters = new List<Chapter>();
            if (loadUnitStory) chapters.AddRange(LoadChapter_Scenario(GetUnitStoryFiles_Server(assetBundleFolder), "乐队剧情"));
            if (loadEventStory) chapters.AddRange(LoadChapter_Scenario(GetEventStoryFiles_Server(assetBundleFolder), "活动剧情"));
            if (loadCardStory) chapters.AddRange(LoadChapter_Scenario(GetCardStoryFiles_Server(assetBundleFolder), "卡片剧情"));
            if (loadMapTalk) chapters.AddRange(LoadChapter_Scenario(GetMapTalkFiles_Server(assetBundleFolder), "地图对话"));
            if (loadLiveTalk) chapters.AddRange(LoadChapter_Ceremony(GetLiveTalkFiles_Server(assetBundleFolder), "LIVE对话"));
            if (loadOtherStory) chapters.AddRange(LoadChapter_Scenario(GetOtherStoryFiles_Server(assetBundleFolder), "其他剧情"));
            return chapters.ToArray();
        }
    }
}
