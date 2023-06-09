using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalADVCounter
{
    /// <summary>
    /// 基础的对话片段、对应原始文件中的一句台词
    /// </summary>
    public class BasicTalkSnippet
    {
        private readonly int refIdx;
        private readonly int talkerId;
        private readonly string content;

        /// <summary>
        /// 构造函数，没什么特别的
        /// </summary>
        /// <param name="refIdx">标记此对话在原文件中的某个位置，可以为行数或此对话在数组中的位置</param>
        /// <param name="talkerId">当前台词所属角色的ID、若原文件中未写明可通过对话窗口显示的名称或显示的立绘获取</param>
        /// <param name="content">对话内容（不含说话者的名字）</param>
        public BasicTalkSnippet(int refIdx, int talkerId, string content)
        {
            this.refIdx = refIdx;
            this.talkerId = talkerId;
            this.content = content;
        }

        /// <summary>
        /// 标记此对话在原文件中的某个位置，可以为行数或在此对话数组中的位置
        /// </summary>
        public int RefIdx => refIdx;
        /// <summary>
        /// 当前台词所属角色的ID，与角色列表中的一致
        /// </summary>
        public int TalkerId => talkerId;
        /// <summary>
        /// 对话内容（不含说话者的名字）
        /// </summary>
        public string Content => content;
    }
}