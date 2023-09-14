// ActionSetData
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ActionSetData
    {
        public uint areaId;
        public uint actionSetId;
        public TimeHourMinute timeFrom;
        public TimeHourMinute timeTo;
        public List<ActionSetDetail> details;
    }
}