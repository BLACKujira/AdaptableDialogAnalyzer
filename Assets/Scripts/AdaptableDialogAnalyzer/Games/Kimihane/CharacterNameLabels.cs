using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public static class CharacterNameLabels
    {
        static Dictionary<string, int> nameMap = new Dictionary<string, int>()
        {
            { "ショートの子／陽菜", 1 },
            { "陽菜", 1 },
            { "倫", 2 },
            { "文", 3 },
            { "おさげの子／文", 3 },
            { "祥子", 4 },
            { "聖夜子", 5 },
        };

        /// <summary>
        /// 通过窗口显示的名称判断角色
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static int GetCharacterID(string displayName)
        {
            if(nameMap.ContainsKey(displayName)) return nameMap[displayName];
            return 0;
        }
    }
}