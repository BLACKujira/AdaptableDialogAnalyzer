using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class Pixiv_SearchResponseIntegrityCheck : TaskWindow
    {
        [Header("Settings")]
        public string sampleFolder;
        public string saveLogFile;

        List<string> messages = new List<string>();

        private void Start()
        {
            string[] tagFolders = Directory.GetDirectories(sampleFolder);
            foreach (var folder in tagFolders)
            {
                Checker checker = new Checker(folder, this);
                checker.Check();
            }

            Priority = 1;
            Progress = $"���";

            if(!string.IsNullOrEmpty(saveLogFile))
            {
                string messageText = string.Join("\n", messages);
                File.WriteAllText(saveLogFile, messageText);
                Debug.Log($"��־�ѱ��浽{saveLogFile}");
            }
        }

        public class Checker
        {
            Pixiv_SearchResponseIntegrityCheck owner;
            string targetTagFolder;
            string tag;



            public Checker(string targetTagFolder, Pixiv_SearchResponseIntegrityCheck owner)
            {
                this.owner = owner;
                this.targetTagFolder = targetTagFolder;
                tag = Path.GetFileName(targetTagFolder);
            }

            public void Check()
            {
                string illustFolder = Path.Combine(targetTagFolder, "illust");
                string mangaFolder = Path.Combine(targetTagFolder, "manga");
                string novelFolder = Path.Combine(targetTagFolder, "novel");

                Check<IllustrationsRoot>(illustFolder);
                Check<MangaRoot>(mangaFolder);
                Check<NovelsRoot>(novelFolder);
            }

            int GetLastPage(object obj)
            {
                if(obj is IllustrationsRoot illustrationsRoot)
                {
                    return illustrationsRoot.body.illust.lastPage;
                }
                else if(obj is MangaRoot mangaRoot)
                {
                    return mangaRoot.body.manga.lastPage;
                }
                else if(obj is NovelsRoot novelsRoot)
                {
                    return novelsRoot.body.novel.lastPage;
                }
                else
                {
                    throw new System.Exception("δ֪����");
                }
            }

            bool IsResponseEmpty(object obj)
            {
                if (obj is IllustrationsRoot illustrationsRoot)
                {
                    return illustrationsRoot.body.illust.data.Count == 0;
                }
                else if (obj is MangaRoot mangaRoot)
                {
                    return mangaRoot.body.manga.data.Count == 0;
                }
                else if (obj is NovelsRoot novelsRoot)
                {
                    return novelsRoot.body.novel.data.Count == 0;
                }
                else
                {
                    throw new System.Exception("δ֪����");
                }
            }

            int GetTotal(object obj)
            {
                if (obj is IllustrationsRoot illustrationsRoot)
                {
                    return illustrationsRoot.body.illust.total;
                }
                else if (obj is MangaRoot mangaRoot)
                {
                    return mangaRoot.body.manga.total;
                }
                else if (obj is NovelsRoot novelsRoot)
                {
                    return novelsRoot.body.novel.total;
                }
                else
                {
                    throw new System.Exception("δ֪����");
                }
            }

            bool GetError(object obj)
            {
                if (obj is IllustrationsRoot illustrationsRoot)
                {
                    return illustrationsRoot.error;
                }
                else if (obj is MangaRoot mangaRoot)
                {
                    return mangaRoot.error;
                }
                else if (obj is NovelsRoot novelsRoot)
                {
                    return novelsRoot.error;
                }
                else
                {
                    throw new System.Exception("δ֪����");
                }
            }

            void Check<T>(string folder)
            {
                string type = Path.GetFileName(folder);

                int lastPage = -1;
                int maxLastPage = -1;

                int lastTotal = -1;
                int minTotal = -1;
                int maxTotal = -1;

                string[] files = Directory.GetFiles(folder);
                Dictionary<int, T> rootDic = new Dictionary<int, T>();
                foreach (var file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (!int.TryParse(fileName, out int index))
                    {
                        if (fileName.StartsWith("d_")) 
                        {
                            owner.Log($"tag[{tag}] type[{type}]: �ݲ�֧���жϵ���");
                        }
                        owner.Log($"tag[{tag}] type[{type}]: �ļ���{fileName}��������");
                        continue;
                    }

                    string json = File.ReadAllText(file);
                    T root = JsonUtility.FromJson<T>(json);
                    rootDic[index] = root;
                }

                var roots = rootDic
                    .OrderBy(kvp => kvp.Key)
                    .ToArray();


                foreach (var kvp in roots)
                {
                    T root = kvp.Value;
                    string fileName = kvp.Key.ToString();

                    if(IsResponseEmpty(root))
                    {
                        owner.Log($"tag[{tag}] type[{type}]: �ļ�{fileName}��ӦΪ��");
                    }

                    int currentLastPage = GetLastPage(root);

                    // �ж��Ƿ��д���
                    bool error = GetError(root);                    
                    if(error)
                    {
                        owner.Log($"tag[{tag}] type[{type}]: �ļ�{fileName}��Ӧ�д���");
                    }

                    // �ж�lastpage�Ƿ����˱��
                    if (lastPage != -1 && lastPage != currentLastPage)
                    {
                        owner.Log($"tag[{tag}] type[{type}]: lastPage���ļ�{fileName}�����˱����{lastPage}->{currentLastPage}");
                    }

                    // ��¼��ǰҳ�������ҳ��
                    if(currentLastPage > maxLastPage)
                    {
                        maxLastPage = currentLastPage;
                    }
                    lastPage = currentLastPage;

                    int currentTotal = GetTotal(root);

                    // ���total�Ƿ�����
                    if(lastTotal != -1 &&  currentTotal < lastTotal)
                    {
                        owner.Log($"tag[{tag}] type[{type}]: total���ļ�{fileName}�����˸�������{lastTotal}->{currentTotal}");
                    }

                    // ��¼��ǰtotal�������Сtotal
                    if(minTotal == -1 || minTotal > currentTotal)
                    {
                        minTotal = currentTotal;
                    }
                    if(maxTotal == -1 || maxTotal < currentTotal)
                    {
                        maxTotal = currentTotal;
                    }
                    lastTotal = currentTotal;
                }

                // ���maxLastPage�������ļ����Ƿ�һ��
                int maxFileName = rootDic.Max(kvp => kvp.Key);
                if (maxLastPage != maxFileName)
                {
                    owner.Log($"tag[{tag}] type[{type}]: ����lastPage�������ļ�����һ�£�{maxLastPage}->{maxFileName}");
                }
            }
        }

        void Log(string message)
        {
            Debug.Log(message);
            messages.Add(message);
        }
    }
}