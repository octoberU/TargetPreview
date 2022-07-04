using System.Collections;
using System.Collections.Generic;
using TargetPreview.Targets;
using UnityEngine;

public class ChainStart : GridTarget
{
    public List<ChainNode> nodes = new List<ChainNode>();

    public override void TimeUpdate(float time)
    {
        base.TimeUpdate(time);
        foreach (var chainNode in nodes)
        {
            chainNode.OriginalTimeUpdate(time);
        }
    }
}