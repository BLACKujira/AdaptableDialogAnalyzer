using AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount;
using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_PixivCharacterPostCount_Rank : MonoBehaviour
    {
        [Header("Components")]
        public List<View_ProjectSekai_PixivCharacterPostCount_Rank_Item> items;
        [Header("Adapter")]
        public ProjectSekai_MasterLoader masterLoader;
        public Pixiv_SearchResponseLoader searchResponseLoader;

        CharacterPostCountManager countManager;
        public CharacterPostCountManager CountManager => countManager;

        private void Start()
        {
            CharacterGetterByPixivTag characterGetterByPixivTag = new CharacterGetterByPixivTag();

            MergedResponse mergedResponse = searchResponseLoader.MergedResponse;
            CharacterPostCounter characterPostCounter = new CharacterPostCounter();

            characterPostCounter.getArtworkCharacters = (artwork) =>
            {
                return characterGetterByPixivTag.GetCharacterByTag(artwork.tags);
            };
            characterPostCounter.getNovelCharacters = (novel) =>
            {
                return characterGetterByPixivTag.GetCharacterByTag(novel.tags);
            };

            CharacterPostCountManager characterPostCountManager = characterPostCounter.Count(mergedResponse);
            countManager = characterPostCountManager.ToTotalMode();

            CharacterPostCountDay characterPostCountDay = countManager.days
                .OrderByDescending(kvp => kvp.Key)
                .Select(kvp => kvp.Value)
                .First();

            List<CharacterPostCountDayItem> characterPostCountDayItems = characterPostCountDay.characterTotalPairs
                .OrderByDescending(kvp => kvp.Value.Value)
                .Select(kvp => kvp.Value)
                .ToList();

            for (int i = 0; i < items.Count; i++)
            {
                View_ProjectSekai_PixivCharacterPostCount_Rank_Item rankItem = items[i];
                CharacterPostCountDayItem dayItem = characterPostCountDayItems[i];
                rankItem.SetData(dayItem.characterId, i + 1, dayItem.Value.ToString());
            }
        }
    }
}
