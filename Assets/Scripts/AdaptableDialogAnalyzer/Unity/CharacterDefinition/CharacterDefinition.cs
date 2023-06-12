using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 角色定义
    /// </summary>
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/CharacterDefinition/CharacterDefinition")]
    public class CharacterDefinition : ScriptableObject
    {
        public List<Character> characters;

        public Character this[int index] => characters[index];
    }
}