using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class Chapter_BanGDream_Live2dSerif : Chapter
    {
        List<MasterLive2dSerif> masterLive2DSerifs;
        public List<MasterLive2dSerif> MasterLive2DSerifs => masterLive2DSerifs;

        public static Chapter CreateFromMaster(List<MasterLive2dSerif> masterLive2DSerifs)
        {
            Chapter_BanGDream_Live2dSerif chapter_BanGDream_Live2dSerif = new Chapter_BanGDream_Live2dSerif();
            chapter_BanGDream_Live2dSerif.masterLive2DSerifs = masterLive2DSerifs;
            return chapter_BanGDream_Live2dSerif;
        }

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            List<BasicTalkSnippet> basicTalkSnippets = new List<BasicTalkSnippet>();
            for (int i = 0; i < masterLive2DSerifs.Count; i++)
            {
                MasterLive2dSerif masterLive2DSerif = masterLive2DSerifs[i];
                int refIdx = i;
                int talkerId = (int)masterLive2DSerif.CharacterId;
                string content = masterLive2DSerif.Serif;
                string name = $"{characterDefinition[talkerId].Namae} m:{masterLive2DSerif.Motion} e:{masterLive2DSerif.Expression}";

                BasicTalkSnippet basicTalkSnippet = new BasicTalkSnippet(refIdx, talkerId, content, name);
                basicTalkSnippets.Add(basicTalkSnippet);
            }

            return basicTalkSnippets.ToArray();
        }
    }
}