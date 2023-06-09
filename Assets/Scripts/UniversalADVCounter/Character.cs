using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalADVCounter
{
    /// <summary>
    /// 表示游戏剧情中出现的一名角色
    /// </summary>
    [Serializable]
    public class Character
    {
        /// <summary>
        /// 角色的唯一ID，如游戏中存在尽量与其一致
        /// </summary>
        public int id;
        /// <summary>
        /// 角色的名称
        /// </summary>
        public string name;
        /// <summary>
        /// 角色的图标
        /// </summary>
        public Sprite icon;
        /// <summary>
        /// 角色的代表色
        /// </summary>
        public Color color;

        public Character()
        {
        }

        public Character(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}