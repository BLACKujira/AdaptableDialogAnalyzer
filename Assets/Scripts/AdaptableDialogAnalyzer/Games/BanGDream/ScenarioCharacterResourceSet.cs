// ScenarioCharacterResourceSet
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public struct ScenarioCharacterResourceSet
    {
        public int characterId;
        public string costumeType;

        public ScenarioCharacterResourceSet(int characterId, string costumeType)
        {
            throw new NotImplementedException();
        }

        public string GetResourceKey()
        {
            return null;
        }

        public static string GetResourceKey(int characterId, string costumeType)
        {
            return null;
        }

        public string GetExceptionCostumeKey()
        {
            return null;
        }
    }
}