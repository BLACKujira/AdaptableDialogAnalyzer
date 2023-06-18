using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionDiscrepancyChecker : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public EquidistantLayoutGenerator2D layoutGenerator;

        public void Initialize(MentionedCountManager mentionedCountManager)
        {
            Dictionary<Vector2Int, string> dictionary = mentionedCountManager.GetDiscrepancyPairs();
            List<KeyValuePair<Vector2Int, string>> list = dictionary.OrderBy(kvp => kvp.Key.x).ToList();

            layoutGenerator.Generate(list.Count, (gobj, id) =>
            {
                Vector2Int vector2Int = list[id].Key;
                string reason = list[id].Value;

                CharacterMentionStats statsAToB = mentionedCountManager[vector2Int.x, vector2Int.y];
                CharacterMentionStats statsBToA = mentionedCountManager[vector2Int.y, vector2Int.x];

                DiscrepancyCheckItem discrepancyCheckItem = gobj.GetComponent<DiscrepancyCheckItem>();
                discrepancyCheckItem.SetData(statsAToB, statsBToA, reason);
            });
        }
    }
}