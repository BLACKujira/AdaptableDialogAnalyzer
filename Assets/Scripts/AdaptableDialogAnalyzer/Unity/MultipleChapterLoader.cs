using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 其他组件通过此组件获取故事章节，可以一次读取篇剧情，可能会通过其存放文件夹判断剧情类型
    /// </summary>
    /// <typeparam name="T">由原始文件生成的对象类型</typeparam>
    public abstract class MultipleChapterLoader : MonoBehaviour
    {
        public abstract Chapter[] GetChapters();
    }
}