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
        [SerializeField] List<Character> characters;
        public Character[] Characters => characters.ToArray();

        public Character this[int characterId]
        {
            get
            {
                foreach (Character character in characters) 
                {
                    if(character.id == characterId) return character;
                }
                throw new System.Exception($"没有定义ID为{characterId}的角色");
            }
        }

        public bool HasDefinition(int characterId)
        {
            foreach (var character in characters)
            {
                if (character.id == characterId) return true; 
            }
            return false;
        }
    }
}