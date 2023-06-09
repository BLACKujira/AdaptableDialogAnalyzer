﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalADVCounter
{
    /// <summary>
    /// 表示游戏的对象列表
    /// </summary>
    [System.Serializable]
    public class Characters : ISerializable<string>
    {
        public Character[] characters;

        /// <summary>
        /// 通过角色列表生成对象
        /// </summary>
        /// <param name="characters"></param>
        public Characters(Character[] characters)
        {
            this.characters = characters;
        }

        /// <summary>
        /// 获取id为index的角色
        /// </summary>
        /// <param name="index"></param>
        /// <returns>id为index的角色</returns>
        public Character this[int index] => characters[index];

        /// <summary>
        /// 使用JsonUtility.ToJson序列化
        /// </summary>
        /// <returns>JSON字符串</returns>
        public string GetSaveData()
        {
            return JsonUtility.ToJson(this);
        }

        /// <summary>
        /// 使用JsonUtility.FromJson反序列化
        /// </summary>
        /// <returns></returns>
        public static Characters Load(string serializedData)
        {
            return JsonUtility.FromJson<Characters>(serializedData);
        }
    }
}