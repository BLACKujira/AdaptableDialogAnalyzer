using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_MobPostRank : MonoBehaviour
    {
        [Header("Components")]
        public List<View_ProjectSekai_MobPostRank_Item> items;
        [Header("Settings")]
        public CharacterDefinition mobDefinition;
        public NicknameMapping mobNickname;
        [Header("Adapter")]
        public Pixiv_SearchResponseLoader searchResponseLoader;

        Dictionary<int, int> countDictionary = new Dictionary<int, int>();
        Dictionary<string, int> nicknameDictionary;

        List<List<string>> GetTagLists(MergedResponse mergedResponse)
        {
            List<List<string>> result = new List<List<string>>();
            result.AddRange(mergedResponse.artworks.Select(a => a.tags));
            result.AddRange(mergedResponse.novels.Select(n => n.tags));
            return result;
        }

        private void Start()
        {
            nicknameDictionary = mobNickname.GetCacheDictionary();

            MergedResponse mergedResponse = searchResponseLoader.MergedResponse;
            var tagLists = GetTagLists(mergedResponse);
            foreach (var tagList in tagLists)
            {
                HashSet<int> mobIds = GetMobIdsByTags(tagList);
                foreach (var mobId in mobIds)
                {
                    if (!countDictionary.ContainsKey(mobId)) countDictionary.Add(mobId, 0);
                    countDictionary[mobId]++;
                }
            }

            List<KeyValuePair<int, int>> countList = countDictionary
                .OrderByDescending(kvp => kvp.Value)
                .ToList();

            for (int i = 0; i < countList.Count && i < items.Count; i++)
            {
                KeyValuePair<int, int> keyValuePair = countList[i];
                View_ProjectSekai_MobPostRank_Item item = items[i];
                item.SetMobId(i + 1, keyValuePair.Key, keyValuePair.Value);
            }
        }

        HashSet<int> GetMobIdsByTags(List<string> tags)
        {
            HashSet<int> mobIds = new HashSet<int>();
            foreach (var tag in tags)
            {
                if (nicknameDictionary.TryGetValue(tag, out int mobId))
                {
                    mobIds.Add(mobId);
                }
            }
            return mobIds;
        }
    }
}
