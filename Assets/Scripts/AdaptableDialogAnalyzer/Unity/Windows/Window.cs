using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(Canvas))]
    public class Window : MonoBehaviour
    {
        public MonoBehaviour controlScript;
        public bool mainCameraRender = false;

        public ParentWindowEffect parentWindowEffect = ParentWindowEffect.Hide;
        public ParentWindowRaycast parentWindowRaycast = ParentWindowRaycast.Disable;

        public enum ParentWindowEffect { None, Hide }
        public enum ParentWindowRaycast { None, Disable }

        public UnityEvent OnClose;
        public UnityEvent OnShow;
        public UnityEvent OnHide;
        public UnityEvent OnReShow;

        public Window parentWindow { get; private set; } = null;

        Stack<Window> referenceWindows;

        GraphicRaycaster graphicRaycaster;
        Canvas canvas;

        public GraphicRaycaster GraphicRaycaster { get { if (!graphicRaycaster) graphicRaycaster = GetComponent<GraphicRaycaster>(); return graphicRaycaster; } }
        public Canvas Canvas { get { if (!canvas) canvas = GetComponent<Canvas>(); return canvas; } }

        /// <summary>
        /// 初始化窗口
        /// </summary>
        /// <param name="parentWindow"></param>
        public virtual void Initialize(Window parentWindow)
        {
            this.parentWindow = parentWindow;
            Canvas.sortingOrder = parentWindow.Canvas.sortingOrder + 1;
        }

        /// <summary>
        /// 显示窗口，并隐藏/关闭射线检测上级窗口
        /// </summary>
        public virtual void Show()
        {
            if (parentWindow)
            {
                switch (parentWindowEffect)
                {
                    case ParentWindowEffect.None:
                        break;
                    case ParentWindowEffect.Hide:
                        parentWindow.Hide();
                        break;
                    default:
                        break;
                }
                switch (parentWindowRaycast)
                {
                    case ParentWindowRaycast.None:
                        break;
                    case ParentWindowRaycast.Disable:
                        parentWindow.GraphicRaycaster.enabled = false;
                        break;
                    default:
                        break;
                }
            }
            gameObject.SetActive(true);
            OnShow.Invoke();
        }

        /// <summary>
        /// 隐藏窗口，但不关闭
        /// </summary>
        public virtual void Hide()
        {
            OnHide.Invoke();
            if (parentWindowEffect == ParentWindowEffect.None) parentWindow.Hide();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 重新显示隐藏的窗口
        /// </summary>
        public virtual void ReShow()
        {
            //windowController.currentWindow = this;
            if (parentWindowEffect == ParentWindowEffect.None) parentWindow.ReShow();
            gameObject.SetActive(true);
            OnReShow.Invoke();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public virtual void Close()
        {
            if (parentWindow)
            {
                switch (parentWindowEffect)
                {
                    case ParentWindowEffect.None:
                        break;
                    case ParentWindowEffect.Hide:
                        parentWindow.ReShow();
                        break;
                    default:
                        break;
                }
                switch (parentWindowRaycast)
                {
                    case ParentWindowRaycast.None:
                        break;
                    case ParentWindowRaycast.Disable:
                        parentWindow.GraphicRaycaster.enabled = true;
                        break;
                    default:
                        break;
                }
            }
            OnClose.Invoke();
            Destroy(gameObject);
        }

        /// <summary>
        /// 打开一个控制脚本为T的窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T OpenWindow<T>(Window window) where T : MonoBehaviour
        {
            Window openWindow = Instantiate(window);
            T controlScript = openWindow.controlScript as T;
            if (!controlScript) throw new Exception("窗口控制脚本缺失或错误");
            openWindow.Initialize(this);
            openWindow.Show();
            return controlScript;
        }

        /// <summary>
        /// 打开一个不需要操作控制脚本的窗口
        /// </summary>
        /// <param name="window"></param>
        public MonoBehaviour OpenWindow(Window window)
        {
            Window openWindow = Instantiate(window);
            openWindow.Initialize(this);
            openWindow.Show();
            return openWindow.controlScript;
        }
    }
}