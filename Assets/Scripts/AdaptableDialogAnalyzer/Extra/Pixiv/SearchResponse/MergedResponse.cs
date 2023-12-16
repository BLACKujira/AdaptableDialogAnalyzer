using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse
{
    [Serializable]
    [ProtoContract]
    public class MergedResponse : MonoBehaviour
    {
        [ProtoMember(1)]
        public List<Artwork.DataItem> artworks = new List<Artwork.DataItem>();
        [ProtoMember(2)]
        public List<Novel.DataItem> novels = new List<Novel.DataItem>();

        public void RemoveUseAI()
        {
            artworks.RemoveAll(d => d.aiType == 2);
            novels.RemoveAll(d => d.aiType == 2);
        }

        public void RemoveBefore(DateTime dateTime)
        {
            artworks.RemoveAll(d => d.CreateDate < dateTime);
            novels.RemoveAll(d => d.CreateDate < dateTime);
        }

        public void RemoveAfter(DateTime dateTime)
        {
            artworks.RemoveAll(d => d.CreateDate > dateTime);
            novels.RemoveAll(d => d.CreateDate > dateTime);
        }
    }
}
