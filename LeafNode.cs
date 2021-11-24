using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class LeafNode : Node
    {
        /* The constructor requires a scriptable object, leaf script to evaluate */
        public LeafNode(LeafScript _leafScript)
        {
            m_leafScript = _leafScript;
        }

        private LeafScript m_leafScript;

        /* Just reports back what the abstract function on the leaf script returns */
        public override NodeStates Evaluate(Context context)
        {
            return m_leafScript.Evaluate(context);
        }
    }
}