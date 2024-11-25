using AdaptableDialogAnalyzer.DataStructures;
using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class SimpleMentionCountResultLoader : MonoBehaviour
    {
        public string file;

        SimpleMentionCountResult simpleMentionCountResult;
        public SimpleMentionCountResult SimpleMentionCountResult
        {
            get
            {
                if (simpleMentionCountResult == null) Load();
                return simpleMentionCountResult;
            }
        }

        void Load()
        {
            string json = File.ReadAllText(file);
            simpleMentionCountResult = JsonUtility.FromJson<SimpleMentionCountResult>(json);
        }
    }
}