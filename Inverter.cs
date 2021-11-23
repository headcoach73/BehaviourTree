using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree 
{
    [System.Serializable]
    public class Inverter : Node
    {
        /* Child node to evaluate */
        [SerializeField]
        private Node m_node;

        public Node node
        {
            get { return m_node; }
        }

        /* The constructor requires the child node that this inverter decorator 
         * wraps*/
        public Inverter(Node node)
        {
            m_node = node;
        }

        /* Reports a success if the child fails and 
        * a failure if the child succeeds. Running will report 
        * as running */
        public override NodeStates Evaluate(Context context)
        {
            switch (m_node.Evaluate(context))
            {
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    return m_nodeState;
            }
            m_nodeState = NodeStates.SUCCESS;
            return m_nodeState;
        }
    }
}


