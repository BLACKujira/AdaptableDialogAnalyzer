// ActionSetAreaItem
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ActionSetAreaItem
    {
        public int categoryId;
        public string animatonName;
        public bool isLoop;

        public bool IsSame(int categoryId, string animatonName, bool isLoop)
        {
            return default;
        }

        public bool IsSame(ActionSetAreaItem item)
        {
            return default;
        }
    }
}