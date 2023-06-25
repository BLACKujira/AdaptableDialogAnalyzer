using AdaptableDialogAnalyzer.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 不需要展示过多信息的处理任务
    /// </summary>
    public abstract class TaskWindow : MonoBehaviour
    {
        [Header("Components")]
        public ProgressBar progressBar;
        public Text txtProgress;

        private float priority = 0;
        private string progress = "处理中";

        protected float Priority { get => priority; set => priority = value; }
        protected string Progress { get => progress; set => progress = value; }

        private void Update()
        {
            progressBar.Priority = Priority;
            txtProgress.text = Progress;
        }
    }
}