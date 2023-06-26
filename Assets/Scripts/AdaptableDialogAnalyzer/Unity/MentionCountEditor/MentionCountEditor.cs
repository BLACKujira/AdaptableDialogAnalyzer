using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionCountEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;
        public EquidistantLayoutGenerator elgSpeaker;
        public EquidistantLayoutGenerator elgMentionedPerson;
        public Text txtCountTotal;
        public Text txtChanged;
        [Header("Prefabs")]
        public Window chapterSelectorOneToOnePrefab;
        public Window chapterSelectorOneToManyPrefab;
        public Window chapterSelectorManyToManyPrefab;
        public Window unidentifiedMatchSelectorPrefab;
        public Window discrepancyCheckerPrefab;

        MentionedCountManager mentionedCountManager;

        int selectedCharacterId = 0;
        List<CharacterMentionCounter_SpeakerItem> speakerItems = new List<CharacterMentionCounter_SpeakerItem>();

        private void Start()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;
            Initialize();
        }

        private void Initialize()
        {
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            elgSpeaker.Generate(characters.Length, (gobj, id) =>
            {
                int characterId = characters[id].id;

                CharacterMentionCounter_SpeakerItem speakerItem = gobj.GetComponent<CharacterMentionCounter_SpeakerItem>();
                speakerItem.SetData(characterId);
                speakerItem.button.onClick.AddListener(() =>
                {
                    SelectSpeaker(characterId);
                });
                speakerItems.Add(speakerItem);
            });

            SelectSpeaker(characters[0].id);
        }

        private void SelectSpeaker(int characterId)
        {
            selectedCharacterId = characterId;
            for (int i = 0; i < speakerItems.Count; i++)
            {
                CharacterMentionCounter_SpeakerItem speakerItem = speakerItems[i];
                if (i == selectedCharacterId) speakerItem.Checked = true;
                else speakerItem.Checked = false;
            }

            GlobalColor.SetThemeColor(GlobalConfig.CharacterDefinition[characterId].color);
            Refresh();
        }

        private void Refresh()
        {
            elgMentionedPerson.ClearItems();

            int countTotal = 0;

            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            elgMentionedPerson.Generate(characters.Length, (gobj, id) =>
            {
                int characterId = characters[id].id;

                MentionCountEditor_MentionedPersonItem mentionedPersonItem = gobj.GetComponent<MentionCountEditor_MentionedPersonItem>();
                mentionedPersonItem.SetData(characterId);

                int total = mentionedCountManager[selectedCharacterId, characterId].Total;
                mentionedPersonItem.Count = total;
                countTotal += total;

                mentionedPersonItem.button.onClick.AddListener(() =>
                {
                    ChapterSelectorOneToOne chapterSelectorOneToOne = window.OpenWindow<ChapterSelectorOneToOne>(chapterSelectorOneToOnePrefab);
                    chapterSelectorOneToOne.Initialize(mentionedCountManager, selectedCharacterId, characterId);
                    chapterSelectorOneToOne.window.OnClose.AddListener(() => { Refresh(); });
                });
            });

            txtCountTotal.text = $"共 {countTotal} 次，在 {mentionedCountManager.CountSerif(selectedCharacterId)} 句台词中";
            txtChanged.text = $"{mentionedCountManagerLoader.ChangedMatricesCount} 个文件已更改";
        }

        public void SaveChangedMatrices()
        {
            mentionedCountManagerLoader.SaveChangedMatrices();
            Refresh();
        }

        public void OpenSelectorOneToMany()
        {
            ChapterSelectorOneToMany chapterSelector = window.OpenWindow<ChapterSelectorOneToMany>(chapterSelectorOneToManyPrefab);
            chapterSelector.Initialize(mentionedCountManager, selectedCharacterId);
            chapterSelector.window.OnClose.AddListener(() => Refresh());
        }

        public void OpenSelectorManyToMany()
        {
            ChapterSelectorManyToMany chapterSelector = window.OpenWindow<ChapterSelectorManyToMany>(chapterSelectorManyToManyPrefab);
            chapterSelector.Initialize(mentionedCountManager);
            chapterSelector.window.OnClose.AddListener(() => Refresh());
        }

        public void OpenSelectorUnidentified()
        {
            UnidentifiedMatchSelector unidentifiedMatchSelector = window.OpenWindow<UnidentifiedMatchSelector>(unidentifiedMatchSelectorPrefab);
            unidentifiedMatchSelector.Initialize(mentionedCountManager);
        }

        public void OpenDiscrepancyChecker()
        {
            MentionDiscrepancyChecker mentionDiscrepancyChecker = window.OpenWindow<MentionDiscrepancyChecker>(discrepancyCheckerPrefab);
            mentionDiscrepancyChecker.Initialize(mentionedCountManager);
        }
    }
}