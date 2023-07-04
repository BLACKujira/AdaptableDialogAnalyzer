using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class Chapter_BanGDream_Scenario : Chapter
    {
        /// <summary>
        /// 原始对象
        /// </summary>
        ScenarioSceneData scenarioSceneData;
        public ScenarioSceneData ScenarioSceneData => scenarioSceneData;

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            List<BasicTalkSnippet> baseTalkDatas = new List<BasicTalkSnippet>();
            for (int i = 0; i < scenarioSceneData.snippets.Count; i++)
            {
                ScenarioSnippet scenarioSnippet = scenarioSceneData.snippets[i];
                //首先检查场景剧情片段的类型是否为对话（ActionType.Talk），如果不是，则跳过该片段。
                if (scenarioSnippet.actionType != ScenarioSnippet.ActionType.Talk)
                    continue;
                ScenarioSnippetTalk scenarioSnippetTalk = scenarioSceneData.talkData[scenarioSnippet.referenceIndex];

                //根据参考索引（ReferenceIndex）获取对话数据（ScenarioSnippetTalk）。根据对话数据中的信息，创建一个基本对话片段对象（BasicTalkSnippet）
                int refIdx = i;
                int talkerId = BanGDreamHelper.GetCharacterId_Scenario(scenarioSnippetTalk);
                string content = scenarioSnippetTalk.body;
                string name = scenarioSnippetTalk.windowDisplayName;

                BasicTalkSnippet baseTalkData = new BasicTalkSnippet(refIdx, talkerId, content, name);
                baseTalkDatas.Add(baseTalkData);
            }
            return baseTalkDatas.ToArray();
        }

        /// <summary>
        /// 从文件读取
        /// </summary>
        /// <param name="rawChapter"></param>
        /// <returns></returns>
        public static Chapter LoadText(string rawChapter)
        {
            Chapter_BanGDream_Scenario chapter_Scenario = new Chapter_BanGDream_Scenario();
            ScenarioSceneData scenarioSceneData = JsonUtility.FromJson<ScenarioSceneData>(rawChapter);

            chapter_Scenario.scenarioSceneData = scenarioSceneData;
            return chapter_Scenario;
        }
    }
}