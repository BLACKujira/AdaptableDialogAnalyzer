using AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount;
using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_PixivCharacterPostCount : MonoBehaviour
    {
        [Header("Components")]
        public View_ProjectSekai_TimelineTypeA timeline;

        [Header("Adapter")]
        public ProjectSekai_MasterLoader masterLoader;
        public Pixiv_SearchResponseLoader searchResponseLoader;

        CharacterPostCountManager countManager;
        public CharacterPostCountManager CountManager => countManager;

       Dictionary<string, int> tagCharacterIdPair;

        private void Awake()
        {
            tagCharacterIdPair = GlobalConfig.CharacterDefinition.Characters
                .Where(c => c.id >= 1 && c.id <= 20)
                .ToDictionary(c => c.name.Replace(" ", string.Empty), c => c.id);

            Extra.Pixiv.SearchResponse.MergedResponse mergedResponse = searchResponseLoader.MergedResponse;
            CharacterPostCounter characterPostCounter = new CharacterPostCounter();

            characterPostCounter.getArtworkCharacters = (artwork) =>
            {
                return GetCharacterByTag(artwork.tags);
            };
            characterPostCounter.getNovelCharacters = (novel) =>
            {
                return GetCharacterByTag(novel.tags);
            };

            CharacterPostCountManager characterPostCountManager = characterPostCounter.Count(mergedResponse);
            countManager = characterPostCountManager.ToTotalMode();

            timeline.Initialize(this);
        }

        int[] GetCharacterByTag(List<string> tags)
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
