using UnityEngine;

namespace TargetPreview.Targets
{
    public class AngledTarget : GridTarget
    {
        public override void UpdateVisuals(TargetData newData)
        {
            base.UpdateVisuals(newData);
            targetCenter.transform.localRotation =
                Quaternion.Euler(new Vector3(0, 0, newData.behavior == TargetBehavior.Horizontal ? 0 : 90));
        }
    }
}