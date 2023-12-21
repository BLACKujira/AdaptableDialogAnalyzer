using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{

    /// <summary>
    /// 定义角色对其他角色的昵称，在集合中的位置应与角色ID相同
    /// </summary>
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/NicknameDefinition/NicknameMapping")]
    public class NicknameMapping : ScriptableObject
    {
        public int speakerId;
        public List<NicknameList> nicknameLists = new List<NicknameList>();

        public NicknameList this[int mentionedPersonId]
        {
            get
            {
                foreach (var nicknameList in nicknameLists)
                {
                    if (nicknameList.mentionedPersonId == mentionedPersonId) return nicknameList;
                }
                throw new System.Exception($"未定义角色{mentionedPersonId}的昵称");
            }
        }

        /// <summary>
        /// 未定义时返回null
        /// </summary>
        /// <param name=""></param>
        public NicknameList TryGetNicknameList(int mentionedPersonId)
        {
            return nicknameLists.FirstOrDefault(nl=>nl.mentionedPersonId == mentionedPersonId);
        }

        /// <summary>
        /// 获取一个以昵称为键，角色ID为值的字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetCacheDictionary()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (var nicknameList in nicknameLists)
            {
                foreach (var nickname in nicknameList.nicknames)
                {
                    dictionary.Add(nickname, nicknameList.mentionedPersonId);
                }
            }
            return dictionary;
        }
    }
}