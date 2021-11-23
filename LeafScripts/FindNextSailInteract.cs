using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MultiplayerProject.AI;


namespace BehaviourTree
{
    [CreateAssetMenu(menuName = "BehaviourTree/LeafScripts/FindNextSailInteract")]
    public class FindNextSailInteract : LeafScript
    {
        public override NodeStates Evaluate(Context context)
        {
            SailingAIContext sailingAIContext = context as SailingAIContext;

            if (sailingAIContext == null)
            {
                Debug.LogWarning("Sailing AI Context was null, FindNextSailInteract Failed");
                return NodeStates.FAILURE;
            }

            sailingAIContext.CurrentSailSpotTarget = sailingAIContext.AIManager.SailingAI.CurrentShipManager.shipAIManager.FindClosestSail(sailingAIContext.AIManager, sailingAIContext.CurrentSailSpotTarget);

            if (sailingAIContext.CurrentSailSpotTarget == null)
            {
                return NodeStates.FAILURE;
            }

            sailingAIContext.SailTargetState = sailingAIContext.CurrentSailSpotTarget.desiredSailState;

            sailingAIContext.NavWaypointDestination = sailingAIContext.CurrentSailSpotTarget.navWaypoint;


            return NodeStates.SUCCESS;
        }
    }
}