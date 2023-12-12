using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    [RequireComponent(typeof(Image))]
    public class View_ProjectSekai_TimelineTypeA_BottomLabel : View_ProjectSekai_TimelineTypeA_Label
    {
        Image image;
        public Image Image
        {
            get
            {
                if(image == null)
                {
                    image = GetComponent<Image>();
                }
                return image;
            }
        }

        public void SetData(Color color)
        {
            Image.color = color;
        }
    }
}
