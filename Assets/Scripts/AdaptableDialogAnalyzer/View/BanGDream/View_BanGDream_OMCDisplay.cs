using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Live2D2;
using AdaptableDialogAnalyzer.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_OMCDisplay : MonoBehaviour
    {
        [Header("Components")]
        public EquidistantLayoutScroll equidistantLayoutScroll;
        public SimpleMentionCountResultLoader mentionCountResultLoader;
        public SimpleL2D2MutiModelManager mutiModelManager;
        [Header("Settings")]
        public float delayTime = 3f;
        public float fadeInScrollValue = 800;
        public List<int> debugCharacterIDs = new List<int>();

        private void Start()
        {
            SimpleMentionCountResult countResult = mentionCountResultLoader.SimpleMentionCountResult;
            HackCountResult(countResult);
            List<SimpleMentionCountResultItemWithRank> sortedCountResult = countResult.GetResultWithRank().OrderByDescending(x => x.rank).ToList();

            equidistantLayoutScroll.equidistantLayoutGenerator.Generate(sortedCountResult.Count, (gobj, id) =>
            {
                View_BanGDream_OMCItem oMCItem = gobj.GetComponent<View_BanGDream_OMCItem>();
                oMCItem.Initlize(sortedCountResult[id], mutiModelManager);
                gobj.SetActive(false);
            });

            // 注册事件
            equidistantLayoutScroll.onItemEnter += (gobj) =>
            {
                gobj.SetActive(true);
                gobj.GetComponent<View_BanGDream_OMCItem>().ActiveModel();
            };

            ScrollEvent scrollEvent = new ScrollEvent(fadeInScrollValue);
            scrollEvent.callEvent += (gobj) =>
            {
                gobj.GetComponent<View_BanGDream_OMCItem>().FadeIn();
            };
            equidistantLayoutScroll.scrollItemEvents.Add(scrollEvent);

            equidistantLayoutScroll.onItemExit += (gobj) =>
            {
                gobj.GetComponent<View_BanGDream_OMCItem>().InactiveModel();
                gobj.SetActive(false);
            };

            ReverseItems();
            StartCoroutine(CoPlay());
        }

        void HackCountResult(SimpleMentionCountResult countResult)
        {
            // 将601合并到15
            countResult.items[15].count += countResult[601].count;
            countResult.items[15].serifCount += countResult[601].serifCount;

            // 移除0、601、214、201
            HashSet<SimpleMentionCountResultItem> removeItems = new HashSet<SimpleMentionCountResultItem>
            {
                countResult[0],
                countResult[601],
                countResult[214],
                countResult[201]
            };
            if (debugCharacterIDs.Count > 0)
            {
                foreach (var item in countResult.items)
                {
                    if (!debugCharacterIDs.Contains(item.characterID))
                    {
                        removeItems.Add(item);
                    }
                }
            }

            foreach (var item in removeItems)
            {
                countResult.items.Remove(item);
            }
        }

        IEnumerator CoPlay()
        {
            yield return new WaitForSeconds(delayTime);

            // 初始化已经进入范围内的item
            equidistantLayoutScroll.ForEachAlreadyEnterRect((gobj) =>
            {
                gobj.SetActive(true);
                gobj.GetComponent<View_BanGDream_OMCItem>().ActiveModel();
            });

            yield return 1;

            equidistantLayoutScroll.ForEachAlreadyEnter(fadeInScrollValue, (gobj) =>
            {
                gobj.GetComponent<View_BanGDream_OMCItem>().FadeIn();
            });

            equidistantLayoutScroll.enableScroll = true;
        }

        void ReverseItems()
        {
            foreach (var item in equidistantLayoutScroll.equidistantLayoutGenerator.Items)
            {
                item.transform.SetAsFirstSibling();
            }
        }
    }
}