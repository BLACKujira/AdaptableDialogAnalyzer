using AdaptableDialogAnalyzer.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// ����Ҫչʾ������Ϣ�Ĵ�������
    /// </summary>
    public abstract class TaskWindow : MonoBehaviour
    {
        [Header("Components")]
        public ProgressBar progressBar;
        public Text txtProgress;

        private float priority = 0;
        private string progress = "������";

        protected float Priority { get => priority; set => priority = value; }
        protected string Progress { get => progress; set => progress = value; }

        private void Update()
        {
            progressBar.Priority = Priority;
            txtProgress.text = Progress;
        }
    }
}