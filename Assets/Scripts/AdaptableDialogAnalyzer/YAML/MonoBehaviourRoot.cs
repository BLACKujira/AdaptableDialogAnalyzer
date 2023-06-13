using AdaptableDialogAnalyzer.Games.ReStage;

namespace AdaptableDialogAnalyzer.YAML
{
    [System.Serializable]
    public class MonoBehaviourRoot<T> where T : MonoBehaviour
    {
        public T MonoBehaviour;
    }

    [System.Serializable]
    public class MonoBehaviour
    {
        public int m_ObjectHideFlags;
        public FileID m_CorrespondingSourceObject;
        public FileID m_PrefabInstance;
        public FileID m_PrefabAsset;
        public FileID m_GameObject;
        public int m_Enabled;
        public int m_EditorHideFlags;
        public FileID m_Script;
        public string m_Name;
        public string m_EditorClassIdentifier;
    }

    [System.Serializable]
    public class FileID
    {
        public int fileID;
        public string guid;
        public int type;
    }
}