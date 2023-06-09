using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UniversalADVCounter.Unity
{
    /// <summary>
    /// 其他组件通过此组件获取每个故事章节
    /// </summary>
    /// <typeparam name="T">由原始文件生成的对象类型</typeparam>
    public abstract class ChapterGetter<T> : MonoBehaviour
    {
        [SerializeField]
        private string originalFileFolder;

        private Chapter<T>[] chapters;
        /// <summary>
        /// 故事章节
        /// </summary>
        public Chapter<T>[] Chapters => chapters;

        private void Awake()
        {
            LoadFiles();
        }

        /// <summary>
        /// 获取文件夹中所有的文件并生成对应的基础对话集
        /// </summary>
        protected virtual void LoadFiles()
        {
            string[] files = Directory.GetFiles(originalFileFolder);
            chapters = new Chapter<T>[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                chapters[i] = LoadFile(file);
            }
        }

        /// <summary>
        /// 将原始文件读取为类型为T的基础对话集
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected abstract Chapter<T> LoadFile(string filePath);
    }
}