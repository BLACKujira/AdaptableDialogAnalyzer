using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{

    public class GlobalConfig : MonoBehaviour
    {
        public CharacterDefinition characterDefinition;
        public NicknameDefinition nicknameDefinition;

        public static CharacterDefinition CharacterDefinition => Instance.characterDefinition;
        public static NicknameDefinition NicknameDefinition => Instance.nicknameDefinition;

        private static GlobalConfig instance;
        public static GlobalConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GlobalConfig>();

                    if (instance == null)
                    {
                        // 如果场景中不存在 GlobalConfig 实例，则报个错
                        Debug.LogError("不存在GlobalConfig实例，请创建一个gameobject并挂载GlobalConfig脚本");
                    }
                }

                return instance;
            }
        }
    }
}