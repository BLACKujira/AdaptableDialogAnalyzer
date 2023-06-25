using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class Core : MonoBehaviour
    {
        [SerializeField] private string workingDirectory;
        [SerializeField] private RectTransform windowRectTransform;
        [Header("Prefabs")]
        [SerializeField] private MessageBox.MessageBox messageBoxPrefab;
        [SerializeField] private MessageBox.MessageBox selectBoxPrefab;

        public static string WorkingDirectory => instance.workingDirectory;

        private static Core instance;
        public void Awake() { instance = this; } 

        /// <summary>
        /// 显示一个按钮的消息窗口
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="onClose"></param>
        public static void ShowMessageBox(string title,string message,Action onClose = null)
        {
            MessageBox.MessageBox messageBox = Instantiate(instance.messageBoxPrefab, instance.windowRectTransform);
            messageBox.Initialize(title, message, onClose);
        }

        /// <summary>
        /// 显示有确定和取消按钮的消息窗口
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="onApply"></param>
        /// <param name="onCancel"></param>
        public static void ShowSelectBox(string title, string message, Action onApply,Action onCancel = null)
        {
            MessageBox.MessageBox messageBox = Instantiate(instance.selectBoxPrefab, instance.windowRectTransform);
            messageBox.Initialize(title, message, onApply, onCancel);
        }
    }
}