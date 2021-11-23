using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MultiplayerProject.AI;

namespace BehaviourTree
{
    [CreateAssetMenu(menuName = "BehaviourTree/LeafScripts/WaitTillDestinationReached")]
    public class WaitTillDestinationReached : LeafScript
    {
        public override NodeStates Evaluate(Context context)
        {
            AIContext aiContext = context as AIContext;

            if (aiContext == null)
            {
                Debug.LogWarning("AI Context was null, WaitTillDestinationReached Failed");
                return NodeStates.FAILURE;
            }

            if (CheckFailCondition(context))
            {
                return NodeStates.FAILURE;
            }

            if (CheckSuccessCondition(context))
            {
                return NodeStates.SUCCESS;
            }

            if (aiContext.AIManager.NpcManager.AIMovementController.hasReachedDestination)
            {
                return NodeStates.SUCCESS;
            }
            else
            {
                return NodeStates.RUNNING;
            }
        }

        protected virtual bool CheckFailCondition(Context context)
        {
            return false;
        }

        protected virtual bool CheckSuccessCondition(Context context)
        {
            return false;
        }
    }
}