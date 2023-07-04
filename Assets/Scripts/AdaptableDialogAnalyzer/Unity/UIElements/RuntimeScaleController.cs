using UnityEngine;

namespace AdaptableDialogAnalyzer.UIElements
{
    /// <summary>
    /// 记录运行前的缩放，并在运行以已倍率修改
    /// </summary>
    public class RuntimeScaleController : MonoBehaviour
    {
        public Transform targetTransform;

        Vector3 startScale = Vector3.one;

        public Vector3 Scale
        {
            get { return new Vector3(targetTransform.localScale.x / startScale.x, targetTransform.localScale.y / startScale.y, targetTransform.localScale.z / startScale.z); }
            set { targetTransform.localScale = new Vector3(targetTransform.localScale.x * startScale.x, targetTransform.localScale.y * startScale.y, targetTransform.localScale.z * startScale.z); }
        }

        public float ScaleRatio
        {
            get => targetTransform.localScale.x / startScale.x;
            set => targetTransform.localScale = value * startScale;
        }

        private void Awake()
        {
            startScale = transform.localScale;
        }
    }
}