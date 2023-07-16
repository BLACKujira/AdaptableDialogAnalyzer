using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class BanGDream_MasterLoader : MonoBehaviour
    {
        public string suiteMasterGetResponseFile;
        public string userEventStoryMemorialResponseFile;

        SuiteMasterGetResponse suiteMasterGetResponse = null;
        public SuiteMasterGetResponse SuiteMasterGetResponse
        {
            get
            {
                if (suiteMasterGetResponse == null)
                {
                    byte[] bytes = File.ReadAllBytes(suiteMasterGetResponseFile);
                    suiteMasterGetResponse = SuiteMasterGetResponse.Parser.ParseFrom(bytes);
                }
                return suiteMasterGetResponse;
            }
        }
        
        UserEventStoryMemorialResponse userEventStoryMemorialResponse = null;
        public UserEventStoryMemorialResponse UserEventStoryMemorialResponse
        {
            get
            {
                if(userEventStoryMemorialResponse == null)
                {
                    byte[] bytes = File.ReadAllBytes(userEventStoryMemorialResponseFile);
                    userEventStoryMemorialResponse = UserEventStoryMemorialResponse.Parser.ParseFrom(bytes);
                }
                return userEventStoryMemorialResponse;
            }
        }

    }
}