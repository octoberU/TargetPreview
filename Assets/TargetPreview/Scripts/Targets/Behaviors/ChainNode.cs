using TargetPreview.Targets;

public class ChainNode : GridTarget
{
    public override void TimeUpdate(float time)
    {
        return;
    }

    public void OriginalTimeUpdate(float time) => base.TimeUpdate(time);
}