using AdaptableDialogAnalyzer.Live2D2;
using live2d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TextAsset motionFile;
    public SimpleModel model;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            model.PlayMotion(Live2DMotion.loadMotion(motionFile.bytes));
        }
    }
}
