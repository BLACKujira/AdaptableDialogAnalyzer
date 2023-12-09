using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using DG.Tweening;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class Pixiv_SearchResponseLoader : MonoBehaviour
    {
        /// <summary>
        /// �ļ��е���֯�ṹ
        /// </summary>
        public enum PathType
        {
            Folder_Tag_Type,
            Folder_Type,
            File_MergedResponse
        }

        [Header("Input")]
        public string path;
        public PathType folderStructureType = PathType.Folder_Tag_Type;
        [Header("Settings")]
        [Tooltip("��File_MergedResponseģʽ������")] public bool loadArtwork = true;
        [Tooltip("��File_MergedResponseģʽ������")] public bool loadNovel = true;
        public string removePostBefore;
        public string removePostAfter;
        public bool removeUseAI = false;
        public List<string> removeHasTags;

        MergedResponse mergedResponse = new MergedResponse();
        public MergedResponse MergedResponse
        {
            get
            {
                if (mergedResponse == null)
                {
                    Initialize();
                }
                return mergedResponse;
            }
        }

        #region ��ȡ�����ļ�
        /// <summary>
        /// ��ȡ�廭�������������
        /// </summary>
        ArtworksRoot LoadArtworks(string file)
        {
            string json = File.ReadAllText(file);
            return JsonUtility.FromJson<ArtworksRoot>(json);
        }

        /// <summary>
        /// ��ȡ�廭�������
        /// </summary>
        IllustrationsRoot LoadIllustrations(string file)
        {
            string json = File.ReadAllText(file);
            return JsonUtility.FromJson<IllustrationsRoot>(json);
        }

        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        MangaRoot LoadManga(string file)
        {
            string json = File.ReadAllText(file);
            return JsonUtility.FromJson<MangaRoot>(json);
        }

        /// <summary>
        /// ��ȡС˵�������
        /// </summary>
        NovelsRoot LoadNovels(string file)
        {
            string json = File.ReadAllText(file);
            return JsonUtility.FromJson<NovelsRoot>(json);
        }
        #endregion

        /// <summary>
        /// ȥ��
        /// </summary>
        void Distinct()
        {
            mergedResponse.artworks = mergedResponse.artworks
                .GroupBy(a=>a.id)
                .Select(a=>a.First())
                .ToList();

            mergedResponse.novels = mergedResponse.novels
                .GroupBy(a => a.id)
                .Select(a => a.First())
                .ToList();
        }

        /// <summary>
        /// ���б��е������ļ���Ϊĳһ���͵Ľ����ȡ����ӵ��ϲ������
        /// </summary>
        void LoadAndAdd(string[] files, ResponseType responseType)
        {
            foreach (string file in files)
            {
                List<Extra.Pixiv.SearchResponse.Artwork.DataItem> artworks = new List<Extra.Pixiv.SearchResponse.Artwork.DataItem>();
                List<Extra.Pixiv.SearchResponse.Novel.DataItem> novels = new List<Extra.Pixiv.SearchResponse.Novel.DataItem>();

                switch (responseType)
                {
                    case ResponseType.artworks:
                        artworks.AddRange(LoadArtworks(file).body.illustManga.data);
                        break;
                    case ResponseType.illustrations:
                        artworks.AddRange(LoadIllustrations(file).body.illust.data);
                        break;
                    case ResponseType.manga:
                        artworks.AddRange(LoadManga(file).body.manga.data);
                        break;
                    case ResponseType.novels:
                        novels.AddRange(LoadNovels(file).body.novel.data);
                        break;
                }

                mergedResponse.artworks.AddRange(artworks);
                mergedResponse.novels.AddRange(novels);
            }
        }

        /// <summary>
        /// ��ȡ�� ��ǩ/���� �ṹ��֯���ļ���
        /// </summary>
        void LoadFile_TagType()
        {
            string[] tagFolders = Directory.GetDirectories(path);
            foreach (var tagFolder in tagFolders)
            {
                string illustFolder = Path.Combine(tagFolder, "illust");
                string mangaFolder = Path.Combine(tagFolder, "manga");
                string novelFolder = Path.Combine(tagFolder, "novel");

                if(loadArtwork && Directory.Exists(illustFolder))
                {
                    LoadAndAdd(Directory.GetFiles(illustFolder), ResponseType.illustrations);
                }
                if (loadArtwork && Directory.Exists(mangaFolder))
                {
                    LoadAndAdd(Directory.GetFiles(mangaFolder), ResponseType.manga);
                }
                if (loadNovel && Directory.Exists(novelFolder))
                {
                    LoadAndAdd(Directory.GetFiles(novelFolder), ResponseType.novels);
                }
            }
        }

        void LoadFile_Type(string tagFolder)
        {
            string illustFolder = Path.Combine(tagFolder, "illust");
            string mangaFolder = Path.Combine(tagFolder, "manga");
            string novelFolder = Path.Combine(tagFolder, "novel");

            if (loadArtwork && Directory.Exists(illustFolder))
            {
                LoadAndAdd(Directory.GetFiles(illustFolder), ResponseType.illustrations);
            }
            if (loadArtwork && Directory.Exists(mangaFolder))
            {
                LoadAndAdd(Directory.GetFiles(mangaFolder), ResponseType.manga);
            }
            if (loadNovel && Directory.Exists(novelFolder))
            {
                LoadAndAdd(Directory.GetFiles(novelFolder), ResponseType.novels);
            }
        }

        /// <summary>
        /// ��ȡ�ϲ����ļ�
        /// </summary>
        void LoadFile_MergedResponse()
        {
            using (var fileStream = File.OpenRead(path))
            {
                mergedResponse = Serializer.Deserialize<MergedResponse>(fileStream);
            }
        }

        /// <summary>
        /// ��ȡ�ļ�
        /// </summary>
        void Initialize()
        {
            switch (folderStructureType)
            {
                case PathType.Folder_Tag_Type:
                    LoadFile_TagType();
                    break;
                case PathType.File_MergedResponse:
                    LoadFile_MergedResponse();
                    break;
                case PathType.Folder_Type:
                    LoadFile_Type(path);
                    break;
            }

            Distinct();
            ApplyFilters();
        }

        /// <summary>
        /// ��Ͷ����������
        /// </summary>
        public void Sort()
        {
            mergedResponse.artworks = MergedResponse.artworks
                .OrderBy(a => DateTime.Parse(a.createDate))
                .ToList();

            mergedResponse.novels = MergedResponse.novels
                .OrderBy(a => DateTime.Parse(a.createDate))
                .ToList();
        }

        public void ApplyFilters()
        {
            if (removeUseAI) mergedResponse.RemoveUseAI();
        }
    }
}