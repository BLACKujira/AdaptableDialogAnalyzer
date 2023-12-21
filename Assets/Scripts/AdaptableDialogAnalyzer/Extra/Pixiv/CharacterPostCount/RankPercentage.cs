using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount
{
    public class RankPercentage
    {
        public int rank;
        public Dictionary<int, float> percentageByCharacter = new Dictionary<int, float>();

        public RankPercentage(int rank)
        {
            this.rank = rank;
        }
    }
}