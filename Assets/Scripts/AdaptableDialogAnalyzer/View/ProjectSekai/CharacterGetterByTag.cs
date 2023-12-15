using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class CharacterGetterByPixivTag
    {
        Dictionary<string, int> tagCharacterIdPair;

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
    }
}
