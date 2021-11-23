using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MultiplayerProject.AI;

namespace BehaviourTree
{
    [CreateAssetMenu(menuName = "BehaviourTree/LeafScripts/SetDestination")]
    public class SetDestination : LeafScript
    {
        public enum DestinationType {NavMesh, NavWaypoints, Custom}

        public DestinationType destinationType;

        public override NodeStates Evaluate(Context context)
        {
            AIContext aiContext = context as AIContext;

            if (aiContext == null)
            {
                Debug.LogWarning("AI Context was null, SetDestination Failed");
                return NodeStates.FAILURE;
            }

            switch (destinationType)
            {
                case DestinationType.NavMesh:
                    {
                        aiContext.AIManager.NpcManager.AIMovementController.SetNavMeshDestination(aiContext.NavMeshDestination);
                        break;
                    }
                case DestinationType.NavWaypoints:
                    {
                        if (aiContext.AIManager.NpcManager.AIMovementController.SetWaypointDestination(aiContext.NavWaypointDestination, true))
                        {
                            return NodeStates.SUCCESS;
                        }
                        else
                        {
                            return NodeStates.FAILURE;
                        }
                    }
                case DestinationType.Custom:
                    {
                        aiContext.AIManager.NpcManager.AIMovementController.SetCustomDestination(aiContext.CustomDestination);
                        break;
                    }
                default:
                    return NodeStates.FAILURE;
            }

            return NodeStates.SUCCESS;
        }
    }
}