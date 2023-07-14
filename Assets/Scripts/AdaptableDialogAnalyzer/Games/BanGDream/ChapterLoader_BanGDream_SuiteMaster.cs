using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class ChapterLoader_BanGDream_SuiteMaster : ChapterLoader
    {
        public BanGDream_SuiteMasterLoader suiteMasterLoader;

        public override Chapter[] InitializeChapters()
        {
            List<Chapter> chapters = new List<Chapter>();

            MasterLive2dSerifMap masterLive2DSerifMap = suiteMasterLoader.SuiteMasterGetResponse.MasterLive2DSerifMap;
            foreach (var keyValuePair in masterLive2DSerifMap.Entries)
            {
                Chapter chapter = Chapter_BanGDream_Live2dSerif.CreateFromMaster(keyValuePair.Value.Entries.ToList());
                chapter.ChapterID = keyValuePair.Key.ToString();
                chapter.ChapterTitle = chapter.ChapterID;
                chapter.ChapterType = "Live2dSerif";
                chapters.Add(chapter);
            }

            MasterCommonsLive2dMap masterCommonsLive2DMap = suiteMasterLoader.SuiteMasterGetResponse.MasterCommonsLive2DMap;
            List<MasterCommonsLive2d>[] masterCommonsLive2Ds = masterCommonsLive2DMap.Entries
                .Select(kvp=>kvp.Value)
                .GroupBy(i => i.CharacterId)
                .Select(g => g.ToList())
                .ToArray();
            foreach (var masterCommonsLive2D in masterCommonsLive2Ds)
            {
                Chapter chapter = Chapter_BanGDream_CommonsLive2d.CreateFromMaster(masterCommonsLive2D);
                chapter.ChapterID = masterCommonsLive2D[0].CharacterId.ToString();
                chapter.ChapterTitle = chapter.ChapterID;
                chapter.ChapterType = "CommonsLive2D";
                chapters.Add(chapter);
            }

            MasterCharacterProfileLive2dMap masterCharacterProfileLive2DMap = suiteMasterLoader.SuiteMasterGetResponse.MasterCharacterProfileLive2DMap;
            List<MasterCharacterProfileLive2d>[] masterCharacterProfileLive2Ds = masterCharacterProfileLive2DMap.Entries
                .Select(kvp => kvp.Value)
                .GroupBy(i => i.CharacterId)
                .Select(g => g.ToList())
                .ToArray();
            foreach (var masterCharacterProfileLive2D in masterCharacterProfileLive2Ds)
            {
                Chapter chapter = Chapter_BanGDream_CharacterProfileLive2d.CreateFromMaster(masterCharacterProfileLive2D);
                chapter.ChapterID = masterCharacterProfileLive2D[0].CharacterId.ToString();
                chapter.ChapterTitle = chapter.ChapterID;
                chapter.ChapterType = "ProfileLive2D";
                chapters.Add(chapter);
            }

            return chapters.ToArray();
        }
    }
}