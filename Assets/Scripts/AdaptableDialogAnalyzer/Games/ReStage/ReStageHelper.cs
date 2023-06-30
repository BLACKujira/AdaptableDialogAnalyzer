using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.Games.ReStage
{
    public static class ReStageHelper
    {
        static Dictionary<int, UnitGroup[]> charUnitMap = new Dictionary<int, UnitGroup[]>()
        {
            { (int)AdvCharacter.ShikimiyaMana, new UnitGroup[] { UnitGroup.Kirare } },
            { (int)AdvCharacter.SayuTsukisaka, new UnitGroup[] { UnitGroup.Kirare } },
            { (int)AdvCharacter.MizuhaIchikishima, new UnitGroup[] { UnitGroup.Kirare } },
            { (int)AdvCharacter.KaeHiiragi, new UnitGroup[] { UnitGroup.Kirare } },
            { (int)AdvCharacter.KasumiHonjou, new UnitGroup[] { UnitGroup.Kirare } },
            { (int)AdvCharacter.MiiHasegawa, new UnitGroup[] { UnitGroup.Kirare } },
            { (int)AdvCharacter.YukariItsumura, new UnitGroup[] { UnitGroup.Ortensia, UnitGroup.AsterReve } },
            { (int)AdvCharacter.HarukaItsumura, new UnitGroup[] { UnitGroup.Ortensia } },
            { (int)AdvCharacter.AoneShikimiya, new UnitGroup[] { UnitGroup.StellaMaris, UnitGroup.StellaMaris2 } },
            { (int)AdvCharacter.RukaIchijou, new UnitGroup[] { UnitGroup.StellaMaris, UnitGroup.StellaMaris2 } },
            { (int)AdvCharacter.SangoMisaki, new UnitGroup[] { UnitGroup.StellaMaris, UnitGroup.StellaMaris2 } },
            { (int)AdvCharacter.AmahaShiratori, new UnitGroup[] { UnitGroup.TroisAnges } },
            { (int)AdvCharacter.KanadeHokaze, new UnitGroup[] { UnitGroup.TroisAnges } },
            { (int)AdvCharacter.NagisaHimura, new UnitGroup[] { UnitGroup.TroisAnges } },
            { (int)AdvCharacter.MikuruBandou, new UnitGroup[] { UnitGroup.TetraRkhia } },
            { (int)AdvCharacter.HakuNishidate, new UnitGroup[] { UnitGroup.TetraRkhia } },
            { (int)AdvCharacter.AkariHaeno, new UnitGroup[] { UnitGroup.TetraRkhia } },
            { (int)AdvCharacter.KurohaShirokita, new UnitGroup[] { UnitGroup.TetraRkhia } },
        };

        /// <summary>
        /// 获取角色所在的全部组合（id7和902已经合并，此角色在两个组合）
        /// </summary>
        public static UnitGroup[] GetCharacterUnits(int characterId)
        {
            if (!charUnitMap.ContainsKey(characterId)) return new UnitGroup[0];
            return charUnitMap[characterId];
        }

        /// <summary>
        /// 获取角色所在的主要组合
        /// </summary>
        public static UnitGroup GetCharacterMainUnit(int characterId)
        {
            if (!charUnitMap.ContainsKey(characterId)) return UnitGroup.None;
            return charUnitMap[characterId][0];
        }

        /// <summary>
        /// 两名角色是否在同一组合（多组合角色只要有一个组合相同则为True） 
        /// </summary>
        public static bool IsInSameGroup(int characterAId, int characterBId)
        {
            UnitGroup[] unitGroupsA = GetCharacterUnits(characterAId);
            UnitGroup[] unitGroupsB = GetCharacterUnits(characterBId);
            return unitGroupsA.Intersect(unitGroupsB).Any();
        }
    }
}