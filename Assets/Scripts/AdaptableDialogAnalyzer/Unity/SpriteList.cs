﻿using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/SpriteList")]
    public class SpriteList : ScriptableObject
    {
        public List<Sprite> sprites;

        public Sprite this[int index] => sprites[index];
    }
}