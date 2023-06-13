using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ReStage
{
    [System.Serializable]
    public class AdvScenario : YAML.MonoBehaviour
    {
        public string _title1;
        public string _title2;
        public List<AdvPageData> _advPages;

        public string Title1 => _title1;
        public string Title2 => _title2;
        public List<AdvPageData> Pages => _advPages;
    }
}