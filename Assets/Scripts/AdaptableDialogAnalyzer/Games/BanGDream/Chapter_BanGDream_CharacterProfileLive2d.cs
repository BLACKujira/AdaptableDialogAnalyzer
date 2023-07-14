using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class Chapter_BanGDream_CharacterProfileLive2d : Chapter
    {
        List<MasterCharacterProfileLive2d> masterCharacterProfileLive2ds;
        public List<MasterCharacterProfileLive2d> MasterCharacterProfileLive2ds => masterCharacterProfileLive2ds;

        public static Chapter CreateFromMaster(List<MasterCharacterProfileLive2d> masterCharacterProfileLive2Ds)
        {
            Chapter_BanGDream_CharacterProfileLive2d chapter = new Chapter_BanGDream_CharacterProfileLive2d();
            chapter.masterCharacterProfileLive2ds = masterCharacterProfileLive2Ds;
            return chapter;
        }

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            List<BasicTalkSnippet> basicTalkSnippets = new List<BasicTalkSnippet>();
            for (int i = 0; i < masterCharacterProfileLive2ds.Count; i++)
            {
                MasterCharacterProfileLive2d masterCharacterProfileLive2d = masterCharacterProfileLive2ds[i];
                int refIdx = i;
                int talkerId = characterDefinition.HasDefinition((int)masterCharacterProfileLive2d.CharacterId) ? (int)masterCharacterProfileLive2d.CharacterId : 0;
                string content = masterCharacterProfileLive2d.Serif;
                string name = $"{characterDefinition[talkerId].Namae} m:{masterCharacterProfileLive2d.Motion} e:{masterCharacterProfileLive2d.Expression}";

                BasicTalkSnippet basicTalkSnippet = new BasicTalkSnippet(refIdx, talkerId, content, name);
                basicTalkSnippets.Add(basicTalkSnippet);
            }

            return basicTalkSnippets.ToArray();
        }
    }
}