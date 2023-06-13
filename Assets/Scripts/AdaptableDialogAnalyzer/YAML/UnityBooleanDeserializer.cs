using System;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NodeDeserializers;

namespace AdaptableDialogAnalyzer.YAML
{
    public class UnityBooleanDeserializer : INodeDeserializer
    {
        public bool Deserialize(IParser parser, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value)
        {
            if (parser.Current is Scalar scalar && expectedType == typeof(bool))
            {
                parser.MoveNext();
                if (scalar.Value == "0")
                {
                    value = false;
                    return true;
                }
                else
                {
                    value = true;
                    return true;
                }
            }

            value = null;
            return false;
        }
    }
}