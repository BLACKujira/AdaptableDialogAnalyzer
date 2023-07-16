using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class BanGDream_SuiteMasterLoader : MonoBehaviour
    {
        public string suiteMasterGetResponseFile;

        SuiteMasterGetResponse suiteMasterGetResponse = null;
        public SuiteMasterGetResponse SuiteMasterGetResponse
        {
            get
            {
                if (suiteMasterGetResponse == null) Initialize();
                return suiteMasterGetResponse;
            }
        }

        void Initialize()
        {
            byte[] bytes = File.ReadAllBytes(suiteMasterGetResponseFile);
            suiteMasterGetResponse = SuiteMasterGetResponse.Parser.ParseFrom(bytes);
        }
    }
}