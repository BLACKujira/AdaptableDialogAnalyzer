using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity.MessageBox
{
    /// <summary>
    /// 有一个或多个按钮的消息窗口
    /// </summary>
    public class MessageBox : MonoBehaviour
    {
        [SerializeField] private Text txt_Title;
        [SerializeField] private Text txt_Message;
        [SerializeField] private List<Button> buttons;

        public Text Txt_Title => txt_Title;
        public Text Txt_Message => txt_Message;
        public List<Button> Buttons => buttons;

        /// <summary>
        /// 初始化消息窗口
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="onCloseEvents">依次赋予每个按钮点击时的事件</param>
        public void Initialize(string title,string message,params Action[] onCloseEvents)
        {
            txt_Title.text = title;
            txt_Message.text = message;
            if (buttons.Count != onCloseEvents.Length)
                throw new NumberOfEventNotMatchException();
            for (int i = 0; i < buttons.Count; i++)
            {
                int id = i;
                buttons[id].onClick.AddListener(() =>
                {
                    if (onCloseEvents[id] != null)
                        onCloseEvents[id]();
                    Destroy(gameObject);
                });
            }
        }


        [Serializable]
        public class NumberOfEventNotMatchException : Exception
        {
            public NumberOfEventNotMatchException() { }
            public NumberOfEventNotMatchException(string message) : base(message) { }
            public NumberOfEventNotMatchException(string message, Exception inner) : base(message, inner) { }
            protected NumberOfEventNotMatchException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}