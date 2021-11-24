using UnityEngine;


namespace BehaviourTree
{
    public abstract class LeafScript : ScriptableObject
    {
        /* The abstract method leaf scripts need to implement */
        public abstract NodeStates Evaluate(Context context);

        /* The current state of the node */
        protected NodeStates m_nodeState;

        public string Description;
    }
}