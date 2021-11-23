using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class LeafNode : Node
    {
        public LeafNode(LeafScript _leafScript)
        {
            leafScript = _leafScript;
        }

        LeafScript leafScript;

        public override NodeStates Evaluate(Context context)
        {
            return leafScript.Evaluate(context);
        }
    }
}