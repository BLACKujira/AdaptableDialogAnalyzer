using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniversalADVCounter.UIElements;
using UniversalADVCounter.Unity;

namespace UniversalADVCounter.Unity.CharacterEditor
{
    [Obsolete]
    public class CharacterEditor : MonoBehaviour
    {
        public const string SAVE_FILE_NAME = "Characters.json";

        [SerializeField] private GridGenerator gridGenerator;
        [SerializeField] private CharacterEditor_CharEditorItem charEditorItemPrefab;
        [SerializeField] private Button addItemButtonPrefab;

        public GridGenerator GridGenerator => gridGenerator;
        public string SavePath => Path.Combine(Core.WorkingDirectory, SAVE_FILE_NAME);

        private void Start()
        {
            if (File.Exists(SavePath))
                LoadData();
            else
                NewData();
        }

        /// <summary>
        /// 如果工作目录下有存档则读取
        /// </summary>
        private void LoadData()
        {
            gridGenerator.Clear();
            string saveData = File.ReadAllText(SavePath);
            Characters characters = Characters.Load(saveData);
            gridGenerator.Generate(
                charEditorItemPrefab.gameObject,
                characters.characters.Length,
                (gobj, id) =>
                {
                    CharacterEditor_CharEditorItem charEditorItem = gobj.GetComponent<CharacterEditor_CharEditorItem>();
                    charEditorItem.Initialize(this, characters[id].id.ToString(), characters[id].name);
                });
            AddButton();
        }
        /// <summary>
        /// 如果工作目录下有存档则创建
        /// </summary>
        private void NewData()
        {
            AddButton();
        }

        /// <summary>
        /// 在网格生成器最后添加一个用于添加物体的按钮
        /// </summary>
        private void AddButton()
        {
            gridGenerator.AddButton(addItemButtonPrefab.gameObject, (gobj) =>
             {
                 Button button = gobj.GetComponent<Button>();
                 button.onClick.AddListener(() => AddItem());
             });
        }

        /// <summary>
        /// 在网格生成器最后添加一个物体
        /// </summary>
        private void AddItem()
        {
            List<string[]> tempList = new List<string[]>(gridGenerator.ObjectList
                .Take(gridGenerator.ObjectList.Count-1)
                .Select((gobj) =>
                {
                    CharacterEditor_CharEditorItem charEditorItem = gobj.GetComponent<CharacterEditor_CharEditorItem>();
                    return new string[] { charEditorItem.If_Id.text, charEditorItem.If_Name.text };
                }));
            tempList.Add(new string[] { string.Empty, string.Empty });
            gridGenerator.Clear();
            gridGenerator.Generate(charEditorItemPrefab.gameObject,
                tempList.Count,
                (gobj, id) =>
                {
                    CharacterEditor_CharEditorItem charEditorItem = gobj.GetComponent<CharacterEditor_CharEditorItem>();
                    charEditorItem.Initialize(this, tempList[id][0], tempList[id][1]);
                });
            AddButton();
        }

        public void Save()
        {
            List<Character> characters;
            try
            {
                characters = new List<Character>
                    (
                        gridGenerator.ObjectList
                            .Take(gridGenerator.ObjectList.Count - 1)
                            .Select((gobj) =>
                            {
                                CharacterEditor_CharEditorItem charEditorItem = gobj.GetComponent<CharacterEditor_CharEditorItem>();
                                return charEditorItem.Character;
                            })
                    );
            }
            catch(CharacterEditor_CharEditorItem.IllegalCharacterIdException)
            {
                Core.ShowMessageBox("错误", "存在ID不合法的角色，请检查是否有角色的ID为负数、0或空值");
                return;
            }
            catch(Exception ex)
            {
                Core.ShowMessageBox("错误", ex.Message);
                return;
            }

            //检查是否有角色ID重复
            HashSet<int> usedId = new HashSet<int>();
            foreach (var character in characters)
            {
                if(usedId.Contains(character.id))
                {
                    Core.ShowMessageBox("错误", "存在ID相同的角色");
                    return;
                }
                usedId.Add(character.id);
            }

            //以ID从小到大排列
            characters.Sort((x, y) => x.id.CompareTo(y.id));

            if(File.Exists(SavePath))
            {
                Core.ShowSelectBox("保存", $"{SavePath}已存在，确定要覆盖保存吗？",()=>
                File.WriteAllText(SavePath,
                    new Characters(characters.ToArray())
                        .GetSaveData()));
            }
            else
            {
                File.WriteAllText(SavePath,
                    new Characters(characters.ToArray())
                        .GetSaveData());
            }
        }
    }
}