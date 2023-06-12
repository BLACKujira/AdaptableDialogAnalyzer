namespace AdaptableDialogAnalyzer
{
    /// <summary>
    /// 用于保存文件，因可能在未来版本脱离unity编辑器不建议使用ScriptableObject
    /// </summary>
    /// <typeparam name="T">序列化后返回的对象，例如string、byte[]</typeparam>
    public interface ISerializable<T>
    {
        /// <summary>
        /// 获取此对象序列化的字符串
        /// </summary>
        /// <returns></returns>
        T GetSaveData();
    }
}