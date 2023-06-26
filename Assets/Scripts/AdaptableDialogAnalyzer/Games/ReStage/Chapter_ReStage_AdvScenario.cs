using AdaptableDialogAnalyzer.Unity;
using AdaptableDialogAnalyzer.YAML;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace AdaptableDialogAnalyzer.Games.ReStage
{
    public class Chapter_ReStage_AdvScenario : Chapter
    {
        AdvScenario advScenario;
        public AdvScenario AdvScenario => advScenario;

        public ReStage_ChapterNameInfo ChapterNameInfo
        {
            get
            {
                if (ChapterID.Count(c => c == '_') != 2) return null;
                string[] nameArray = ChapterID.Split('_');
                string type = nameArray[0];
                if (!int.TryParse(nameArray[1], out int index1)) return null;
                if (!int.TryParse(nameArray[2], out int index2)) return null;
                return new ReStage_ChapterNameInfo(type, index1, index2);
            }
        }

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            List<BasicTalkSnippet> basicTalkSnippets = new List<BasicTalkSnippet>();
            for (int i = 0; i < advScenario.Pages.Count; i++)
            {
                AdvPageData advPageData = advScenario.Pages[i];

                int refIdx = i;

                int speakerId = (int)advPageData.nameCharacter;
                if (speakerId == 902) speakerId = 7;
                if (!GlobalConfig.CharacterDefinition.HasDefinition(speakerId)) speakerId = 0;

                string content = string.IsNullOrEmpty(advPageData.text) ? string.Empty : advPageData.text.Replace("\\n", "\n");
                string displayName = advPageData.name;

                BasicTalkSnippet basicTalkSnippet = new BasicTalkSnippet(refIdx, speakerId, content, displayName);
                basicTalkSnippets.Add(basicTalkSnippet);
            }
            return basicTalkSnippets.ToArray();
        }

        public bool IsVoiceOnly()
        {
            if (advScenario.Pages.Count >= 5
                && (advScenario.Pages[3].text?.Contains("……") ?? false)
                && (advScenario.Pages[3].text?.Contains("color") ?? false))
            {
                return true;
            }
            return false;
        }

        public static Chapter_ReStage_AdvScenario LoadText(string rawChapter)
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

        public static string GetTypeName(ReStage_ChapterNameInfo chapterNameInfo)
        {
            if (chapterNameInfo == null) return "其他剧情";
            switch (chapterNameInfo.type)
            {
                case "card": return $"卡面剧情 {chapterNameInfo.index1 / 1000 * 1000}";
                case "main": return "主线剧情";
                case "event": return "活动剧情";
                case "origin": return "原作小说";
            }
            return "其他剧情";
        }
    }
}