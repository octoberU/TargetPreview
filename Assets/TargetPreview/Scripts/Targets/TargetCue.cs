using AudicaTools;
using TargetPreview.Math;
using TargetPreview.Targets;

namespace TargetPreview.Scripts.Targets
{
    public class TargetCue : AudicaTools.Cue
    {
        public float timeMs;
        public TargetCue(int tick, float tickLength, int pitch, int velocity, GridOffset gridOffset, float zOffset, int handType, int behavior) : base(tick, tickLength, pitch, velocity, gridOffset, zOffset, handType, behavior)
        {
            timeMs = GetMsTime();
        }

        public TargetCue(int tick, int tickLength, int pitch, int velocity, GridOffset gridOffset, float zOffset, HandType handType, Behavior behavior) : base(tick, tickLength, pitch, velocity, gridOffset, zOffset, handType, behavior)
        {
            timeMs = GetMsTime();
        }
        
        public static implicit operator TargetData(TargetCue cue) =>
            new TargetData()
            {
                time = cue.GetMsTime(),
                transformData = TargetTransform.CalculateTargetTransform(cue.pitch, (cue.gridOffset.x, cue.gridOffset.y, cue.zOffset)),
                handType = (TargetHandType)cue.handType,
                behavior = (TargetBehavior)cue.behavior
            };
    }
}