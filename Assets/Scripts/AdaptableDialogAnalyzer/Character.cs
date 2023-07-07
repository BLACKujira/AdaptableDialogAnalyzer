using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer
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

        string namae = null;
        /// <summary>
        /// 获取角色名称最后一个空格后的字符串，有缓存，不用担心性能消耗
        /// </summary>
        public string Namae
        {
            get
            {
                if(string.IsNullOrEmpty(namae)) namae = GetNamae();
                return namae;
            }
        }
        string GetNamae()
        {
            string[] nameArray = name.Split(' ');
            return nameArray[nameArray.Length - 1];
        }

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