using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public abstract class LeafScript : ScriptableObject
    {
        /* Implementing classes use this method to evaluate the desired set of conditions */
        public abstract NodeStates Evaluate(Context context);

        /* The current state of the node */
        protected NodeStates m_nodeState;

        public string description;
    }
}