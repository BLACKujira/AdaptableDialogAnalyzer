using AdaptableDialogAnalyzer.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class ChapterLoader_Folder_BanGDream_Scenario : ChapterLoader
    {
        public string assetBundleFolder;
        public BanGDream_MasterLoader masterLoader;
        [Header("Settings")]
        public bool loadMainStory = true;
        public bool loadBandStory = true;
        public bool loadEventStory = true;
        public bool loadAreaTalk = true;
        public bool loadCardStory = true;
        public bool loadAfterLive = true;
        public bool loadBirthdayStory = true;
        public bool loadBackstageTalk = true;
        public bool loadOtherStory = true;
        [Header("Settings2")]
        public bool setTitle = false;
        public bool setTimeAndRemoveUnused = true;

        const string SCENARIO_EXTENSION = ".json";

        public const string TYPE_MAINSTORY = "主线剧情";
        public const string TYPE_BANDSTORY = "乐队剧情";
        public const string TYPE_EVENTSTORY = "活动剧情";
        public const string TYPE_AREATALK = "区域对话";
        public const string TYPE_CARDSTORY = "卡面剧情";
        public const string TYPE_AFTERLIVE = "LIVE结束语言";
        public const string TYPE_BIRTHDAYSTORY = "生日剧情";
        public const string TYPE_BACKSTAGETALK = "后台对话";
        public const string TYPE_OTHERSTORY = "其他剧情";

        const string DIR_EVENTSTORY = @"assets\star\forassetbundle\asneeded\scenario\eventstory";
        const string DIR_BIRTHDAYSTORY = @"assets\star\forassetbundle\asneeded\scenario\birthdaystory";
        const string DIR_AREATALK = @"assets\star\forassetbundle\startapp\scenario\actionset";
        const string DIR_AFTERLIVE = @"assets\star\forassetbundle\startapp\scenario\afterlive";
        const string DIR_BANDSTORY = @"assets\star\forassetbundle\startapp\scenario\band";
        const string DIR_MAINSTORY = @"assets\star\forassetbundle\startapp\scenario\main";
        const string DIR_CARDSTORY = @"assets\star\forassetbundle\asneeded\characters\resourceset";
        const string DIR_BACKSTAGETALK = @"assets\star\forassetbundle\asneeded\backstage\talkset";

        const string DIR_LOGINSTORY = @"assets\star\forassetbundle\asneeded\scenario\loginstory";
        const string DIR_AREAOPENINGSTORY = @"assets\star\forassetbundle\asneeded\scenario\areaopeningstory";
        const string DIR_BACKSTAGESTORY = @"assets\star\forassetbundle\asneeded\scenario\backstagestory";
        const string DIR_DIGESTSTORY = @"assets\star\forassetbundle\asneeded\scenario\digeststory";
        const string DIR_PRECEDINGSTORY = @"assets\star\forassetbundle\asneeded\scenario\precedingstory";

        const string DIR_ACTIONSET = @"assets\star\forassetbundle\startapp\actionset";

        Regex regex_BackstageTalk = new Regex("BackstageTalkSet\\d+\\.json");

        SuiteMasterGetResponse suiteMasterGetResponse;
        UserEventStoryMemorialResponse userEventStoryMemorialResponse;

        public override Chapter[] InitializeChapters()
        {
            List<Chapter> chapters = new List<Chapter>();

            if (loadMainStory) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_MAINSTORY), TYPE_MAINSTORY));
            if (loadBandStory) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_BANDSTORY), TYPE_BANDSTORY));
            if (loadEventStory) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_EVENTSTORY), TYPE_EVENTSTORY));
            if (loadBirthdayStory) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_BIRTHDAYSTORY), TYPE_BIRTHDAYSTORY));
            if (loadAreaTalk) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_AREATALK), TYPE_AREATALK));
            if (loadAfterLive) chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_AFTERLIVE), TYPE_AFTERLIVE));

            //由于自我介绍剧情也在这个文件夹内，即便只选择读取卡片剧情也会出现其他剧情
            if (loadCardStory) chapters.AddRange(LoadScenarioFromSubFolders(
                Chapter_BanGDream_Scenario.LoadText,
                Path.Combine(assetBundleFolder, DIR_CARDSTORY),
                (path) =>
                {
                    string fileName = Path.GetFileName(path);
                    if (fileName.StartsWith("Scenarioepisode") || fileName.StartsWith("Scenariomemorial")) return TYPE_CARDSTORY;
                    else return TYPE_OTHERSTORY;
                },
                (_) => true));

            if (loadBackstageTalk) chapters.AddRange(LoadScenarioFromSubFolders(
                Chapter_BanGDream_BackstageTalk.LoadText,
                Path.Combine(assetBundleFolder, DIR_BACKSTAGETALK), TYPE_BACKSTAGETALK,
                (path) =>
                {
                    string fileName = Path.GetFileName(path);
                    if (regex_BackstageTalk.IsMatch(fileName)) return true;
                    return false;
                }));

            if (loadOtherStory)
            {
                chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_LOGINSTORY), TYPE_OTHERSTORY));
                chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_AREAOPENINGSTORY), TYPE_OTHERSTORY));
                chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_BACKSTAGESTORY), TYPE_OTHERSTORY));
                chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_DIGESTSTORY), TYPE_OTHERSTORY));
                chapters.AddRange(LoadScenarioFromSubFolders(Path.Combine(assetBundleFolder, DIR_PRECEDINGSTORY), TYPE_OTHERSTORY));
            }

            //foreach (var chapter in chapters)
            //{
            //    if (chapter.ChapterTime <= BanGDreamHelper.SEASON_1_STARTTIME) Debug.Log($"剧情时间未找到 {chapter.ChapterID}");
            //}
            PostProcess(chapters);
            return chapters.ToArray();
        }

        public void PostProcess(List<Chapter> chapters)
        {
            if(setTitle) suiteMasterGetResponse = masterLoader.SuiteMasterGetResponse;
            if (setTitle)
            {
                ChapterTitleGetter chapterTitleGetter = new ChapterTitleGetter(suiteMasterGetResponse);
                foreach (var chapter in chapters)
                {
                    chapter.ChapterTitle = chapterTitleGetter.GetChapterTitle(chapter.ChapterType,chapter.ChapterID);
                }
            }
        }

        #region 获取剧情
        /// <summary>
        /// 获取一个文件夹中的所有剧情，并标记为type,func:路径，返回类型 
        /// </summary>
        public List<Chapter> LoadScenarioFromFolder(Func<string, Chapter> getChapter, string folder, Func<string, string> getType, Func<string, bool> acceptChapter)
        {
            List<Chapter> chapters = new List<Chapter>();
            string[] files = Directory.GetFiles(folder);

            foreach (string file in files)
            {
                if (!Path.GetExtension(file).ToLower().Equals(SCENARIO_EXTENSION)) continue;
                if (!acceptChapter(file)) continue;

                string raw = File.ReadAllText(file);
                Chapter chapter = getChapter(raw);
                chapter.ChapterID = Path.GetFileNameWithoutExtension(file);
                chapter.ChapterType = getType(file);

                //设置世界并再次判断是否排除此文件
                if (setTimeAndRemoveUnused && !SetTimeAndAccept(chapter)) continue;

                chapters.Add(chapter);
            }

            return chapters;
        }

        /// <summary>
        /// 以递归形式获取所有子文件夹中的所有剧情 
        /// </summary>
        public List<Chapter> LoadScenarioFromSubFolders(Func<string, Chapter> getChapter, string folder, Func<string, string> getType, Func<string, bool> acceptChapter)
        {
            List<Chapter> chapters = new List<Chapter>();
            chapters.AddRange(LoadScenarioFromFolder(getChapter, folder, getType, acceptChapter));

            string[] folders = Directory.GetDirectories(folder);
            foreach (var dir in folders)
            {
                chapters.AddRange(LoadScenarioFromSubFolders(getChapter, dir, getType, acceptChapter));
            }

            return chapters;
        }

        /// <summary>
        /// 以递归形式获取所有子文件夹中的所有剧情 
        /// </summary>
        public List<Chapter> LoadScenarioFromSubFolders(Func<string, Chapter> getChapter, string folder, string type, Func<string, bool> acceptChapter)
        {
            return LoadScenarioFromSubFolders(getChapter, folder, (_) => type, acceptChapter);
        }

        /// <summary>
        /// 以递归形式获取所有子文件夹中的所有剧情 
        /// </summary>
        public List<Chapter> LoadScenarioFromSubFolders(string folder, string type)
        {
            return LoadScenarioFromSubFolders(Chapter_BanGDream_Scenario.LoadText, folder, (_) => type, (_) => true);
        }
        #endregion

        #region 获取时间（弃用）
        List<ActionSetData> actionSetDataList = null;
        public List<ActionSetData> GetActionSetDataList()
        {
            return Directory.GetDirectories(Path.Combine(assetBundleFolder, DIR_ACTIONSET))
                .SelectMany(dir => Directory.GetFiles(dir))
                .Where(file => Path.GetFileName(file).StartsWith("ActionSet"))
                .Select(file => JsonUtility.FromJson<ActionSetData>(File.ReadAllText(file)))
                .ToList();
        }

        Dictionary<uint, MasterEvent> allEventMap = null;
        public void InitAllEventMap()
        {
            allEventMap = new Dictionary<uint, MasterEvent>();
            foreach (var keyValuePair in userEventStoryMemorialResponse.PastEventMap.Entries)
            {
                allEventMap[keyValuePair.Key] = keyValuePair.Value;
            }
            foreach (var keyValuePair in suiteMasterGetResponse.MasterEventMap.Entries)
            {
                allEventMap[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        /// <summary>
        /// 设置剧情的时间，并选择是否丢弃剧情
        /// </summary>
        public bool SetTimeAndAccept(Chapter chapter)
        {
            if (allEventMap == null) InitAllEventMap();
            switch (chapter.ChapterType)
            {
                case TYPE_MAINSTORY: return SetTimeAndAccept_MainStory(chapter);
                case TYPE_BIRTHDAYSTORY: return SetTimeAndAccept_BirthdayStory(chapter);
                case TYPE_AREATALK: return SetTimeAndAccept_AreaTalk(chapter);
                case TYPE_BACKSTAGETALK: return SetTimeAndAccept_BackstageTalk(chapter);
                case TYPE_BANDSTORY: return SetTimeAndAccept_BandStory(chapter);
                case TYPE_EVENTSTORY: return SetTimeAndAccept_EventStory(chapter);
                case TYPE_CARDSTORY: return SetTimeAndAccept_CardStory(chapter);
                default: return true;
            }
        }

        Dictionary<string, MasterMainStory> masterMainStoryMap = null;
        public bool SetTimeAndAccept_MainStory(Chapter chapter)
        {
            if (masterMainStoryMap == null)
            {
                //初始化字典
                masterMainStoryMap = new Dictionary<string, MasterMainStory>();
                foreach (var keyValuePair in suiteMasterGetResponse.MasterMainStoryMap.Entries)
                {
                    masterMainStoryMap["Scenario" + keyValuePair.Value.ScenarioId] = keyValuePair.Value;
                }
            }

            if (masterMainStoryMap.ContainsKey(chapter.ChapterID))
            {
                chapter.ChapterTime = (long)masterMainStoryMap[chapter.ChapterID].PublishedAt;
                return true;
            }

            return false;
        }

        Dictionary<string, MasterBirthdayStory> masterBirthdayStoryMap = null;
        public bool SetTimeAndAccept_BirthdayStory(Chapter chapter)
        {
            if (masterBirthdayStoryMap == null)
            {
                //初始化字典
                masterBirthdayStoryMap = new Dictionary<string, MasterBirthdayStory>();
                foreach (var keyValuePair in suiteMasterGetResponse.MasterBirthdayStoryMap.Entries)
                {
                    masterBirthdayStoryMap["Scenario" + keyValuePair.Value.ScenarioId] = keyValuePair.Value;
                }
            }

            if (masterBirthdayStoryMap.ContainsKey(chapter.ChapterID))
            {
                chapter.ChapterTime = (long)masterBirthdayStoryMap[chapter.ChapterID].StartAt;
                return true;
            }

            return false;
        }

        Dictionary<string, MasterActionSet> masterActionSetMap = null;
        public bool SetTimeAndAccept_AreaTalk(Chapter chapter)
        {
            if (masterActionSetMap == null)
            {
                if (actionSetDataList == null) actionSetDataList = GetActionSetDataList();

                IEnumerable<(string, MasterActionSet)> rawMap = actionSetDataList
                    .Where(d => d.details.Count > 0)
                    .Select(d => ("Scenario" + d.details.FirstOrDefault().reactionTypeBelongId, d.actionSetId))
                    .Where(t => suiteMasterGetResponse.MasterActionSetMap.Entries.ContainsKey(t.actionSetId))
                    .Select(t => (t.Item1, suiteMasterGetResponse.MasterActionSetMap.Entries[t.actionSetId]));

                masterActionSetMap = new Dictionary<string, MasterActionSet>();
                foreach ((string fileName, MasterActionSet actionSet) in rawMap)
                {
                    masterActionSetMap[fileName] = actionSet;
                }
            }

            if (masterActionSetMap.ContainsKey(chapter.ChapterID))
            {
                MasterActionSet masterActionSet = masterActionSetMap[chapter.ChapterID];

                //通过活动时间推测区域对话发生时间
                PastEventMap pastEventMap = userEventStoryMemorialResponse.PastEventMap;
                if (allEventMap.ContainsKey(masterActionSet.EventId))
                {
                    MasterEvent masterEvent = allEventMap[masterActionSet.EventId];
                    chapter.ChapterTime = (long)masterEvent.StartAt;
                    return true;
                }

                //通过特殊时段推测剧情发生时间
                //特殊区域的对话似乎一定发生在特殊时段，故不做特殊处理
                MasterSeasonSpecialMap masterSeasonSpecialMap = suiteMasterGetResponse.MasterSeasonSpecialMap;
                if (masterSeasonSpecialMap.Entries.ContainsKey(masterActionSet.SeasonSpecialId))
                {
                    MasterSeasonSpecial masterSeasonSpecial = masterSeasonSpecialMap.Entries[masterActionSet.SeasonSpecialId];
                    chapter.ChapterTime = (long)masterSeasonSpecial.StartAt;
                    return true;
                }

                //剩余的确定到剧情的季度
                chapter.ChapterTime = BanGDreamHelper.GetSeasonStartTime(masterActionSet.StartSeason);
                return true;
            }

            return false;
        }

        Dictionary<string, MasterBackstageTalkSet> masterBackstageTalkSetMap = null;
        public bool SetTimeAndAccept_BackstageTalk(Chapter chapter)
        {
            if (masterBackstageTalkSetMap == null)
            {
                //初始化字典
                masterBackstageTalkSetMap = new Dictionary<string, MasterBackstageTalkSet>();
                foreach (var keyValuePair in suiteMasterGetResponse.MasterBackstageTalkSetMap.Entries)
                {
                    masterBackstageTalkSetMap["BackstageTalkSet" + keyValuePair.Value.BackstageTalkSetId] = keyValuePair.Value;
                }
            }

            if (masterBackstageTalkSetMap.ContainsKey(chapter.ChapterID))
            {
                MasterBackstageTalkSet masterBackstageTalkSet = masterBackstageTalkSetMap[chapter.ChapterID];

                //通过特殊时段推测剧情发生时间
                MasterSeasonSpecialMap masterSeasonSpecialMap = suiteMasterGetResponse.MasterSeasonSpecialMap;
                if (masterSeasonSpecialMap.Entries.ContainsKey(masterBackstageTalkSet.SeasonSpecialId))
                {
                    MasterSeasonSpecial masterSeasonSpecial = masterSeasonSpecialMap.Entries[masterBackstageTalkSet.SeasonSpecialId];
                    chapter.ChapterTime = (long)masterSeasonSpecial.StartAt;
                    return true;
                }

                //通过季节推断时间
                MasterSeasonBasicMap masterSeasonBasicMap = suiteMasterGetResponse.MasterSeasonBasicMap;
                if (masterSeasonBasicMap.Entries.ContainsKey(masterBackstageTalkSet.SeasonBasicId))
                {
                    MasterSeasonBasic masterSeasonBasic = masterSeasonBasicMap.Entries[masterBackstageTalkSet.SeasonBasicId];
                    chapter.ChapterTime = (long)masterSeasonBasic.StartAt;
                    return true;
                }

                chapter.ChapterTime = BanGDreamHelper.GetSeasonStartTime(masterBackstageTalkSet.StartSeason);
                return true;
            }

            return false;
        }

        Dictionary<string, MasterBandStory> masterBandStoryMap = null;
        public bool SetTimeAndAccept_BandStory(Chapter chapter)
        {
            if (masterBandStoryMap == null)
            {
                //初始化字典
                MasterBandStoryMap[] masterBandStoryMaps = new MasterBandStoryMap[]
                {
                    suiteMasterGetResponse.MasterPoppinPartyStoryMap,
                    suiteMasterGetResponse.MasterAfterglowStoryMap,
                    suiteMasterGetResponse.MasterHelloHappyWorldStoryMap,
                    suiteMasterGetResponse.MasterPastelPalettesStoryMap,
                    suiteMasterGetResponse.MasterRoseliaStoryMap,
                    suiteMasterGetResponse.MasterMorfonicaStoryMap,
                    suiteMasterGetResponse.MasterRaiseASuilenStoryMap
                };

                IEnumerable<KeyValuePair<uint, MasterBandStory>> entries = masterBandStoryMaps.SelectMany(m => m.Entries);

                masterBandStoryMap = new Dictionary<string, MasterBandStory>();
                foreach (var keyValuePair in entries)
                {
                    masterBandStoryMap["Scenario" + keyValuePair.Value.ScenarioId] = keyValuePair.Value;
                }
            }

            if (masterBandStoryMap.ContainsKey(chapter.ChapterID))
            {
                chapter.ChapterTime = (long)masterBandStoryMap[chapter.ChapterID].PublishedAt;
                return true;
            }

            return false;
        }

        Dictionary<string, MasterEvent> masterEventMap = null;
        Regex regex_GetEventStoryEvent = new Regex("Scenarioevent\\d+(?=-\\d{2})");
        public bool SetTimeAndAccept_EventStory(Chapter chapter)
        {
            if (masterEventMap == null)
            {
                //初始化字典
                masterEventMap = new Dictionary<string, MasterEvent>();
                foreach (var keyValuePair in allEventMap)
                {
                    masterEventMap[$"Scenarioevent{keyValuePair.Value.EventId:00}"] = keyValuePair.Value;
                }
            }

            Match match = regex_GetEventStoryEvent.Match(chapter.ChapterID);
            if (match.Success)
            {
                string key = match.Value;
                if (masterEventMap.ContainsKey(key))
                {
                    chapter.ChapterTime = (long)masterEventMap[key].StartAt;
                    return true;
                }
            }

            return false;
        }

        Dictionary<string, MasterCharacterSituation> masterCharacterSituationMap = null;
        Dictionary<uint, MasterEvent> masterSituationEventMap = null;
        public bool SetTimeAndAccept_CardStory(Chapter chapter)
        {
            if (masterCharacterSituationMap == null)
            {
                //初始化字典
                masterCharacterSituationMap = new Dictionary<string, MasterCharacterSituation>();
                foreach (var keyValuePair in suiteMasterGetResponse.MasterCharacterSituationMap.Entries)
                {
                    if (keyValuePair.Value.Episodes == null) continue;
                    foreach (var masterEpisode in keyValuePair.Value.Episodes.Entries)
                    {
                        masterCharacterSituationMap[$"Scenario{masterEpisode.ScenarioId}"] = keyValuePair.Value;
                    }
                }

                masterSituationEventMap = new Dictionary<uint, MasterEvent>();
                foreach (var keyValuePair in suiteMasterGetResponse.MasterEventSituationMap.Entries)
                {
                    if (allEventMap.ContainsKey(keyValuePair.Key))
                    {
                        foreach (var masterEventSituation in keyValuePair.Value.Entries)
                        {
                            masterSituationEventMap[masterEventSituation.SituationId] = allEventMap[keyValuePair.Key];
                        }
                    }
                }
            }

            if (masterCharacterSituationMap.ContainsKey(chapter.ChapterID))
            {
                MasterCharacterSituation masterCharacterSituation = masterCharacterSituationMap[chapter.ChapterID];

                //先通过卡片所属的活动判断发生时间
                if (masterSituationEventMap.ContainsKey(masterCharacterSituation.SituationId))
                {
                    chapter.ChapterTime = (long)masterSituationEventMap[masterCharacterSituation.SituationId].StartAt;
                    return true;
                }

                chapter.ChapterTime = (long)masterCharacterSituation.ReleasedAt;
                return true;
            }

            return false;
        }
        #endregion
    }
}