using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(menuName ="BehaviourTree/LeafScripts/TestLeafScript")]
    public class TestLeafScript : LeafScript
    {
        public string Identifier;
        public NodeStates ReturnState;

        public TestLeafScript()
        {
            description = "A Leaf Script made for testing purposes";
        }

        public override NodeStates Evaluate(Context context)
        {
            Debug.Log($"Test leaf success {Identifier}");
            return ReturnState;
        }
    }
}