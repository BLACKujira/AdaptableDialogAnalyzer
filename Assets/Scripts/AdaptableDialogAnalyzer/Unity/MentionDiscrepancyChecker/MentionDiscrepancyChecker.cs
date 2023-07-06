using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionDiscrepancyChecker : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public EquidistantLayoutGenerator2D layoutGenerator;
        public Toggle togPassZero;

        MentionedCountManager mentionedCountManager;
        Dictionary<Vector2Int, string> dictionary;

        public void Initialize(MentionedCountManager mentionedCountManager)
        {
            this.mentionedCountManager = mentionedCountManager;
            togPassZero.onValueChanged.AddListener((_) => Refresh());

            dictionary = mentionedCountManager.GetDiscrepancyPairs(false);
            Refresh();
        }

        void Refresh()
        {
            List<KeyValuePair<Vector2Int, string>> discrepancyList = dictionary.OrderBy(kvp => kvp.Key.x).ToList();
            if (togPassZero.isOn)
            {
                discrepancyList = discrepancyList.Where(kvp => kvp.Key.x != 0 && kvp.Key.y != 0).ToList();
            }

            layoutGenerator.Generate(discrepancyList.Count, (gobj, id) =>
            {
                Vector2Int vector2Int = discrepancyList[id].Key;
                string reason = discrepancyList[id].Value;

                CharacterMentionStats statsAToB = mentionedCountManager[vector2Int.x, vector2Int.y];
                CharacterMentionStats statsBToA = mentionedCountManager[vector2Int.y, vector2Int.x];

                DiscrepancyCheckItem discrepancyCheckItem = gobj.GetComponent<DiscrepancyCheckItem>();
                discrepancyCheckItem.SetData(statsAToB, statsBToA, reason);
            });
        }
    }
}