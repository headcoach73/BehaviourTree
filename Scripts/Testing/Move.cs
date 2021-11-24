using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree.Testing
{
    public class Move : Node
    {
        [SerializeField]
        private Vector3 m_direciton;

        public override NodeStates Evaluate(Context context)
        {
            TestContext testContext = context as TestContext;
            testContext.Location += m_direciton * Time.deltaTime;
            return NodeStates.SUCCESS;
        }
    }
}


