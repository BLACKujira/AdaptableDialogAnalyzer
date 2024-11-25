using System;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View
{
    public class ScrollEvent
    {
        public float scrollValue;

        public ScrollEvent(float scrollValue)
        {
            this.scrollValue = scrollValue;
        }

        public event Action<GameObject> callEvent;

        public void CallEvent(GameObject item) => callEvent?.Invoke(item);
    }
}
