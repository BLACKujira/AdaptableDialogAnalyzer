using AdaptableDialogAnalyzer.Games.ProjectSekai;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class Chapter_BanGDream_BackstageTalk : Chapter
    {
        BackstageTalkSet backstageTalkSet;
        public BackstageTalkSet BackstageTalkSet => backstageTalkSet;

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            List<BasicTalkSnippet> basicTalkSnippets = new List<BasicTalkSnippet>();
            for (int i = 0; i < backstageTalkSet.snippets.Count; i++)
            {
                BackstageTalkSnippet backstageTalkSnippet = backstageTalkSet.snippets[i];
                int refIdx = i;
                int talkerId = BanGDreamHelper.GetCharacterId_BackstageTalk(backstageTalkSnippet);
                string content = backstageTalkSnippet.talkText;

                BasicTalkSnippet basicTalkSnippet = new BasicTalkSnippet(refIdx, talkerId, content);
                basicTalkSnippets.Add(basicTalkSnippet);
            }
            return basicTalkSnippets.ToArray();
        }

        public static Chapter LoadText(string rawChapter)
        {
            Chapter_BanGDream_BackstageTalk chapter = new Chapter_BanGDream_BackstageTalk();
            chapter.backstageTalkSet = JsonUtility.FromJson<BackstageTalkSet>(rawChapter);
            return chapter;
        }
    }
}