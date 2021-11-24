using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Repeater : Node
    {
        public enum Mode { UntilFailure, UntilSuccess, MaxNumberOfTimes }

        /** The child nodes for this repeater */
        protected Node m_node;

        protected int m_numberOfRepeats;

        protected Mode m_repeatType;

        /** The constructor requires a child node to be  
        * passed in*/
        public Repeater(Node node, string nodeGuid, Mode type, int maxNumberOfRepeats)
        {
            m_node = node;
            m_nodeGuid = nodeGuid;
            m_repeatType = type;
            m_numberOfRepeats = maxNumberOfRepeats;
        }
        /* This node will return running until the set condition is met
        * UntilFailure,  UntilSuccess or until a MaxNumberOfTimes */
        public override NodeStates Evaluate(Context context)
        {
            if (!context.CompositeNodeIndex.ContainsKey(m_nodeGuid))
            {
                context.CompositeNodeIndex[m_nodeGuid] = 0;
            }

            while (!(context.CompositeNodeIndex[m_nodeGuid] > m_numberOfRepeats && m_repeatType == Mode.MaxNumberOfTimes))
            {
                switch (m_node.Evaluate(context))
                {
                    case NodeStates.FAILURE:
                        if (m_repeatType == Mode.UntilFailure) 
                        {
                            context.CompositeNodeIndex[m_nodeGuid] = 0;
                            return NodeStates.FAILURE;
                        }
                        else
                        {
                            return NodeStates.RUNNING;
                        }
                    case NodeStates.SUCCESS:
                        if (m_repeatType == Mode.UntilSuccess)
                        {
                            context.CompositeNodeIndex[m_nodeGuid] = 0;
                            return NodeStates.SUCCESS;
                        }
                        else
                        {
                            return NodeStates.RUNNING;
                        }
                    case NodeStates.RUNNING:
                        m_nodeState = NodeStates.RUNNING;
                        return m_nodeState;
                    default:
                        continue;
                }
            }
            context.CompositeNodeIndex[m_nodeGuid] = 0;
            m_nodeState = NodeStates.SUCCESS;
            return m_nodeState;
        }
    }
}