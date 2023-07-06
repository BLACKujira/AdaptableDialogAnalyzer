using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class ChapterLoader_Folder_BanGDream_Scenario : ChapterLoader
    {
        public string apkAssetFolder;
        public string assetBundleFolder;
        [Header("Settings")]
        public bool loadMainStory = true;
        public bool loadBandStory = true;
        public bool loadEventStory = true;
        public bool loadAreaTalk = true;
        public bool loadCardStory = true;
        public bool loadAfterLive = true;
        public bool loadBirthdayStory = true;
        public bool loadOtherStory = true;

        const string SCENARIO_EXTENSION = ".json";

        const string TYPE_MAINSTORY = "主线剧情";
        const string TYPE_BANDSTORY = "乐队剧情";
        const string TYPE_EVENTSTORY = "活动剧情";
        const string TYPE_AREATALK = "区域对话";
        const string TYPE_CARDSTORY = "卡面剧情";
        const string TYPE_AFTERLIVE = "LIVE结束语言";
        const string TYPE_BIRTHDAYSTORY = "生日剧情";
        const string TYPE_OTHERSTORY = "其他剧情";

        const string DIR_EVENTSTORY = @"assets\star\forassetbundle\asneeded\scenario\eventstory";
        const string DIR_BIRTHDAYSTORY = @"assets\star\forassetbundle\asneeded\scenario\birthdaystory";
        const string DIR_AREATALK = @"assets\star\forassetbundle\startapp\scenario\actionset";
        const string DIR_AFTERLIVE = @"assets\star\forassetbundle\startapp\scenario\afterlive";
        const string DIR_BANDSTORY = @"assets\star\forassetbundle\startapp\scenario\band";
        const string DIR_MAINSTORY = @"assets\star\forassetbundle\startapp\scenario\main";
        const string DIR_CARDSTORY = @"assets\star\forassetbundle\asneeded\characters\resourceset";

        const string DIR_LOGINSTORY = @"assets\star\forassetbundle\asneeded\scenario\loginstory";
        const string DIR_AREAOPENINGSTORY = @"assets\star\forassetbundle\asneeded\scenario\areaopeningstory";
        const string DIR_BACKSTAGESTORY = @"assets\star\forassetbundle\asneeded\scenario\backstagestory";
        const string DIR_DIGESTSTORY = @"assets\star\forassetbundle\asneeded\scenario\digeststory";
        const string DIR_PRECEDINGSTORY = @"assets\star\forassetbundle\asneeded\scenario\precedingstory";

        public override Chapter[] InitializeChapters()
        {
            List<Chapter> chapters = new List<Chapter>();

            if(loadMainStory) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_MAINSTORY), TYPE_MAINSTORY));
            if (loadBandStory) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_BANDSTORY), TYPE_BANDSTORY));
            if (loadEventStory) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_EVENTSTORY), TYPE_EVENTSTORY));
            if (loadBirthdayStory) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_BIRTHDAYSTORY), TYPE_BIRTHDAYSTORY));
            if (loadAreaTalk) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_AREATALK), TYPE_AREATALK));
            if (loadAfterLive) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_AFTERLIVE), TYPE_AFTERLIVE));
            if (loadCardStory) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_CARDSTORY), TYPE_CARDSTORY));

            if (loadOtherStory)
            {
                chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_LOGINSTORY), TYPE_OTHERSTORY));
                chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_AREAOPENINGSTORY), TYPE_OTHERSTORY));
                chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_BACKSTAGESTORY), TYPE_OTHERSTORY));
                chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_DIGESTSTORY), TYPE_OTHERSTORY));
                chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_PRECEDINGSTORY), TYPE_OTHERSTORY));
            }

            return chapters.ToArray();
        }

        /// <summary>
        /// 获取一个文件夹中的所有剧情，并标记为type 
        /// </summary>
        public List<Chapter> LoadScenarioFromFolder(string folder, string type)
        {
            List<Chapter> chapters = new List<Chapter>();
            string[] files = Directory.GetFiles(folder);

            foreach (string file in files)
            {
                if (!Path.GetExtension(file).ToLower().Equals(SCENARIO_EXTENSION)) continue;

                string raw = File.ReadAllText(file);
                Chapter chapter = Chapter_BanGDream_Scenario.LoadText(raw);
                chapter.ChapterID = Path.GetFileNameWithoutExtension(file);
                chapter.ChapterType = type;
                chapters.Add(chapter);
            }

            return chapters;
        }

        /// <summary>
        /// 以递归形式获取所有子文件夹中的所有剧情 
        /// </summary>
        public List<Chapter> LoadScenarioFromSubFolders(string folder, string type)
        {
            List<Chapter> chapters = new List<Chapter>();
            chapters.AddRange(LoadScenarioFromFolder(folder, type));

            string[] folders = Directory.GetDirectories(folder);
            foreach (var dir in folders)
            {
                chapters.AddRange(LoadScenarioFromSubFolders(dir, type));
            }

            return chapters;
        }
    }
}