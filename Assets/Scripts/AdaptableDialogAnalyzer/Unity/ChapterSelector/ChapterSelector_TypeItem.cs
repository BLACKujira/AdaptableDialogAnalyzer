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

        string typeName;
        public string TypeName => typeName;

        /// <summary>
        /// 设置是否显示已选择标志
        /// </summary>
        public bool Checked
        {
            get => goCheckMark.activeSelf;
            set => goCheckMark.SetActive(value);
        }

        public void SetData(string typeName)
        {
            this.typeName = typeName;
            txtType.text = typeName;
        }
    }
}