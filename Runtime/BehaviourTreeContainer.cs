using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


namespace BehaviourTree
{
    [Serializable]
    [CreateAssetMenu(menuName ="BehaviourTree/New Behaviour Tree")]
    public class BehaviourTreeContainer : ScriptableObject
    {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<BehaviourTreeNodeData> BehaviourTreeNodes = new List<BehaviourTreeNodeData>();
        public BehaviourTreeNodeData RootNodeData;

        //private Selector RootNode;

        public Node ConstructTree()
        {
            return ConstructNode(RootNodeData);
        }

        //public NodeStates EvaluateTree(Context context)
        //{
        //    if (RootNode == null)
        //    {
        //        RootNode = ConstructTree() as Selector;

        //        if (RootNode == null)
        //        {
        //            Debug.LogWarning("Tree construction failed, root node was null");
        //            return NodeStates.FAILURE;
        //        }
        //    }

        //    return RootNode.Evaluate(context);
        //}

        private Node ConstructNode(BehaviourTreeNodeData nodeData)
        {
            var childNodes = new List<Node>();

            if (nodeData.NodeType != BehaviourTreeNodeType.Leaf)
            {
                //Find Children
                var children = NodeLinks.Where(node => node.BaseNodeGuid == nodeData.Guid).ToList();
                children.Sort((a, b) => a.PortName.CompareTo(b.PortName));

                //Construct Children
                foreach (var child in children)
                {
                    var childNode = ConstructNode(BehaviourTreeNodes.Find(node => node.Guid == child.TargetNodeGuid));
                    childNodes.Add(childNode);
                }
            }

            Node node;

            //Create Node
            switch (nodeData.NodeType)
            {
                case BehaviourTreeNodeType.Selector:
                    node = new Selector(childNodes, nodeData.Guid);
                    break;
                case BehaviourTreeNodeType.Sequence:
                    node = new Sequence(childNodes, nodeData.Guid);
                    break;
                case BehaviourTreeNodeType.Leaf:
                    node = new LeafNode(nodeData.leafScript);
                    break;
                case BehaviourTreeNodeType.Repeater:
                    node = new Repeater(childNodes[0], nodeData.Guid, nodeData.repeatType, nodeData.maxNumberOfRepeats);
                    break;
                default:
                    node = new Selector(childNodes, nodeData.Guid);
                    break;
            }

            return node;
        }
    }
}