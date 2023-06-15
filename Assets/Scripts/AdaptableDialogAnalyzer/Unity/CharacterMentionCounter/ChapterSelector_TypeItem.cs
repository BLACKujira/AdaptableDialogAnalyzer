using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class ChapterSelector_TypeItem : MonoBehaviour
    {
        [Header("Components")]
        public Button button;
        public Text txtType;
        public GameObject goCheckMark;

        public void SetData(string typeName)
        {
            txtType.text = typeName;
        }
    }
}