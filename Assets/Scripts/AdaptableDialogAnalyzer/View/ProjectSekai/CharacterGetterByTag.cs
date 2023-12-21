using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class CharacterGetterByPixivTag
    {
        Dictionary<string, int> tagCharacterIdPair;
        Dictionary<string, int> tagUnitNamePairs = new Dictionary<string, int>()
        {
            { "Leo/need", 2 },
            { "レオニード", 2 },
            { "レオニ", 2 },

            { "モモジャン", 3 },
            { "MOREMOREJUMP!", 3 },

            { "VividBADSQUAD", 4 },
            { "ビビバス", 4 },

            { "ワンダショ", 5 },
            { "ワンダーランズ×ショウタイム", 5 },
            { "Wonderlands x Showtime", 5 },

            { "25時、ナイトコードで。", 6 },
            { "ニーゴ", 6 }
        };

        public CharacterGetterByPixivTag()
        {
            tagCharacterIdPair = GlobalConfig.CharacterDefinition.Characters
                .Where(c => c.id >= 1 && c.id <= 20)
                .ToDictionary(c => c.name.Replace(" ", string.Empty), c => c.id);
        }
        /// <summary>
        /// 统计函数用，根据标签获取角色ID
        /// </summary>
        public int[] GetCharacterByTag(List<string> tags)
        {
            List<int> characterIds = new List<int>();
            foreach (var tag in tags)
            {
                if (tagCharacterIdPair.ContainsKey(tag))
                {
                    characterIds.Add(tagCharacterIdPair[tag]);
                }
            }
            return characterIds.ToArray();
        }

        /// <summary>
        /// 统计函数用，根据标签获取组合ID
        /// </summary>
        public int[] GetUnitByTag(List<string> tags)
        {
            HashSet<int> result = new HashSet<int>();

            // 包含组合内角色
            int[] characterIds = GetCharacterByTag(tags);
            foreach (var characterId in characterIds)
            {
                int unitId = ProjectSekaiHelper.UnitToId(ProjectSekaiHelper.characters[characterId].unit);
                if (unitId > 0)
                {
                    result.Add(unitId);
                }
            }

            // 包含组合名
            foreach (var tag in tags)
            {
                if(tagUnitNamePairs.ContainsKey(tag))
                {
                    int unitId = tagUnitNamePairs[tag];
                    result.Add(unitId);
                }
            }

            return result.ToArray();
        }
    }
}
