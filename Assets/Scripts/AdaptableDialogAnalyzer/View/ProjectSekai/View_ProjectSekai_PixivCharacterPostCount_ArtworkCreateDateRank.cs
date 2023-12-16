using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_PixivCharacterPostCount_ArtworkCreateDateRank : MonoBehaviour
    {
        [Header("Components")]
        public List<View_ProjectSekai_PixivCharacterPostCount_Rank_Item> items;
        [Header("Adapter")]
        public ProjectSekai_MasterLoader masterLoader;
        public Pixiv_SearchResponseLoader searchResponseLoader;

        Dictionary<int, DateTime> firstCreateDate = new Dictionary<int, DateTime>();

        private void Start()
        {
            Extra.Pixiv.SearchResponse.MergedResponse mergedResponse = searchResponseLoader.MergedResponse;
            CharacterGetterByPixivTag characterGetterByPixivTag = new CharacterGetterByPixivTag();

            foreach (var artwork in mergedResponse.artworks)
            {
                int[] characterIds = characterGetterByPixivTag.GetCharacterByTag(artwork.tags);
                DateTime createDate = artwork.CreateDate;

                foreach (int characterId in characterIds)
                {
                    if (!firstCreateDate.ContainsKey(characterId))
                    {
                        firstCreateDate.Add(characterId, createDate);
                    }
                    else
                    {
                        if (firstCreateDate[characterId] > createDate)
                        {
                            firstCreateDate[characterId] = createDate;
                        }
                    }
                }
            }

            List<KeyValuePair<int, DateTime>> characterIdCreateDatePairs = firstCreateDate
                .OrderBy(kvp => kvp.Value)
                .ToList();

            for (int i = 0; i < items.Count; i++)
            {
                View_ProjectSekai_PixivCharacterPostCount_Rank_Item rankItem = items[i];
                KeyValuePair<int, DateTime> pair = characterIdCreateDatePairs[i];
                rankItem.SetData(pair.Key, i + 1, pair.Value.ToString("yy/MM/dd"));
            }
        }
    }
}
