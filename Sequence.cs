using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    [System.Serializable]
    public class Sequence : Node
    {
        /** Children nodes that belong to this sequence */
        [SerializeField]
        private List<Node> m_nodes = new List<Node>();

        /** Must provide an initial set of children nodes to work */
        public Sequence(List<Node> nodes, string nodeGuid)
        {
            m_nodes = nodes;
            m_nodeGuid = nodeGuid;
        }

        /* If any child node returns a failure, the entire node fails. Whence all  
        * nodes return a success, the node reports a success. */
        public override NodeStates Evaluate(Context context)
        {
            if (!context.CompositeNodeIndex.ContainsKey(m_nodeGuid))
            {
                context.CompositeNodeIndex[m_nodeGuid] = 0;
            }

            while (context.CompositeNodeIndex[m_nodeGuid] < m_nodes.Count)
            {
                switch (m_nodes[context.CompositeNodeIndex[m_nodeGuid]].Evaluate(context))
                {
                    case NodeStates.FAILURE:
                        m_nodeState = NodeStates.FAILURE;
                        //If Failure reset nodes progress
                        context.CompositeNodeIndex[m_nodeGuid] = 0;
                        return m_nodeState;
                    case NodeStates.SUCCESS:
                        context.CompositeNodeIndex[m_nodeGuid]++;
                        continue;
                    case NodeStates.RUNNING:
                        m_nodeState = NodeStates.RUNNING;
                        return m_nodeState;
                    default:
                        m_nodeState = NodeStates.SUCCESS;
                        context.CompositeNodeIndex[m_nodeGuid] = 0;
                        return m_nodeState;
                }
            }

            //Reset if made all the way through
            context.CompositeNodeIndex[m_nodeGuid] = 0;
            m_nodeState = NodeStates.SUCCESS;
            return m_nodeState;
        }
    }
}

