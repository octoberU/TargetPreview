using System;
using System.Collections.Generic;
using System.Linq;
using TargetPreview.Display;
using TargetPreview.Targets;
using UnityEngine;

namespace TargetPreview.Scripts.Targets
{
    public class CueManager : MonoBehaviour, IReceiveTimeUpdates
    {
        [SerializeField] TargetPool targetPool;
        
        public float cueLookAheadTimeMs = 3000f;

        TargetCue[] targetCues = { };
        
        public TargetCue[] TargetCues
        {
            get => targetCues;
            set
            {
                targetCues = 
                    value
                        .OrderBy(x => x.timeMs)
                        .ThenBy(x => (int)x.behavior)
                        .ThenBy(x => (int)x.handType)
                        .ToArray(); //Ensure that cues are always sorted by time and behavior. This is extremely important when handling chains.
                OnTimeUpdated(TimeController.Time);
            }
        }
        
        List<TargetReference> activeCues = new List<TargetReference>(300);

        void Awake() =>
            TimeController.AddListener(this);

        void OnDestroy() =>
            TimeController.RemoveListener(this);

        public void OnTimeUpdated(float time)
        {
            //Clean up any active cues.
            for (var i = activeCues.Count - 1; i >= 0; i--)
            {
                if (ShouldCullCue(activeCues[i].cue))
                    RemoveActiveCue(activeCues[i]);
            }
            
            for (int i = 0; i < targetCues.Length; i++)
            {
                if(!ShouldCullCue(targetCues[i]) && !IsCueCurrentlyActive(targetCues[i]))
                {
                    AddActiveCue(targetCues[i]);
                }
            }
            
            bool WithinLookAheadTime(float cueTimeMs) =>
                Mathf.Abs(cueTimeMs - time) <= cueLookAheadTimeMs;

            bool CurrentlySustained(TargetCue cue) =>
                (time >= cue.timeMs && time <= cue.timeEndMs);

            bool ShouldCullCue(TargetCue cue) =>
                !WithinLookAheadTime(cue.timeMs) && !CurrentlySustained(cue);

            bool IsCueCurrentlyActive(TargetCue cue)
            {
                bool currentlyActive = false;
                for (var i = 0; i < activeCues.Count; i++)
                {
                    if (activeCues[i].cue == cue)
                    {
                        currentlyActive = true;
                        break;
                    }
                }

                return currentlyActive;
            }
        }

        void AddActiveCue(TargetCue cue) =>
            activeCues.Add(new TargetReference()
            {
                target = targetPool.Take(cue),
                cue = cue
            });

        void RemoveActiveCue(TargetReference reference)
        {
            targetPool.Return(reference.target);
            activeCues.Remove(reference);
        }

        struct TargetReference
        {
            public TargetCue cue;
            public Target target;
        }
    }
}