// Sekai.ScenarioSceneData
using UnityEngine;

/// <summary>
/// PJSK中一般剧情及地图对话使用的类
/// </summary>
namespace UniversalADVCounter.Games.ProjectSekai
{
    [System.Serializable]
    public class ScenarioSceneData
    {
        public string ScenarioId;
        public ScenarioCharacterResourceSet[] AppearCharacters;
        public ScenarioCharacterLayout[] FirstLayout;
        public string FirstBgm;
        public string FirstBackground;
        public ScenarioSnippet[] Snippets;
        public ScenarioSnippetTalk[] TalkData;
        public ScenarioSnippetCharacterLayout[] LayoutData;
        public ScenarioSnippetSpecialEffect[] SpecialEffectData;
        public ScenarioSnippetSound[] SoundData;
        public string[] NeedBundleNames;
        public string[] IncludeSoundDataBundleNames;
    }
}