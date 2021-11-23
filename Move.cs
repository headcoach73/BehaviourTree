using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Move : Node
    {
        [SerializeField]
        Vector3 direciton;

        public override NodeStates Evaluate(Context context)
        {
            TestContext testContext =  context as TestContext;
            testContext.location += direciton * Time.deltaTime;
            return NodeStates.SUCCESS;
        }
    }
}


