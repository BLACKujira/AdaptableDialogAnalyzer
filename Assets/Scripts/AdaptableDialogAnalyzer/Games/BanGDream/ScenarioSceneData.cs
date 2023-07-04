// ScenarioSceneData
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ScenarioSceneData
    {
        public string scenarioSceneId;
        public StoryType storyType;
        public ScenarioCharacterResourceSet[] appearCharacters;
        public ScenarioCharacterLayout[] firstLayout;
        public string firstBgm;
        public string firstBackground;
        public string firstBackgroundBundleName;
        public List<ScenarioSnippet> snippets;
        public List<ScenarioSnippetTalk> talkData;
        public List<ScenarioSnippetSelectable> selectableData;
        public List<ScenarioSnippetCharacterLayout> layoutData;
        public List<ScenarioSnippetSpecialEffect> specialEffectData;
        public List<ScenarioSnippetSound> soundData;
        public List<string> needBundleNames;
        public List<string> includeSoundDataBundleNames;

        public bool IsDigestScene
        {
            get
            {
                return default;
            }
        }

        public void EnqueueAppearCharacters(Queue<ScenarioCharacterResourceSet> targetQueue)
        {
        }

        public void CollectNeedBundleNames(List<string> useOtherBundles)
        {
        }

        public void CollectFirstBackgroundBundleName(List<string> useOtherBundles)
        {
        }

        public void CollectSpecialEffectDataBundleNames(List<string> useOtherBundles)
        {
        }

        public void CollectDigestBundleNames(List<string> useOtherBundles)
        {
        }

        public void CollectIncludeSoundDataBundleNames(List<string> includeSoundDataBundles)
        {
        }

        public void CollectSoundDataBundleNames(List<string> loadedSoundBundles, List<string> useSoundBundles)
        {
        }
    }
}