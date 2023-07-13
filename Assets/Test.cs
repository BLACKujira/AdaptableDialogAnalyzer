using AdaptableDialogAnalyzer.Live2D2;
using live2d;
using live2d.framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TextAsset motionFile;
    public TextAsset facialFile;
    public SimpleModel model;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            if (motionFile) model.PlayMotion(Live2DMotion.loadMotion(motionFile.bytes));
            if(facialFile) model.PlayFacial(L2DExpressionMotion.loadJson(facialFile.bytes));
        }
    }
}
