using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 编辑器中的剧情选择器的基类
    /// </summary>
    public abstract class ChapterSelector : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Text txtTip;
        public EquidistantLayoutGenerator elgTypes;
        public EquidistantLayoutGenerator elgChapters;

        Dictionary<string, List<CountMatrix>> chapters = new Dictionary<string, List<CountMatrix>>();
        string selectedType;

        List<ChapterSelector_TypeItem> typeItems = new List<ChapterSelector_TypeItem>();

        CountManager countManager;
        public CountManager CountManager => countManager;

        protected void Initialize(CountManager countManager)
        {
            this.countManager = CountManager;

            chapters = countManager.GetCountMatrixByType();

            if (chapters.Count == 0)
            {
                Debug.LogError("统计数据为空");
            }

            txtTip.text = GetTip();

            string[] types = chapters.Select(kvp => kvp.Key).ToArray();
            elgTypes.Generate(types.Length, (gobj, id) =>
            {
                string selectedType = types[id];

                ChapterSelector_TypeItem typeItem = gobj.GetComponent<ChapterSelector_TypeItem>();
                typeItem.SetData(selectedType);
                typeItem.button.onClick.AddListener(() =>
                {
                    SelectType(selectedType);
                });

                typeItems.Add(typeItem);
            });

            SelectType(types[0]);
        }

        void SelectType(string type)
        {
            selectedType = type;
            foreach (var typeItem in typeItems)
            {
                if (typeItem.TypeName.Equals(type)) typeItem.Checked = true;
                else typeItem.Checked = false;
            }
            Refresh();
        }

        /// <summary>
        /// 刷新，在编辑完毕关闭窗口时手动调用
        /// </summary>
        protected void Refresh()
        {
            elgChapters.ClearItems();
            List<CountMatrix> countMatrices = chapters[selectedType];
            countMatrices = FilterCountMatrices(countMatrices);
            elgChapters.Generate(countMatrices.Count, (gobj, id) =>
            {
                ChapterSelector_ChapterItem chapterItem = gobj.GetComponent<ChapterSelector_ChapterItem>();
                InitializeChapterItem(countMatrices[id], chapterItem);
            });
        }

        /// <summary>
        /// 根据不同类型的子类调用不同的章节对象处理方式
        /// </summary>
        protected abstract void InitializeChapterItem(CountMatrix countMatrix, ChapterSelector_ChapterItem chapterItem);

        /// <summary>
        /// 筛选显示的剧情
        /// </summary>
        protected abstract List<CountMatrix> FilterCountMatrices(List<CountMatrix> countMatrices);

        /// <summary>
        /// 左下角的提示
        /// </summary>
        protected abstract string GetTip();
    }
}