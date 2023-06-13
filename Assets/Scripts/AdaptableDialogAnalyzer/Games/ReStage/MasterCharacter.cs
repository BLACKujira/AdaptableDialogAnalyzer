using System;

namespace AdaptableDialogAnalyzerzer.Games.ReStage
{
    [Serializable]
    public class MasterCharacter
    {
        public int id;

        public string name;

        public string read;

        public int unit_id;

        public string res_id;

        public string image_color;

        public string cv;

        public string description;

        private const int CHARACTER_ID_COLLABO_LIlITH = 117;

        private const int CHARACTER_ID_COLLABO_LIlITH_STATUE = 119;

        private const int CHARACTER_ID_COLLABO_ARISU_SUZUSHIMA = 125;

        private const int CHARACTER_ID_COLLABO_ARISU_SUZUSHIMA2 = 126;

        public const int CHARACTER_ID_COLLABO_KURO_MOMO = 141;

        private int _cachedSpecialId;

        public int SpecialID_ForSD => 0;

        public int SpecialID_ForCharacterDetail => 0;

        public int SpecialID_ForLiveStage => 0;

        public static int ReplaceCharacterIdForStory(int charaterId)
        {
            return 0;
        }

        public int GetSpecialId(int cardId)
        {
            return 0;
        }

        public void ClearCachedSpecialId()
        {
        }

        private int RepalaceSpecialCharacterId(int characterId, int specialId, bool cache = false)
        {
            return 0;
        }
    }
}