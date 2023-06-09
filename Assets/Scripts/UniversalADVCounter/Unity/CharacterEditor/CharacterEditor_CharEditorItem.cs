using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniversalADVCounter.Unity.CharacterEditor
{
    [Obsolete]
    public class CharacterEditor_CharEditorItem : MonoBehaviour
    {
        private CharacterEditor characterEditor;

        [SerializeField] private InputField if_Id;
        [SerializeField] private InputField if_Name;
        [SerializeField] private Button btn_Remove;

        /// <summary>
        /// 读取输入框中的数据并打包成Character对象
        /// </summary>
        public Character Character
        {
            get
            {
                int charId;
                try
                {
                    charId = int.Parse(If_Id.text);
                }
                catch
                {
                    throw new IllegalCharacterIdException();
                }
                if(charId<0)
                    throw new IllegalCharacterIdException();
                return new Character(charId, If_Name.text);
            }
        }

        public CharacterEditor CharacterEditor => characterEditor;

        public InputField If_Id => if_Id;
        public InputField If_Name => if_Name;

        [System.Serializable]
        public class IllegalCharacterIdException : System.Exception
        {
            public IllegalCharacterIdException() { }
            public IllegalCharacterIdException(string message) : base(message) { }
            public IllegalCharacterIdException(string message, System.Exception inner) : base(message, inner) { }
            protected IllegalCharacterIdException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// 用于读取存档时载入数据，并绑定移除此物体的事件
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="charId"></param>
        /// <param name="name"></param>
        public void Initialize(CharacterEditor characterEditor,string charId,string name)
        {
            Initialize(characterEditor);
            If_Id.text = charId;
            If_Name.text = name;
        }

        /// <summary>
        /// 绑定移除此物体的事件
        /// </summary>
        /// <param name="itemId"></param>
        public void Initialize(CharacterEditor characterEditor)
        {
            this.characterEditor = characterEditor;
            btn_Remove.onClick.AddListener(() =>
            {
                characterEditor.GridGenerator.ObjectList.Remove(gameObject);
                characterEditor.GridGenerator.ResetPosition();
                Destroy(gameObject);
            });
        }

    }
}