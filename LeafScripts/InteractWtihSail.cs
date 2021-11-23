using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MultiplayerProject.AI;

namespace BehaviourTree
{
    [CreateAssetMenu(menuName = "BehaviourTree/LeafScripts/InteractWtihSail")]
    public class InteractWtihSail : LeafScript
    {
        public override NodeStates Evaluate(Context context)
        {
            SailingAIContext sailingAIContext = context as SailingAIContext;

            if (sailingAIContext == null)
            {
                Debug.LogWarning("Sailing AI Context was null, AttempToInteractWtihSail Failed");
                return NodeStates.FAILURE;
            }

            if (sailingAIContext.CurrentSailSpotTarget == null)
            {
                Debug.LogWarning("Sail target was null, Attempt To Interact Wtih Sail Failed");
                return NodeStates.FAILURE;
            }

            if (sailingAIContext.CurrentSailSpotTarget.sail != null && sailingAIContext.CurrentSailSpotTarget.sail.sailState != sailingAIContext.SailTargetState)
            {
                sailingAIContext.CurrentSailSpotTarget.sail.SailInteract.InteractWithSailServer(sailingAIContext.AIManager.transform.position);
            }
            return NodeStates.SUCCESS;
        }
    }
}