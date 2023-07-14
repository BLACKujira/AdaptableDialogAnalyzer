using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class Chapter_BanGDream_CommonsLive2d : Chapter
    {
        List<MasterCommonsLive2d> masterCommonsLive2d;
        public List<MasterCommonsLive2d> MasterCommonsLive2d => masterCommonsLive2d;

        public static Chapter CreateFromMaster(List<MasterCommonsLive2d> masterCommonsLive2d)
        {
            Chapter_BanGDream_CommonsLive2d chapter = new Chapter_BanGDream_CommonsLive2d();
            chapter.masterCommonsLive2d = masterCommonsLive2d;
            return chapter;
        }

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            List<BasicTalkSnippet> basicTalkSnippets = new List<BasicTalkSnippet>();
            for (int i = 0; i < masterCommonsLive2d.Count; i++)
            {
                MasterCommonsLive2d masterCommonsLive2D = masterCommonsLive2d[i];
                int refIdx = i;
                int talkerId = characterDefinition.HasDefinition((int)masterCommonsLive2D.CharacterId) ? (int)masterCommonsLive2D.CharacterId : 0;
                string content = masterCommonsLive2D.Serif;
                string name = $"{characterDefinition[talkerId].Namae} m:{masterCommonsLive2D.Motion} e:{masterCommonsLive2D.Expression}";

                BasicTalkSnippet basicTalkSnippet = new BasicTalkSnippet(refIdx, talkerId, content, name);
                basicTalkSnippets.Add(basicTalkSnippet);
            }

            return basicTalkSnippets.ToArray();
        }
    }
}