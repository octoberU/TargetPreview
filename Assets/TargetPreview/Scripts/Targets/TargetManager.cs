using System;
using System.Collections;
using System.Collections.Generic;
using TargetPreview.Targets;
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
            UpdateManagedTargets();
        }
    }

    static float time;
    static int globalTimeProperty = Shader.PropertyToID("_GlobalTime");
    
    static List<Target> targets = new List<Target>(1000);
    
    public static void AppendTarget(Target target)
    {
        targets.Add(target);
        target.TimeUpdate();
    }
    
    public static void RemoveTarget(Target target) =>
        targets.Remove(target);

    static void UpdateManagedTargets()
    {
        foreach (var target in targets)
            target.TimeUpdate();
    }
}
