using AdaptableDialogAnalyzer.YAML;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace AdaptableDialogAnalyzer.Games.ReStage
{
    public class Chapter_ReStage_AdvScenario : Chapter
    {
        AdvScenario advScenario;
        public AdvScenario AdvScenario => advScenario;

        public static Chapter LoadText(string rawChapter)
        {
            IDeserializer deserializer = new DeserializerBuilder()
                .WithNodeTypeResolver(new UnityTypeResolver<MonoBehaviourRoot<AdvScenario>>())
                .WithNodeDeserializer(new UnityBooleanDeserializer())
                .Build();
            AdvScenario advScenario = deserializer.Deserialize<MonoBehaviourRoot<AdvScenario>>(rawChapter).MonoBehaviour;
            Chapter_ReStage_AdvScenario chapter_ReStage_AdvScenario = new Chapter_ReStage_AdvScenario();
            chapter_ReStage_AdvScenario.advScenario = advScenario;
            return chapter_ReStage_AdvScenario;
        }

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            List<BasicTalkSnippet> basicTalkSnippets = new List<BasicTalkSnippet>();
            for (int i = 0; i < advScenario.Pages.Count; i++)
            {
                AdvPageData advPageData = advScenario.Pages[i];

                int refIdx = i;
                int speakerId = (int)advPageData.nameCharacter > 18 ? 0 : (int)advPageData.nameCharacter;
                string content = advPageData.text.Replace("\\n","\n");
                string displayName = advPageData.name;

                BasicTalkSnippet basicTalkSnippet = new BasicTalkSnippet(refIdx, speakerId, content, displayName);
                basicTalkSnippets.Add(basicTalkSnippet);
            }
            return basicTalkSnippets.ToArray();
        }
    }
}