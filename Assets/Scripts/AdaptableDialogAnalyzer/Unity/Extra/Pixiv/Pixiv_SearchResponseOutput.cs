using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class Pixiv_SearchResponseOutput : TaskWindow
    {
        [Header("Adapter")]
        public Pixiv_SearchResponseLoader searchResponseLoader;

        private void Start()
        {
            foreach (var dataItem  in searchResponseLoader.MergedResponse.artworks)
            {
                Debug.Log(JsonUtility.ToJson(dataItem));
            }
            foreach (var dataItem in searchResponseLoader.MergedResponse.novels)
            {
                Debug.Log(JsonUtility.ToJson(dataItem));
            }

            Priority = 1;
            Progress = $"完成";
        }
    }
}