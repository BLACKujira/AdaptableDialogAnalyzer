using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionCountDialogueEditorOneToOne : MentionCountDialogueEditor
    {
        public Toggle togHideUnmatched;
        [Header("Settings")]
        public Color colorWhite = new Color(240, 240, 240, 255);
        public Color colorBlack = new Color32(68, 68, 102, 255);
        [Header("Prefabs")]
        public SpeechBubbleButton speechBubblePrefab;
        public Window editorOneToManyPrefab;
        public Window editorManyToManyPrefab;

        int speakerId;
        int mentionedPersonId;
        Color colorUnmatched;

        protected override SpeechBubbleButton SpeechBubblePrefab => speechBubblePrefab;

        public void Initialize(MentionedCountMatrix mentionedCountMatrix, int speakerId, int mentionedPersonId)
        {
            this.speakerId = speakerId;
            this.mentionedPersonId = mentionedPersonId;

            togHideUnmatched.isOn = hideUnmatched;
            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
                hideUnmatched = value;
                Refresh();
            });

            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            colorUnmatched = APCA.GetBlackOrWhite(GlobalConfig.CharacterDefinition[mentionedPersonId].color, colorWhite, colorBlack);

            Initialize(mentionedCountMatrix);
        }

        protected override List<BasicTalkSnippet> FilterTalkSnippets(List<BasicTalkSnippet> talkSnippets)
        {
            talkSnippets = talkSnippets.Where(ts => ts.SpeakerId == speakerId).ToList();

            if (togHideUnmatched.isOn)
            {
                talkSnippets = talkSnippets
                    .Where(ts => MentionedCountMatrix[speakerId, mentionedPersonId]?.HasSerif(ts.RefIdx) ?? false)
                    .ToList();
            }

            return talkSnippets;
        }

        protected override string GetTip()
        {
            return $"标记对话 | 单角色模式 | {GlobalConfig.CharacterDefinition[speakerId].name} | {GlobalConfig.CharacterDefinition[mentionedPersonId].name}";
        }

        protected override void InitializeSpeechBubble(BasicTalkSnippet basicTalkSnippet, SpeechBubbleButton speechBubbleButton)
        {
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            speechBubbleButton.defaultBGColor = colorUnmatched;
            speechBubbleButton.SetData(basicTalkSnippet, false, false);

            if (MentionedCountMatrix[speakerId, mentionedPersonId].HasSerif(basicTalkSnippet.RefIdx))
            {
                speechBubbleButton.iceContent.SetIndividualColor(GlobalConfig.CharacterDefinition[mentionedPersonId].color);
            }

            speechBubbleButton.button.onClick.AddListener(() =>
            {
                if (MentionedCountMatrix[speakerId, mentionedPersonId].HasSerif(basicTalkSnippet.RefIdx))
                {
                    MentionedCountMatrix[speakerId, mentionedPersonId].RemoveMatchedDialogue(basicTalkSnippet.RefIdx);
                    speechBubbleButton.iceContent.SetIndividualColor(speechBubbleButton.defaultBGColor);
                }
                else
                {
                    MentionedCountMatrix.AddMatchedDialogue(speakerId, mentionedPersonId, basicTalkSnippet.RefIdx);
                    speechBubbleButton.iceContent.SetIndividualColor(GlobalConfig.CharacterDefinition[mentionedPersonId].color);
                }

                //标记此矩阵已被修改
                MentionedCountMatrix.HasChanged = true;
            });
        }

        public void OpenEditorOneToMany()
        {
            MentionCountDialogueEditorOneToMany editorOneToMany = window.OpenWindow<MentionCountDialogueEditorOneToMany>(editorOneToManyPrefab);
            editorOneToMany.Initialize(MentionedCountMatrix, speakerId);
        }

        public void OpenEditorManyToMany()
        {
            MentionCountDialogueEditorManyToMany editorManyToMany = window.OpenWindow<MentionCountDialogueEditorManyToMany>(editorManyToManyPrefab);
            editorManyToMany.Initialize(MentionedCountMatrix);
        }
    }
}