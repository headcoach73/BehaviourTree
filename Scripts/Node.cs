using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree 
{
    public enum BehaviourTreeNodeType { Selector, Sequence, Leaf, Repeater }

    [System.Serializable]
    public abstract class Node
    {
        /* Delegate that returns the state of the node.*/
        public delegate NodeStates NodeReturn();

        /* The current state of the node */
        protected NodeStates m_nodeState;
        protected string m_nodeGuid;

        public NodeStates NodeState
        {
            get { return m_nodeState; }
        }

        /* The constructor for the node */
        public Node() 
        {

        }

        /* Implementing classes use this method to evaluate the desired set of conditions */
        public abstract NodeStates Evaluate(Context context);
    }

    public class Context 
    {
        public Dictionary<string, int> CompositeNodeIndex = new Dictionary<string, int>();
    }






    public enum NodeStates {SUCCESS, FAILURE, RUNNING }
}

