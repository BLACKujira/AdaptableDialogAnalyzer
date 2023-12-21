using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_MobPostRank_Item : MonoBehaviour
    {
        [Header("Components")]
        public Text txtRank;
        public Text txtName;
        public Text txtCount;
        public IndividualColorElement iceColor;
        [Header("Settings")]
        public CharacterDefinition mobDefinition;

        public void SetMobId(int rank, int mobId, int count)
        {
            var character = mobDefinition[mobId];
            txtRank.text = rank.ToString();
            txtName.text = character.name;
            txtCount.text = count.ToString();
            iceColor.SetIndividualColor(character.color);
        }
    }
}
