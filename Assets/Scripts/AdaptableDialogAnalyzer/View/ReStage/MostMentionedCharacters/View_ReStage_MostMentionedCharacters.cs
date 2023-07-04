using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.ReStage;
using AdaptableDialogAnalyzer.UIElements;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_MostMentionedCharacters : MonoBehaviour
    {
        [Header("Components")]
        public View_ReStage_MostMentionedCharacters_Item itemMostAll;
        public View_ReStage_MostMentionedCharacters_Item itemMostDifferentUnit;
        [Header("Settings")]
        public bool passZero = true;
        public bool passSelf = true;
        public int speakerId = 1;
        public float fadeTime = 0.4f;
        public bool reverse = false;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        List<GraphicsAlphaController> alphaControllers = new List<GraphicsAlphaController>();
        List<RectTransform> rectTransforms = new List<RectTransform>();
        MentionedCountManager mentionedCountManager;

        private void Start()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            List<CharacterMentionStats> characterMentionStatsList = mentionedCountManager.GetMentionStatsList(speakerId, true, true);
            int serifCount = mentionedCountManager.CountSerif(speakerId);

            CharacterMentionStats mostAll = characterMentionStatsList
                .OrderBy(cms => -cms.Total)
                .First();

            CharacterMentionStats mostDifferentUnit = characterMentionStatsList
                .Where(cms => reverse ? ReStageHelper.IsInSameGroup(cms.SpeakerId, cms.MentionedPersonId) : !ReStageHelper.IsInSameGroup(cms.SpeakerId, cms.MentionedPersonId))
                .OrderBy(cms => -cms.Total)
                .First();

            Character charSpeaker = characterDefinition[speakerId];
            Character charMostAll = characterDefinition[mostAll.MentionedPersonId];
            Character charDifferentUnit = characterDefinition[mostDifferentUnit.MentionedPersonId];

            string colorSpeaker = ColorUtility.ToHtmlStringRGB(charSpeaker.color);
            string colorMostAll = ColorUtility.ToHtmlStringRGB(charMostAll.color);
            string colorDifferentUnit = ColorUtility.ToHtmlStringRGB(charDifferentUnit.color);

            itemMostAll.SetData(mostAll.SpeakerId, mostAll.MentionedPersonId,
                $"提及次数最多：<color=#{colorMostAll}>{charMostAll.name}</color>",
                $@"在<color=#{colorSpeaker}>{charSpeaker.Namae}</color>的{serifCount}句台词中
共提及<color=#{colorMostAll}>{charMostAll.Namae}</color>{mostAll.Total}次，占比{(float)mostAll.Total / serifCount * 100:0.0}%
平均每{serifCount / mostAll.Total:0.0}句台词提到一次<color=#{colorMostAll}>{charMostAll.Namae}</color>");

            itemMostDifferentUnit.SetData(mostDifferentUnit.SpeakerId, mostDifferentUnit.MentionedPersonId,
                $"组合{(reverse ? "内" : "外")}提及最多：<color=#{colorDifferentUnit}>{charDifferentUnit.name}</color>",
                $@"在<color=#{colorSpeaker}>{charSpeaker.Namae}</color>的{serifCount}句台词中
共提及<color=#{colorDifferentUnit}>{charDifferentUnit.Namae}</color>{mostDifferentUnit.Total}次，占比{(float)mostDifferentUnit.Total / serifCount * 100:0.0}%
平均每{serifCount / mostDifferentUnit.Total:0.0}句台词提到一次<color=#{colorDifferentUnit}>{charDifferentUnit.Namae}</color>");

            alphaControllers.Add(itemMostAll.GetComponent<GraphicsAlphaController>());
            alphaControllers.Add(itemMostDifferentUnit.GetComponent<GraphicsAlphaController>());
            rectTransforms.Add(itemMostAll.GetComponent<RectTransform>());
            rectTransforms.Add(itemMostDifferentUnit.GetComponent<RectTransform>());

            foreach (var alphaController in alphaControllers)
            {
                alphaController.Alpha = 0;
            }
            foreach (var rectTransform in rectTransforms)
            {
                rectTransform.localScale = new Vector3(0, 0, 1);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Play();
            }
        }

        void Play()
        {
            foreach (var alphaController in alphaControllers)
            {
                alphaController.DoFade(1, fadeTime);
            }
            foreach (var rectTransform in rectTransforms)
            {
                rectTransform.DOScale(Vector3.one, fadeTime);
            }
        }
    }
}