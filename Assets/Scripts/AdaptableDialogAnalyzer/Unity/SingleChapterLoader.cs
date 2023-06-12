using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 其他组件通过此组件获取故事章节，有TextAsset和文本文件两种，提供反序列化和获取通用对话数据的方法
    /// 如果不特别设置不能判断剧情类型
    /// </summary>
    /// <typeparam name="T">由原始文件生成的对象类型</typeparam>
    public abstract class SingleChapterLoader : MonoBehaviour
    {
        public abstract Chapter GetChapter();
    }
}