using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class Pixiv_SearchResponseMerger : TaskWindow
    {
        [Header("Settings")]
        public string saveFile;
        [Header("Adapter")]
        public Pixiv_SearchResponseLoader searchResponseLoader;

        private void Start()
        {
            searchResponseLoader.Sort();
            using(var saveFileStream = File.Create(saveFile))
            {
                Serializer.Serialize(saveFileStream, searchResponseLoader.MergedResponse);
            }
            Priority = 1;
            Progress = "Íê³É";
        }
    }
}