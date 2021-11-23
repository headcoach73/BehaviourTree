using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MultiplayerProject.AI;


namespace BehaviourTree
{
    [CreateAssetMenu(menuName = "BehaviourTree/LeafScripts/CheckTargetSailState")]
    public class CheckTargetSailState : WaitTillDestinationReached
    {
        protected override bool CheckSuccessCondition(Context context)
        {
            SailingAIContext sailingAIContext = context as SailingAIContext;

            if (sailingAIContext == null)
            {
                Debug.LogWarning("Sailing AI Context was null, CheckTargetSailState Failed");
                return true;
            }

            if (sailingAIContext.CurrentSailSpotTarget == null)
            {
                Debug.LogWarning("Sailing AI CurrentSailTarget was null, CheckTargetSailState Failed");
                return true;
            }


            if (sailingAIContext.CurrentSailSpotTarget.sail == null || sailingAIContext.CurrentSailSpotTarget.sail.sailState == sailingAIContext.SailTargetState)
            {
                return true;
            }

            return false;
        }
    }
}