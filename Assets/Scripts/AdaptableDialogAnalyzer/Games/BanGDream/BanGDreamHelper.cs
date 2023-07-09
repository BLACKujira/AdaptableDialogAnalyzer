using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public static class BanGDreamHelper
    {
        
        static Dictionary<string,int> characterNamaeMap = null;
        /// <summary>
        /// 记录每个角色的标准名称
        /// </summary>
        static Dictionary<string,int> CharacterNamaeMap
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

        public static int GetCharacterId_Scenario(ScenarioSnippetTalk scenarioSnippetTalk)
        {
            if (scenarioSnippetTalk.talkCharacters == null || scenarioSnippetTalk.talkCharacters.Length <= 0) return 0;
            int characterId = scenarioSnippetTalk.talkCharacters[0].characterId;
            if (!GlobalConfig.CharacterDefinition.HasDefinition(characterId)) characterId = 0;

            //在显示名称严格等于角色名时也算作是对应角色的台词
            if(characterId == 0)
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
            if(!int.TryParse(idArray[0],out int eventId)) return null;
            if(!int.TryParse(idArray[1],out int chapter)) return null;

            return new EventStoryInfo(eventId, chapter);
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