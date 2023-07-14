using UnityEngine;

namespace AdaptableDialogAnalyzer.MaterialController
{
    public class MaterialController_HDRColorTexture : MaterialControllerBase
    {
        [ColorUsage(true,true)]
        public Color startHDRColor;

        public Color HDRColor { get => Material.GetColor("hDRColor"); set { Material.SetColor("hDRColor", value); } }

        public void Start()
        {
            if(startHDRColor != Color.clear)
                HDRColor = startHDRColor;
        }
    }
}