using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    [System.Serializable]
    public class ChapterCache
    {
        public string chapterID;
        public string chapterType;
        public string chapterTitle;
        public string extraInfo;
        public BasicTalkSnippet[] talkSnippets;

        public ChapterCache(Chapter chapter)
        {
            chapterID = chapter.ChapterID;
            chapterType = chapter.ChapterType;
            chapterTitle = chapter.ChapterTitle;
            extraInfo = chapter.ExtraInfo;
            talkSnippets = chapter.GetTalkSnippets();
        }

        public string GetSerializedData()
        {
            return JsonUtility.ToJson(this);
        }

        public static ChapterCache LoadData(string serializedData)
        {
            return JsonUtility.FromJson<ChapterCache>(serializedData);
        }
    }
}