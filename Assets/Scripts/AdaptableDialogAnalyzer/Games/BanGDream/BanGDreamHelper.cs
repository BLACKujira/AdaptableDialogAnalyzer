using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using static AdaptableDialogAnalyzer.Games.BanGDream.GameDefine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public static class BanGDreamHelper
    {
        static Dictionary<string, int> characterNamaeMap = null;
        /// <summary>
        /// 记录每个角色的标准名称
        /// </summary>
        static Dictionary<string, int> CharacterNamaeMap
        {
            get
            {
                if (characterNamaeMap == null) characterNamaeMap = GetCharacterNamaeMap();
                return characterNamaeMap;
            }
        }
        static Dictionary<string, int> GetCharacterNamaeMap()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (var character in GlobalConfig.CharacterDefinition.Characters)
            {
                if (character.id == 0) continue;
                dictionary[character.Namae] = character.id;
            }
            return dictionary;
        }

        static Dictionary<int, BandIdName> characterBandMap = new Dictionary<int, BandIdName>()
        {
            { 1, BandIdName.PoppinParty },
            { 2, BandIdName.PoppinParty },
            { 3, BandIdName.PoppinParty },
            { 4, BandIdName.PoppinParty },
            { 5, BandIdName.PoppinParty },
            { 6, BandIdName.Afterglow },
            { 7, BandIdName.Afterglow },
            { 8, BandIdName.Afterglow },
            { 9, BandIdName.Afterglow },
            { 10, BandIdName.Afterglow },
            { 11, BandIdName.HelloHappyWorld },
            { 12, BandIdName.HelloHappyWorld },
            { 13, BandIdName.HelloHappyWorld },
            { 14, BandIdName.HelloHappyWorld },
            { 15, BandIdName.HelloHappyWorld },
            { 16, BandIdName.PastelPalettes },
            { 17, BandIdName.PastelPalettes },
            { 18, BandIdName.PastelPalettes },
            { 19, BandIdName.PastelPalettes },
            { 20, BandIdName.PastelPalettes },
            { 21, BandIdName.Roselia },
            { 22, BandIdName.Roselia },
            { 23, BandIdName.Roselia },
            { 24, BandIdName.Roselia },
            { 25, BandIdName.Roselia },
            { 26, BandIdName.Morfonica },
            { 27, BandIdName.Morfonica },
            { 28, BandIdName.Morfonica },
            { 29, BandIdName.Morfonica },
            { 30, BandIdName.Morfonica },
            { 31, BandIdName.RaiseASuilen },
            { 32, BandIdName.RaiseASuilen },
            { 33, BandIdName.RaiseASuilen },
            { 34, BandIdName.RaiseASuilen },
            { 35, BandIdName.RaiseASuilen },
        };

        public static int GetCharacterId_Scenario(ScenarioSnippetTalk scenarioSnippetTalk)
        {
            if (scenarioSnippetTalk.talkCharacters == null || scenarioSnippetTalk.talkCharacters.Length <= 0) return 0;
            int characterId = scenarioSnippetTalk.talkCharacters[0].characterId;
            if (characterId == 601) characterId = 15; //合并美咲和ミッシェル
            if (!GlobalConfig.CharacterDefinition.HasDefinition(characterId)) characterId = 0;

            //在显示名称严格等于角色名时也算作是对应角色的台词
            if (characterId == 0)
            {
                if (CharacterNamaeMap.ContainsKey(scenarioSnippetTalk.windowDisplayName))
                    characterId = characterNamaeMap[scenarioSnippetTalk.windowDisplayName];
            }

            return characterId;
        }

        public static int GetCharacterId_BackstageTalk(BackstageTalkSnippet backstageTalkSnippet)
        {
            if (backstageTalkSnippet.characterIds == null || backstageTalkSnippet.characterIds.Count <= 0) return 0;
            int characterId = (int)backstageTalkSnippet.characterIds[0];
            if (!GlobalConfig.CharacterDefinition.HasDefinition(characterId)) characterId = 0;
            return characterId;
        }

        public static EventStoryInfo GetEventStoryInfo(string chapterId)
        {
            string scenarioevent = "Scenarioevent";
            if (!chapterId.StartsWith(scenarioevent)) return null;

            string ids = chapterId.Substring(scenarioevent.Length);
            string[] idArray = ids.Split('-');

            if (idArray.Length != 2) return null;
            if (!int.TryParse(idArray[0], out int eventId)) return null;
            if (!int.TryParse(idArray[1], out int chapter)) return null;

            return new EventStoryInfo(eventId, chapter);
        }

        public static bool IsMainCharacter(int characterID)
        {
            if (characterID >= 1 && characterID <= 35) return true;
            return false;
        }

        public static BandIdName GetCharacterBand(int characterID)
        {
            if(characterBandMap.ContainsKey(characterID)) return characterBandMap[characterID];
            return BandIdName.None;
        }
    }

    public class EventStoryInfo
    {
        public int eventId;
        public int chapter;

        public EventStoryInfo(int eventId, int chapter)
        {
            this.eventId = eventId;
            this.chapter = chapter;
        }
    }
}