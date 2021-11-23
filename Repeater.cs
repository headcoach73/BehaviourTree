using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    

    public class Repeater : Node
    {
        public enum Mode { UntilFailure, UntilSuccess, MaxNumberOfTimes }

        /** The child nodes for this repeater */
        [SerializeField]
        protected Node m_node;

        [SerializeField]
        protected int numberOfRepeats;

        [SerializeField]
        protected Mode repeatType;

        /** The constructor requires a list of child nodes to be  
        * passed in*/
        public Repeater(Node node, string nodeGuid, Mode type, int maxNumberOfRepeats)
        {
            m_node = node;
            NodeGuid = nodeGuid;
            repeatType = type;
            numberOfRepeats = maxNumberOfRepeats;
        }
        /* If any of the children reports a success, the selector will 
        * immediately report a success upwards. If all children fail, 
        * it will report a failure instead.*/
        public override NodeStates Evaluate(Context context)
        {
            if (!context.CompositeNodeIndex.ContainsKey(NodeGuid))
            {
                context.CompositeNodeIndex[NodeGuid] = 0;
            }

            while (!(context.CompositeNodeIndex[NodeGuid] > numberOfRepeats && repeatType == Mode.MaxNumberOfTimes))
            {
                switch (m_node.Evaluate(context))
                {
                    case NodeStates.FAILURE:
                        if (repeatType == Mode.UntilFailure) 
                        {
                            context.CompositeNodeIndex[NodeGuid] = 0;
                            return NodeStates.FAILURE;
                        }
                        else
                        {
                            return NodeStates.RUNNING;
                        }
                    case NodeStates.SUCCESS:
                        if (repeatType == Mode.UntilSuccess)
                        {
                            context.CompositeNodeIndex[NodeGuid] = 0;
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
            context.CompositeNodeIndex[NodeGuid] = 0;
            m_nodeState = NodeStates.SUCCESS;
            return m_nodeState;
        }
    }
}