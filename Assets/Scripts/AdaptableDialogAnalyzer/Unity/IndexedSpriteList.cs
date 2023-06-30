using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/IndexedSpriteList")]
    public class IndexedSpriteList : ScriptableObject
    {
        public List<IndexedSprite> indexedSprites = new List<IndexedSprite>();
        public Sprite defaultSprite;

        /// <summary>
        /// 不存在时返回默认sprite
        /// </summary>
        /// <param name="spriteId"></param>
        /// <returns></returns>
        public Sprite this[int spriteId]
        {
            get
            {
                Sprite sprite = TryGetSprite(spriteId);
                if(sprite == null)
                {
                    sprite = defaultSprite;
                    Debug.LogWarning($"不存在id为{spriteId}的sprite");
                }
                return sprite;
            }
        }

        /// <summary>
        /// 当不存在时返回null
        /// </summary>
        /// <param name="spriteId"></param>
        /// <returns></returns>
        public Sprite TryGetSprite(int spriteId)
        {
            foreach (var indexedSprite in indexedSprites)
            {
                if (indexedSprite.id == spriteId) return indexedSprite.sprite;
            }
            return null;
        }
    }
}