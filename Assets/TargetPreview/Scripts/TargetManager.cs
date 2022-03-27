using System;
using System.Collections;
using System.Collections.Generic;
using TargetPreview.Models;
using TargetPreview.ScriptableObjects;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static float Time
    {
        get => time;
        set
        {
            time = value;
            Shader.SetGlobalFloat(globalTimeProperty, (float)Time);
        }
    }

    static float time;
    static int globalTimeProperty = Shader.PropertyToID("_GlobalTime");
}
