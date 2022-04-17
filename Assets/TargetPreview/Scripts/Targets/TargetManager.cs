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
        target.TimeUpdate(time);
    }
    
    public static void RemoveTarget(Target target) =>
        targets.Remove(target);

    static void UpdateManagedTargets()
    {
        int totalTargetCount = targets.Count;
        for (var i = 0; i < totalTargetCount; i++)
        {
            if (targets[i].ShouldRender)
            {
                targets[i].TimeUpdate(time);
                targets[i].gameObject.SetActive(true);
            }
            else if(targets[i].gameObject.activeInHierarchy)
                targets[i].gameObject.SetActive(false);
        }
            
    }
}
