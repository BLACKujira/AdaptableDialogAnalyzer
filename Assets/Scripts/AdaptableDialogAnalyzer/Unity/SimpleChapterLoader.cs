using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 其他组件通过此组件获取故事章节，有TextAsset和文本文件两种，提供反序列化和获取通用对话数据的方法
    /// DEBUG用，只测试读取逻辑，不需要设置各项属性，可以不用在这个类上花太长时间
    /// </summary>
    /// <typeparam name="T">由原始文件生成的对象类型</typeparam>
    public abstract class SimpleChapterLoader : MonoBehaviour
    {
        public abstract Chapter GetChapter();
    }
}