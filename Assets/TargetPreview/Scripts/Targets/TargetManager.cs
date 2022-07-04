using System;
using System.Collections;
using System.Collections.Generic;
using TargetPreview.Targets;
using TargetPreview.ScriptableObjects;
using TargetPreview.Scripts;
using TargetPreview.Scripts.Targets;
using UnityEngine;

public class TargetManager : MonoBehaviour, IReceiveTimeUpdates
{
    public static float Time { get; private set; }
    static readonly int GlobalTimeProperty = Shader.PropertyToID("_GlobalTime");
    
    List<Target> targets = new List<Target>(1000);
    
    Dictionary<TargetHandType, Target> lastChainHeadForHand = new Dictionary<TargetHandType, Target>();

    void Awake() =>
        TimeController.AddListener(this);

    void OnDestroy() =>
        TimeController.RemoveListener(this);

    public void AppendTarget(Target target)
    {
        targets.Add(target);
        target.parentManager = this;

        var targetBehavior = target.TargetData.behavior;
        if(targetBehavior == TargetBehavior.Chain || targetBehavior == TargetBehavior.ChainStart)
        {
            HandledChainNodes(target);
        }
        
        target.TimeUpdate(Time);
    }
    
    public void RemoveTarget(Target target) =>
        targets.Remove(target);

    void UpdateManagedTargets()
    {
        int totalTargetCount = targets.Count;
        for (var i = 0; i < totalTargetCount; i++)
        {
            if (targets[i].ShouldRender)
            {
                targets[i].TimeUpdate(Time);
                targets[i].gameObject.SetActive(true);
            }
            else if(targets[i].gameObject.activeInHierarchy)
                targets[i].gameObject.SetActive(false);
        }
    }

    public void OnTimeUpdated(float time)
    {
        Time = time;
        Shader.SetGlobalFloat(GlobalTimeProperty, Time);
        UpdateManagedTargets();
    }

    void HandledChainNodes(Target target)
    {
        switch (target.TargetData.behavior)
        {
            case TargetBehavior.ChainStart:
                lastChainHeadForHand[target.TargetData.handType] = target;
                break;
            case TargetBehavior.Chain:
                ((ChainStart)lastChainHeadForHand[target.TargetData.handType]).nodes.Add((ChainNode)target);
                break;
            default:
                break;
        }
    }
    
    
}
