using AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_RankPercentage : MonoBehaviour
    {
        [Header("Components")]
        public List<View_ProjectSekai_RankPercentage_Line> items;
        [Header("Adapter")]
        public Pixiv_SearchResponseLoader searchResponseLoader;

        CharacterPostCountManager countManager;
        public CharacterPostCountManager CountManager => countManager;

        private void Start()
        {
            CharacterGetterByPixivTag characterGetterByPixivTag = new CharacterGetterByPixivTag();

            Extra.Pixiv.SearchResponse.MergedResponse mergedResponse = searchResponseLoader.MergedResponse;
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

            List<RankPercentage> dataList = countManager.GetRankPercentageList();

            for (int i = 0; i < items.Count; i++)
            {
                View_ProjectSekai_RankPercentage_Line item = items[i];
                RankPercentage rankPercentage = dataList[i];
                item.SetData(i + 1, rankPercentage.percentageByCharacter.ToList());
            }
        }
    }
}
