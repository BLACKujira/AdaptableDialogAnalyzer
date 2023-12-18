using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity.UIElements
{
    public abstract class AutoSortBarChart : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform barContainerTransform;
        [Header("Settings")]
        public Direction2 direction = Direction2.Horizontal;
        public int maxBarCount = 10;
        public float distance = 30;
        public float dataFramePerSec = 3;
        public float fadeOutMoveDistance = 30;
        [Header("Prefab")]
        public AutoSortBarChart_Bar barPrefab;

        /// <summary>
        /// ��һ֡λ�ã�Ŀ��λ�ã���һ֡��ʱ�䣬������һ֡��λ��
        /// </summary>
        public Func<Vector2, Vector2, float, Vector2> moveFunc = DefaultMoveFunc;

        /// <summary>
        /// ��ǰ��ͼ������ʾ����
        /// </summary>
        protected List<BarManager> activeBars = new List<BarManager>();

        float currentDataFrame = 0;
        public float CurrentDataFrame => currentDataFrame;

        /// <summary>
        /// ��������
        /// </summary>
        protected class BarManager
        {
            public AutoSortBarChart_Bar bar;
            public Func<Vector2, Vector2, float, Vector2> moveFunc;

            int id;
            public int Id => id;

            bool isRecycling = false;
            public bool IsRecycling => isRecycling;

            Vector2 targetPosition;
            public Vector2 TargetPosition
            {
                get
                {
                    return targetPosition;
                }
                set
                {
                    if (isRecycling)
                    {
                        Debug.Log("����ʱ�����ƶ�");
                        return;
                    }
                    targetPosition = value;
                }
            }

            public BarManager(AutoSortBarChart autoSortBarChart, AutoSortBarChart_Bar bar, int id)
            {
                this.bar = bar;
                this.id = id;
                moveFunc = autoSortBarChart.moveFunc;
                targetPosition = bar.barTransform.anchoredPosition;
            }

            /// <summary>
            /// ������һ֡��λ�ò��ƶ�
            /// </summary>
            public void Move(float deltaTime)
            {
                bar.barTransform.anchoredPosition = moveFunc(bar.barTransform.anchoredPosition, targetPosition, deltaTime);
            }

            /// <summary>
            /// �������Ƴ�������
            /// ע�⣺�������������Ƴ��������ڵ����������Ƴ���
            /// �ڵ���ʹ������Ҫÿ֡�ṩ�µ����ݡ�
            /// ����ʱ�����ƶ�����Ҫ�ڵ���ǰ������ʧʱ��λ�á�
            /// </summary>
            /// <param name="activeBars"></param>
            public void Recycle(List<BarManager> activeBars)
            {
                isRecycling = true;
                bar.FadeOut().OnComplete(() => activeBars.Remove(this));
            }
        }

        /// <summary>
        /// ��ȡ��dataFrame������֡������
        /// </summary>
        protected abstract List<IAutoSortBarChartData> GetDataFrame(int dataFrame);

        /// <summary>
        /// ��ȡ����֡���������ﵽ���һ������֡ʱ����ֹͣ���š�
        /// </summary>
        protected abstract int GetTotalDataFrames();

        /// <summary>
        /// ��õ�rank������Ŀ��λ��
        /// </summary>
        protected virtual Vector2 GetTargetPosition(int rank)
        {
            Vector2 targetPosition = new Vector2();
            if (direction == Direction2.Horizontal)
            {
                targetPosition.x = rank * distance;
            }
            else
            {
                targetPosition.y = rank * distance;
            }
            return targetPosition;
        }

        /// <summary>
        /// �������еĶ����ʼ����Ĭ��Ϊ��
        /// </summary>
        protected virtual void InitializeBar(AutoSortBarChart_Bar bar, IAutoSortBarChartData data)
        {

        }

        /// <summary>
        /// ��ͼ�������һ����
        /// </summary>
        protected virtual AutoSortBarChart_Bar AddBar(IAutoSortBarChartData data)
        {
            int rank = activeBars.Count;
            AutoSortBarChart_Bar bar = Instantiate(barPrefab, barContainerTransform);
            InitializeBar(bar, data);
            bar.barTransform.anchoredPosition = GetTargetPosition(rank);
            bar.canvasGroup.alpha = 0;
            bar.FadeIn();

            BarManager barManager = new BarManager(this, bar, data.Id);
            activeBars.Add(barManager);
            return bar;
        }

        /// <summary>
        /// ������֡��������Ĭ�ϰ���Value��������
        /// </summary>
        protected virtual List<IAutoSortBarChartData> GetSortedDataFrame(List<IAutoSortBarChartData> dataFrame)
        {
            return dataFrame
                .OrderByDescending(df => df.Value)
                .ToList();
        }

        /// <summary>
        /// ��ȡ��ֵ�������֡
        /// </summary>
        List<IAutoSortBarChartData> GetLerpedDataFrame(float currentDataFrame)
        {
            List<IAutoSortBarChartData> lerpedDataFrame = new List<IAutoSortBarChartData>();
            List<IAutoSortBarChartData> dataFrame1 = GetDataFrame((int)currentDataFrame);
            List<IAutoSortBarChartData> dataFrame2 = GetDataFrame((int)currentDataFrame + 1);

            float t = currentDataFrame - (int)currentDataFrame;
            foreach (var data1 in dataFrame1)
            {
                IAutoSortBarChartData data2 = dataFrame2.Where(df => df.Id == data1.Id).FirstOrDefault();
                if (data2 == null)
                {
                    Debug.Log($"��ֵ�����ڵ�{(int)currentDataFrame + 1}������֡�У��Ҳ���{data1.Id}��Ӧ������");
                    lerpedDataFrame.Add(data1);
                }
                else
                {
                    lerpedDataFrame.Add(data1.Lerp(data2, t));
                }
            }

            return lerpedDataFrame;
        }

        protected virtual void Play()
        {
            StartCoroutine(CoPlay());
        }

        protected virtual IEnumerator CoPlay()
        {
            while (true)
            {
                PlayFrame(currentDataFrame);
                currentDataFrame += Time.deltaTime * dataFramePerSec;
                yield return 1;
            }
        }

        protected virtual void PlayFrame(float currentDataFrame)
        {
            List<IAutoSortBarChartData> lerpedDataFrame;
            int totalDataFrames = GetTotalDataFrames();
            if (currentDataFrame >= totalDataFrames - 1)
            {
                lerpedDataFrame = GetDataFrame(totalDataFrames - 1);
            }
            else
            {
                lerpedDataFrame = GetLerpedDataFrame(currentDataFrame);
            }

            lerpedDataFrame = GetSortedDataFrame(lerpedDataFrame);
            float valueMax = lerpedDataFrame.Max(df => df.Value);

            // ����
            foreach (var barManager in activeBars)
            {
                if (barManager.IsRecycling)
                {
                    continue;
                }
                if (!lerpedDataFrame.Any(df => df.Id == barManager.Id) || lerpedDataFrame.FindIndex(df => df.Id == barManager.Id) >= maxBarCount)
                {
                    barManager.TargetPosition += direction == Direction2.Horizontal ? new Vector2(fadeOutMoveDistance, 0) : new Vector2(0, fadeOutMoveDistance); // ����ʱ�ƶ�
                    barManager.Recycle(activeBars); // �������Ƴ�
                }
            }

            // ����
            for (int i = 0; i < lerpedDataFrame.Count && i < maxBarCount; i++)
            {
                IAutoSortBarChartData data = lerpedDataFrame[i];
                if (!activeBars.Any(ab => !ab.IsRecycling && ab.Id == data.Id))
                {
                    AddBar(data);
                }
            }

            // ����ֵ
            foreach (var barManager in activeBars)
            {
                IAutoSortBarChartData data = lerpedDataFrame.Where(df => df.Id == barManager.Id).FirstOrDefault();
                if (data != null)
                {
                    barManager.bar.SetData(data, valueMax);
                    if (!barManager.IsRecycling)
                    {
                        barManager.TargetPosition = GetTargetPosition(lerpedDataFrame.IndexOf(data)); // ���û�л��������λ��
                    }
                }
            }

            // �ƶ�
            foreach (var barManager in activeBars)
            {
                barManager.Move(Time.deltaTime);
            }
        }

        public static Vector2 DefaultMoveFunc(Vector2 lastPosition, Vector2 targetPosition, float deltaTime)
        {
            const float LERPT = 2f;
            const float MOVE_DELTA_ADD = 150F;

            Vector2 vector2 = Vector2.Lerp(lastPosition, targetPosition, LERPT * Mathf.Min(deltaTime, 1));
            vector2 = Vector2.MoveTowards(vector2, targetPosition, MOVE_DELTA_ADD * deltaTime);

            return vector2;
        }
    }
}