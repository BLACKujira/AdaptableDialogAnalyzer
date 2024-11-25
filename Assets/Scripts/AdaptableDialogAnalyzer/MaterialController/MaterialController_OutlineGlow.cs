using UnityEngine;

namespace AdaptableDialogAnalyzer.MaterialController
{
    public class MaterialController_OutlineGlow : MaterialControllerBase
    {
        public float baseTextureTexelSize = 0.01f;
        public float searchWidth = 1;
        public float outlineWidth = 1;
        [ColorUsage(true, true)]
        public Color outlineColor = new Color(2, 2, 2, 1);

        public float BaseTextureTexelSize { get => Material.GetFloat("baseTextureTexelSize"); set { Material.SetFloat("baseTextureTexelSize", value); baseTextureTexelSize = value; } }
        public float SearchWidth { get => Material.GetFloat("searchWidth"); set { Material.SetFloat("searchWidth", value); searchWidth = value; } }
        public float OutlineWidth { get => Material.GetFloat("outlineWidth"); set { Material.SetFloat("outlineWidth", value); outlineWidth = value; } }
        public Color OutlineColor { get => Material.GetColor("outlineColor"); set { Material.SetColor("outlineColor", value);   outlineColor = value; } }

        public void Start()
        {
            if (baseTextureTexelSize != 0)
                BaseTextureTexelSize = baseTextureTexelSize;
            if (searchWidth != 0)
                SearchWidth = searchWidth;
            if (outlineWidth != 0)
                OutlineWidth = outlineWidth;
            if (outlineColor != Color.clear)
                OutlineColor = outlineColor;
        }
    }
}