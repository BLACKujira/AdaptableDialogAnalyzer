using AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount;
using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_TotalAndDeltaCurve : MonoBehaviour
    {
        [Header("Components")]
        public View_ProjectSekai_TotalCurve totalCurve;
        public View_ProjectSekai_DeltaCurve deltaCurve;
        [Header("Adapter")]
        public Pixiv_SearchResponseLoader searchResponseLoader;

        CharacterPostCountManager countManager;
        public CharacterPostCountManager CountManager => countManager;

        int[] constCharacterId = new int[] { 1 };

        private void Awake()
        {
            Extra.Pixiv.SearchResponse.MergedResponse mergedResponse = searchResponseLoader.MergedResponse;
            CharacterPostCounter characterPostCounter = new CharacterPostCounter();

            characterPostCounter.getArtworkCharacters = (artwork) =>
            {
                return constCharacterId;
            };
            characterPostCounter.getNovelCharacters = (novel) =>
            {
                return constCharacterId;
            };

            CharacterPostCountManager characterPostCountManager = characterPostCounter.Count(mergedResponse);
            countManager = characterPostCountManager.ToTotalMode();
            countManager.CalcDelta();

            totalCurve.Initialize(countManager);
            deltaCurve.Initialize(countManager);
        }
    }
}
