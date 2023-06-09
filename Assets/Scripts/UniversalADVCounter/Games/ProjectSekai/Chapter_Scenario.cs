using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UniversalADVCounter.Games.ProjectSekai
{
    public class Chapter_Scenario : Chapter<ScenarioSceneData>
    {
        public Chapter_Scenario(string filePath) : base(filePath)
        {
        }

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            List<BasicTalkSnippet> baseTalkDatas = new List<BasicTalkSnippet>();
            for (int i = 0; i < OriginalObject.Snippets.Length; i++)
            {
                ScenarioSnippet scenarioSnippet = OriginalObject.Snippets[i];
                if (scenarioSnippet.Action != ScenarioSnippet.ActionType.Talk)
                    continue;
                ScenarioSnippetTalk scenarioSnippetTalk = OriginalObject.TalkData[scenarioSnippet.ReferenceIndex];
                int refIdx = i;
                int talkerId = ConstData.GetCharacterId_Scenario(scenarioSnippetTalk);
                string content = scenarioSnippetTalk.Body;
                BasicTalkSnippet baseTalkData = new BasicTalkSnippet(refIdx,talkerId,content);
                baseTalkDatas.Add(baseTalkData);
            }
            return baseTalkDatas.ToArray();
        }

        public override void Initialize(string filePath)
        {
            originalObject = JsonUtility.FromJson<ScenarioSceneData>(File.ReadAllText(filePath));
        }
    }
}