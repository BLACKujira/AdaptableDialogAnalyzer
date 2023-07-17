using UnityEngine;

namespace AdaptableDialogAnalyzer.MaterialController
{
    public class MaterialController_CyclicTexture : MaterialControllerBase
    {
        public float startSpeedX;
        public float startSpeedY;

        public float SpeedX { get => Material.GetFloat("speedX"); set { Material.SetFloat("speedX", value); } }
        public float SpeedY { get => Material.GetFloat("speedY"); set { Material.SetFloat("speedY", value); } }

        public void Start()
        {
            SpeedX = startSpeedX;
            SpeedY = startSpeedY;
        }
    }
}