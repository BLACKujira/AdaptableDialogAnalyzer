using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_KataomoiItem : View_BanGDream_TriadItem
    {
        public void SetData(int characterAId, int characterBId, float percentAToB, float percentBToA)
        {
            SetGraphics(characterAId, characterBId);
            txtTotal.text = $"{Mathf.Abs(percentAToB - percentBToA) * 100:00.00}%".ToString();
            txtAToB.text = $"{percentAToB * 100:00.00}%".ToString();
            txtBToA.text = $"{percentBToA * 100:00.00}%".ToString();
        }
    }
}