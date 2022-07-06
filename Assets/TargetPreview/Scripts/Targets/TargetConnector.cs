using TargetPreview.ScriptableObjects;
using TargetPreview.Targets;
using UnityEngine;

namespace TargetPreview.Scripts.Targets
{
    public class TargetConnector : MonoBehaviour
    {
        public Target firstTarget;
        public Target secondTarget;
        [SerializeField] LineRenderer lineRenderer;
        Vector3[] positions = new Vector3[2];

        public void Initialize(Target firstTarget, Target secondTarget)
        {
            this.firstTarget = firstTarget;
            this.secondTarget = secondTarget;
            lineRenderer.startColor = VisualConfig.GetColorForHandType(firstTarget.TargetData.handType); 
            lineRenderer.endColor = VisualConfig.GetColorForHandType(secondTarget.TargetData.handType); 
        }

        public void UpdateConnector()
        {
            bool shouldRender = firstTarget.ShouldRender;
            lineRenderer.enabled = shouldRender;
            if(!shouldRender)
                return;
            
            positions[0] = firstTarget.transform.position;
            positions[1] = secondTarget.transform.position;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(positions);
        }
    }
}