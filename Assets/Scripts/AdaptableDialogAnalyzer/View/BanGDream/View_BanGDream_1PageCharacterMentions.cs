using AdaptableDialogAnalyzer.MaterialController;
using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_1PageCharacterMentions : View_BanGDream_1Page
    {
        [Header("Components")]
        public MaterialController_HDRColorTexture particleMaterial;
        [Header("Settings")]
        public IndexedHDRColorList characterColorList;

        protected override void Start()
        {
            base.Start();

            View_BanGDream_CharacterMentions characterMentions = page as View_BanGDream_CharacterMentions;
            if(characterMentions != null) 
            {
                particleMaterial.HDRColor = characterColorList[characterMentions.speakerId];
            }
            else
            {
                Debug.Log("page必须是View_BanGDream_CharacterMentions类型");
            }
        }
    }
}