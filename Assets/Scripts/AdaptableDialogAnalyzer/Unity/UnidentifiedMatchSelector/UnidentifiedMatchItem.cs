using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class UnidentifiedMatchItem : MonoBehaviour
    {
        [Header("Components")]
        public Text txtNickname;
        public Text txtCount;
        public Button button;

        public void SetData(string nickname,int count)
        {
            txtNickname.text = nickname;
            txtCount.text = count.ToString();
        }
    }
}