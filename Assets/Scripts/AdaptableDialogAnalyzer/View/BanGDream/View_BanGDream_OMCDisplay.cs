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
        [Header("Setting")]
        public float delayTime = 3f;
        public float fadeInScrollValue = 800;

        private void Start()
        {
            SimpleMentionCountResult countResult = mentionCountResultLoader.SimpleMentionCountResult;
            HackCountResult(countResult);
            List<SimpleMentionCountResultItemWithRank> sortedCountResult = countResult.GetResultWithRank().OrderByDescending(x => x.rank).ToList();

            try
            {
                equidistantLayoutScroll.equidistantLayoutGenerator.Generate(sortedCountResult.Count, (gobj, id) =>
                {
                    View_BanGDream_OMCItem oMCItem = gobj.GetComponent<View_BanGDream_OMCItem>();
                    oMCItem.Initlize(sortedCountResult[id], mutiModelManager);
                });
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

            // 注册事件
            equidistantLayoutScroll.onItemEnter += (gobj) =>
            {
                Debug.Log("onItemEnter");
                gobj.GetComponent<View_BanGDream_OMCItem>().ActiveModel();
            };

            ScrollEvent scrollEvent = new ScrollEvent(fadeInScrollValue);
            scrollEvent.callEvent += (gobj) =>
            {
                Debug.Log("callEvent");
                gobj.GetComponent<View_BanGDream_OMCItem>().FadeIn();
            };
            equidistantLayoutScroll.scrollEvents.Add(scrollEvent);

            equidistantLayoutScroll.onItemExit += (gobj) =>
            {
                Debug.Log("onItemExit");
                gobj.GetComponent<View_BanGDream_OMCItem>().InactiveModel();
            };

            StartCoroutine(CoPlay());
        }

        void HackCountResult(SimpleMentionCountResult countResult)
        {
            // 将601合并到15
            countResult.items[15].count += countResult[601].count;
            countResult.items[15].serifCount += countResult[601].serifCount;

            // 移除0、601、214、201
            List<SimpleMentionCountResultItem> removeItems = new List<SimpleMentionCountResultItem>
            {
                countResult[0],
                countResult[601],
                countResult[214],
                countResult[201]
            };
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
                Debug.Log("onItemEnter");
                gobj.GetComponent<View_BanGDream_OMCItem>().ActiveModel();
            });

            yield return 1;

            equidistantLayoutScroll.ForEachAlreadyEnter(fadeInScrollValue, (gobj) =>
            {
                Debug.Log("callEvent");
                gobj.GetComponent<View_BanGDream_OMCItem>().FadeIn();
            });

            equidistantLayoutScroll.enableScroll = true;
        }
    }
}