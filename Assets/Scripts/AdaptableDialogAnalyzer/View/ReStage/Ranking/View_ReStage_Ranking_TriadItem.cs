using AdaptableDialogAnalyzer.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ReStage
{

    public class View_ReStage_Ranking_TriadItem : MonoBehaviour
    {
        [Header("Components")]
        public Image imgCharAIcon;
        public Image imgCharBIcon;
        public Text txtSum;
        public Text txtAToB;
        public Text txtBToA;
        [Header("Settings")]
        public IndexedSpriteList charIconList;

        public void SetData(int characterAId, int characterBId, string sumText, string aToBText, string bToAText)
        {
            imgCharAIcon.sprite = charIconList[characterAId];
            imgCharBIcon.sprite = charIconList[characterBId];
            txtSum.text = sumText;
            txtAToB.text = aToBText;
            txtBToA.text = bToAText;
        }
    }
}