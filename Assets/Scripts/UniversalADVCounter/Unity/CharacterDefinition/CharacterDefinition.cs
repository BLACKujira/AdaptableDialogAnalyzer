using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalADVCounter.Unity.CharacterEditor
{
    /// <summary>
    /// 角色定义
    /// </summary>
    [CreateAssetMenu(menuName = "UniversalADVCounter/CharacterDefinition/CharacterDefinition")]
    public class CharacterDefinition : ScriptableObject
    {
        public List<Character> characters;
    }
}