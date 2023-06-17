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
            List<Character> characters = GlobalConfig.CharacterDefinition.characters;
            elgSpeaker.Generate(characters.Count, (gobj, id) =>
            {
                CharacterMentionCounter_SpeakerItem speakerItem = gobj.GetComponent<CharacterMentionCounter_SpeakerItem>();
                speakerItem.SetData(id);
                speakerItem.button.onClick.AddListener(() =>
                {
                    SelectSpeaker(id);
                });
                speakerItems.Add(speakerItem);
            });

            SelectSpeaker(0);
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

            GlobalColor.SetThemeColor(GlobalConfig.CharacterDefinition.characters[characterId].color);
            Refresh();
        }

        private void Refresh()
        {
            elgMentionedPerson.ClearItems();

            int countTotal = 0;

            List<Character> characters = GlobalConfig.CharacterDefinition.characters;
            elgMentionedPerson.Generate(characters.Count, (gobj, id) =>
            {
                MentionCountEditor_MentionedPersonItem mentionedPersonItem = gobj.GetComponent<MentionCountEditor_MentionedPersonItem>();
                mentionedPersonItem.SetData(id);

                int total = mentionedCountManager[selectedCharacterId, id].Total;
                mentionedPersonItem.Count = total;
                countTotal += total;

                mentionedPersonItem.button.onClick.AddListener(() =>
                {
                    ChapterSelectorOneToOne chapterSelectorOneToOne = window.OpenWindow<ChapterSelectorOneToOne>(chapterSelectorOneToOnePrefab);
                    chapterSelectorOneToOne.Initialize(mentionedCountManager, selectedCharacterId, id);
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
    }
}