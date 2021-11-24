using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace BehaviourTree.Editor
{
    public class GraphSaveUtility
    {
        private BehaviourTreeGraphView m_targetGraphView;
        private BehaviourTreeContainer m_container;

        private List<Edge> Edges => m_targetGraphView.edges.ToList();
        private List<BehaviourTreeGraphNode> Nodes => m_targetGraphView.nodes.ToList().Cast<BehaviourTreeGraphNode>().ToList();

        public static GraphSaveUtility GetInstance(BehaviourTreeGraphView targetGraphView, BehaviourTreeContainer _targetContainer)
        {
            return new GraphSaveUtility
            {
                m_targetGraphView = targetGraphView,
                m_container = _targetContainer,
            };
        }

        public void SaveGraph()
        {
            if (!Edges.Any()) return; //If no edges then we can't save anything

            var savedLinks = new List<NodeLinkData>();
            var savedNodes = new List<BehaviourTreeNodeData>();
            BehaviourTreeNodeData savedRootNode; 

            //Save Connections
            var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

            for (int i = 0; i < connectedPorts.Length; i++)
            {
                var outputNode = connectedPorts[i].output.node as BehaviourTreeGraphNode;
                var inputNode = connectedPorts[i].input.node as BehaviourTreeGraphNode;

                savedLinks.Add(new NodeLinkData()
                {
                    BaseNodeGuid = inputNode.Guid,
                    PortName = connectedPorts[i].input.portName,
                    TargetNodeGuid = outputNode.Guid,
                });
            }

            //Save Nodes
            var rootNode = Nodes.First(node => node.RootNode);
            savedRootNode = new BehaviourTreeNodeData()
            {
                Guid = rootNode.Guid,
                Position = rootNode.GetPosition().position,
                NodeType = rootNode.NodeType,
            };

            foreach (var behaviourTreeNode in Nodes.Where(node=>!node.RootNode))
            {
                var nodeData = new BehaviourTreeNodeData()
                {
                    Guid = behaviourTreeNode.Guid,
                    Position = behaviourTreeNode.GetPosition().position,
                    NodeType = behaviourTreeNode.NodeType,
                };
                switch (behaviourTreeNode.NodeType)
                {
                    case BehaviourTreeNodeType.Selector:
                        break;
                    case BehaviourTreeNodeType.Sequence:
                        break;
                    case BehaviourTreeNodeType.Leaf:
                        nodeData.LeafScript = ((LeafGraphNode)behaviourTreeNode).LeafScript;
                        break;
                    case BehaviourTreeNodeType.Repeater:
                        int repeats = 0;
                        Int32.TryParse(((RepeaterGraphNode)behaviourTreeNode).MaxNumberOfRepeats, out repeats);
                        nodeData.MaxNumberOfRepeats = repeats;
                        nodeData.RepeatType = ((RepeaterGraphNode)behaviourTreeNode).RepeatMode;
                        break;
                    default:
                        break;
                }
                savedNodes.Add(nodeData);
            }

            m_container.BehaviourTreeNodes = savedNodes;
            m_container.NodeLinks = savedLinks;
            m_container.RootNodeData = savedRootNode;

            EditorUtility.SetDirty(m_container);
        }

        public void LoadGraph()
        {
            if (m_container == null)
            {
                EditorUtility.DisplayDialog("Container Not Found", "Target Behaviour Tree Graph Container was null!", "OK");
                return;
            }

            ClearGraph();
            GenerateNodes();
            ConnectNodes();
        }

        private void ClearGraph()
        {
            foreach (var node in Nodes)
            {
                Edges.Where(x => x.output.node == node).ToList().ForEach(edge => m_targetGraphView.RemoveElement(edge));

                m_targetGraphView.RemoveElement(node);
            }
        }

        private void GenerateNodes()
        {
            GenerateEntryPointNode();

            //Generate rest of the nodes and their ports
            foreach (var nodeData in m_container.BehaviourTreeNodes)
            {
                GenerateNodeFromData(nodeData);
            }
        }

        private void GenerateEntryPointNode()
        {
            var rootNode = m_targetGraphView.GenerateEntryPointNode();

            if (m_container.RootNodeData != null)
            {
                if (string.IsNullOrWhiteSpace(m_container.RootNodeData.Guid))
                {
                    m_container.RootNodeData.Guid = Guid.NewGuid().ToString();
                }

                rootNode.Guid = m_container.RootNodeData.Guid;

                //Find relevant links
                var rootPorts = m_container.NodeLinks.Where(x => x.BaseNodeGuid == rootNode.Guid).ToList();
                //Removes the port auto generated by GenerateEntryPointNode
                m_targetGraphView.RemovePort(rootNode, rootNode.inputContainer[0].Q<Port>());

                //Adds the saved ports in order
                rootPorts.Sort((a, b) => a.PortName.CompareTo(b.PortName));
                rootPorts.ForEach(x => m_targetGraphView.AddChoicePort(rootNode, x.PortName));
            }

            m_targetGraphView.AddElement(rootNode);
        }

        private void GenerateNodeFromData(BehaviourTreeNodeData nodeData)
        {
            //We don't need to m_targetGraphView.AddElement(node); because the create methods already implement it

            switch (nodeData.NodeType)
            {
                case BehaviourTreeNodeType.Selector:

                    var tempSelectorNode = m_targetGraphView.CreateSelectorGraphNode(nodeData.Position);
                    tempSelectorNode.Guid = nodeData.Guid;

                    var selectorNodePorts = m_container.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();

                    selectorNodePorts.ForEach(x => m_targetGraphView.AddChoicePort(tempSelectorNode, x.PortName));
                    break;

                case BehaviourTreeNodeType.Sequence:

                    var tempSequenceNode = m_targetGraphView.CreateSequenceGraphNode(nodeData.Position);
                    tempSequenceNode.Guid = nodeData.Guid;

                    var sequenceNodePorts = m_container.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();

                    sequenceNodePorts.ForEach(x => m_targetGraphView.AddChoicePort(tempSequenceNode, x.PortName));
                    break;

                case BehaviourTreeNodeType.Leaf:
                    var tempLeafNode = m_targetGraphView.CreateLeafGraphNode(nodeData.Position, nodeData.LeafScript);
                    tempLeafNode.Guid = nodeData.Guid;
                    break;
                case BehaviourTreeNodeType.Repeater:
                    var tempRepeaterNode = m_targetGraphView.CreateRepeaterGraphNode(nodeData.Position, nodeData.RepeatType, nodeData.MaxNumberOfRepeats.ToString());
                    tempRepeaterNode.Guid = nodeData.Guid;
                    break;
            }
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                //Skip leaf nodes as they can't have children
                var leafNode = Nodes[i] as LeafGraphNode;
                if (leafNode != null) continue; 

                //Finds the nodes links to its children
                var connections = m_container.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].Guid).ToList();
                connections.Sort((a, b) => a.PortName.CompareTo(b.PortName)); //Needs to be sorted to keep port order consisitent

                for (int j = 0; j < connections.Count; j++)
                {
                    //Find the node associated with the connection
                    var targetNodeGuid = connections[j].TargetNodeGuid;
                    var targetNode = Nodes.First(x => x.Guid == targetNodeGuid);

                    LinkNodes(Nodes[i].inputContainer[j].Q<Port>(), (Port)targetNode.outputContainer[0]);

                    //Sets the saved position of the child node
                    targetNode.SetPosition(new Rect(m_container.BehaviourTreeNodes.First(x => x.Guid == targetNodeGuid).Position, m_targetGraphView.DefaultNodeSize));
                }
            }
        }

        private void LinkNodes(Port input, Port output)
        {
            var tempEdge = new Edge
            {
                output = output,
                input = input,
            };

            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            m_targetGraphView.Add(tempEdge);
        }
    }
}