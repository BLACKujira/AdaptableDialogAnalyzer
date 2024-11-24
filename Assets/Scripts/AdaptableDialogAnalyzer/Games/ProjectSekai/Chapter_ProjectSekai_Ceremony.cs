using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    /// <summary>
    /// 虚拟LIVE剧情
    /// </summary>
    public class Chapter_ProjectSekai_Ceremony : Chapter
    {
        /// <summary>
        /// 原始对象
        /// </summary>
        public MasterOfCeremonyData masterOfCeremonyData;
        public MasterOfCeremonyData MasterOfCeremonyData => masterOfCeremonyData;

        public int[] characterIdInTalkEvents;

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            List<BasicTalkSnippet> baseTalkDatas = new List<BasicTalkSnippet>();
            for (int i = 0; i < masterOfCeremonyData.characterTalkEvents.Length; i++)
            {
                CharacterTalkEvent characterTalkEvent = masterOfCeremonyData.characterTalkEvents[i];
                int refIdx = i;
                int talkerId = characterIdInTalkEvents[i];
                string name = ConstData.characters[talkerId].namae;
                string content = characterTalkEvent.Serif;
                BasicTalkSnippet basicTalkSnippet = new BasicTalkSnippet(refIdx, talkerId, content, name);
                baseTalkDatas.Add(basicTalkSnippet);
            }
            return baseTalkDatas.ToArray();
        }

        /// <summary>
        /// 从文件读取
        /// </summary>
        /// <param name="rawChapter"></param>
        /// <returns></returns>
        public static Chapter LoadText(string rawChapter, MasterCharacter3D[] character3ds)
        {
            Chapter_ProjectSekai_Ceremony chapter_Ceremony = new Chapter_ProjectSekai_Ceremony();
            MasterOfCeremonyData masterOfCeremonyData = JsonUtility.FromJson<MasterOfCeremonyData>(rawChapter);
            chapter_Ceremony.masterOfCeremonyData = masterOfCeremonyData;

            chapter_Ceremony.characterIdInTalkEvents = new int[masterOfCeremonyData.characterTalkEvents.Length];
            Dictionary<int, int> dicC3dId = new Dictionary <int, int>();
            foreach (var masterCharacter3D in character3ds)
            {
                dicC3dId[masterCharacter3D.id] = masterCharacter3D.characterId;
            }
            for (int i = 0; i < chapter_Ceremony.characterIdInTalkEvents.Length; i++)
            {
                int c3dId = masterOfCeremonyData.characterTalkEvents[i].Character3dId;
                chapter_Ceremony.characterIdInTalkEvents[i] = dicC3dId.ContainsKey(c3dId) ? dicC3dId[c3dId] : 0;
            }

            return chapter_Ceremony;
        }
    }
}