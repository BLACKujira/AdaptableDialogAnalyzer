using UnityEngine;

namespace AdaptableDialogAnalyzer.MaterialController
{
    public class MaterialController_HDRLightTexture : MaterialControllerBase
    {
        public float startLightOffset = 0;
        public float startSaturationOffset = 0;

        public float LightOffset { get => Material.GetFloat("lightOffset"); set { Material.SetFloat("lightOffset", value); } }
        public float SaturationOffset { get => Material.GetFloat("saturationOffset"); set { Material.SetFloat("saturationOffset", value); } }

        public void Start()
        {
            if (startLightOffset != 0)
                LightOffset = startLightOffset;
            if (startSaturationOffset != 0)
                SaturationOffset = startSaturationOffset;
        }
    }
}