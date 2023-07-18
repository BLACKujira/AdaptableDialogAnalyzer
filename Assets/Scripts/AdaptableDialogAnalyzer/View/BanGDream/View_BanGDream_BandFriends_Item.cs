using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_BandFriends_Item : MonoBehaviour
    {
        [Header("Components")]
        public List<View_BanGDream_BandFriends_Item_Char> charItems;
        public View_BanGDream_LiveSDStage liveSDStage;
        public Text txtTotal;
        [Header("Settings")]
        public IndexedColorList textColorList;

        public void Initialize(Transform effectTransform) => liveSDStage.Initialize(effectTransform);

        public void SetData(int mentionedPersonId, Vector2Int[] countArray)
        {
            if (countArray.Length != charItems.Count)
            {
                Debug.Log("输入人数与图标数不匹配");
                return;
            }

            liveSDStage.SetData(mentionedPersonId);

            for (int i = 0; i < charItems.Count; i++)
            {
                Vector2Int vector2Int = countArray[i];
                View_BanGDream_BandFriends_Item_Char itemChar = charItems[i];
                itemChar.SetData(vector2Int.x, vector2Int.y);
            }

            txtTotal.text = $"共被提及 :  {countArray.Sum(v => v.y)} 次";
            txtTotal.color = textColorList[mentionedPersonId];
        }

        public void FadeIn() => liveSDStage.FadeIn();
    }
}