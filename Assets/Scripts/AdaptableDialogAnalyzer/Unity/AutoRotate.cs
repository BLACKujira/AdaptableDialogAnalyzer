using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class AutoRotate : MonoBehaviour
    {
        public List<Transform> transforms = new List<Transform>();
        [Header("Settings")]
        public float speed;

        private void Update()
        {
            foreach (Transform transform in transforms) 
            {
                Vector3 eulerAngles = transform.rotation.eulerAngles;
                eulerAngles.z += Time.deltaTime * speed;
                transform.rotation = Quaternion.Euler(eulerAngles);
            }
        }
    }
}