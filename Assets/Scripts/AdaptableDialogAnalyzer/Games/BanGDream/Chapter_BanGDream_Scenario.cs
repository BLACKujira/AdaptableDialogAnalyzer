using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class Chapter_BanGDream_Scenario : Chapter
    {
        /// <summary>
        /// ԭʼ����
        /// </summary>
        ScenarioSceneData scenarioSceneData;
        public ScenarioSceneData ScenarioSceneData => scenarioSceneData;

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            List<BasicTalkSnippet> baseTalkDatas = new List<BasicTalkSnippet>();
            for (int i = 0; i < scenarioSceneData.snippets.Count; i++)
            {
                ScenarioSnippet scenarioSnippet = scenarioSceneData.snippets[i];
                //���ȼ�鳡������Ƭ�ε������Ƿ�Ϊ�Ի���ActionType.Talk����������ǣ���������Ƭ�Ρ�
                if (scenarioSnippet.actionType != ScenarioSnippet.ActionType.Talk)
                    continue;
                ScenarioSnippetTalk scenarioSnippetTalk = scenarioSceneData.talkData[scenarioSnippet.referenceIndex];

                //���ݲο�������ReferenceIndex����ȡ�Ի����ݣ�ScenarioSnippetTalk�������ݶԻ������е���Ϣ������һ�������Ի�Ƭ�ζ���BasicTalkSnippet��
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
        /// ���ļ���ȡ
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