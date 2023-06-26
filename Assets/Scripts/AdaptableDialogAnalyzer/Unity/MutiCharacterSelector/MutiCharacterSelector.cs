using AdaptableDialogAnalyzer.UIElements;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 选择多个角色的窗口
    /// </summary>
    public class MutiCharacterSelector : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public EquidistantLayoutGenerator2D layoutGenerator;

        List<CharacterToggle> characterToggles = new List<CharacterToggle>();
        Action<int[]> onApply;

        public int[] SelectedIds
        {
            get
            {
                List<int> indexes = new List<int>();
                for (int i = 0; i < characterToggles.Count; i++)
                {
                    CharacterToggle toggle = characterToggles[i];
                    if (toggle != null && toggle.toggle.isOn) indexes.Add(i);
                }
                return indexes.ToArray();
            }
        }

        private void Awake()
        {
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            layoutGenerator.Generate(characters.Length, (gobj, id) =>
            {
                CharacterToggle characterToggle = gobj.GetComponent<CharacterToggle>();
                characterToggle.SetData(characters[id].id);
                characterToggles.Add(characterToggle);
            });
        }

        public void Initialize(int[] selectedCharacterIds, Action<int[]> onApply)
        {
            this.onApply = onApply;

            HashSet<int> charIdSet = new HashSet<int>(selectedCharacterIds);
            for (int i = 0; i < characterToggles.Count; i++)
            {
                CharacterToggle characterToggle = characterToggles[i];
                if (characterToggle == null) continue;
                characterToggle.toggle.isOn = charIdSet.Contains(i);
            }
        }

        public void Apply()
        {
            onApply(SelectedIds);
            window.Close();
        }
    }
}