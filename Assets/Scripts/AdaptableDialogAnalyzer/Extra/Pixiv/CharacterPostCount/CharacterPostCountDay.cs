using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount
{
    public class CharacterPostCountDay
    {
        public DateTime date;
        public Dictionary<int, int> characterCountPairs = new Dictionary<int, int>();

        public CharacterPostCountDay(DateTime date)
        {
            this.date = date;
        }

        public void Add(int characterId, int count = 1)
        {
            if(!characterCountPairs.ContainsKey(characterId))
            {
                characterCountPairs[characterId] = 0;
            }
            characterCountPairs[characterId] += count;
        }
    }
}