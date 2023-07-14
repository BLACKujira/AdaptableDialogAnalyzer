using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.MaterialController
{
    public class MaterialControllerBase : MonoBehaviour
    {
        public bool cloneMaterial = false;

        Material material = null;
        public Material Material
        {
            get
            {
                if (material == null) Initialize();
                return material;
            }
        }

        private void Initialize()
        {
            if (TryGetComponent(out Renderer renderer))
            {
                if (cloneMaterial) renderer.material = Instantiate(renderer.material);
                material = renderer.material;
            }
            else if (TryGetComponent(out Graphic graphic))
            {
                if (cloneMaterial) graphic.material = Instantiate(graphic.material);
                material = graphic.material;
            }
            else
            {
                Debug.LogError("没有找到此物体的渲染器或图形组件");
            }
        }

        private void Awake()
        {
            if (material == null)
                Initialize();
        }
    }
}