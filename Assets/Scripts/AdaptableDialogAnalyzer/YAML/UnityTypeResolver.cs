using System;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace AdaptableDialogAnalyzer.YAML
{
    public class UnityTypeResolver<T> : INodeTypeResolver where T : class
    {
        private const string UnityTagPrefix = "tag:unity3d.com";

        public bool Resolve(NodeEvent nodeEvent, ref Type currentType)
        {
            if (nodeEvent.Tag != null && nodeEvent.Tag.Value.StartsWith(UnityTagPrefix))
            {
                currentType = typeof(T);
                return true;
            }

            return false;
        }
    }
}