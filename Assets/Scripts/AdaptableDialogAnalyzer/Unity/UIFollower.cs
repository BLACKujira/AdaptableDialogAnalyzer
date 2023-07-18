using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 让一个Transform与一个RectTransform在画布中的位置同步，请把此组件加在RectTransform对应的物体上
    /// </summary>
    public class UIFollower : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] RectTransform sourceRectTransform;
        [SerializeField] Transform targetTransform;
        [Header("Settings")]
        public bool onlySyncOnStart = false;
        public bool syncActive = true;

        public RectTransform SourceRectTransform
        {
            get => sourceRectTransform;
            set => sourceRectTransform = value;
        }
        public Transform TargetTransform
        {
            get => targetTransform;
            set
            {
                targetTransform = value;
                SyncActive();
                SyncPosition();
            }
        }

        private void Start()
        {
            SyncPosition();
        }

        private void LateUpdate()
        {
            if (!onlySyncOnStart) SyncPosition();
        }

        private void OnEnable()
        {
            SyncActive();
        }

        private void OnDisable()
        {
            SyncActive();
        }

        private void SyncPosition()
        {
            if (sourceRectTransform == null || targetTransform == null) return;

            Vector3 newPosition = sourceRectTransform.TransformPoint(Vector3.zero);
            newPosition.z = targetTransform.position.z; // 保持目标对象的 z 位置不变

            targetTransform.position = newPosition;
        }

        private void SyncActive()
        {
            if (syncActive && targetTransform && targetTransform.gameObject.activeSelf != sourceRectTransform.gameObject.activeInHierarchy)
            {
                targetTransform.gameObject.SetActive(sourceRectTransform.gameObject.activeInHierarchy);
            }
        }
    }
}