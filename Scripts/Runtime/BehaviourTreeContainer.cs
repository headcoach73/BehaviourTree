using UnityEngine;
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

        public Node ConstructTree()
        {
            return ConstructNode(RootNodeData);
        }


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
                    node = new LeafNode(nodeData.LeafScript);
                    break;
                case BehaviourTreeNodeType.Repeater:
                    node = new Repeater(childNodes[0], nodeData.Guid, nodeData.RepeatType, nodeData.MaxNumberOfRepeats);
                    break;
                default:
                    node = new Selector(childNodes, nodeData.Guid);
                    break;
            }

            return node;
        }
    }
}