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
        private BehaviourTreeGraphView _targetGraphView;
        private BehaviourTreeContainer Container;

        private List<Edge> Edges => _targetGraphView.edges.ToList();
        private List<BehaviourTreeGraphNode> Nodes => _targetGraphView.nodes.ToList().Cast<BehaviourTreeGraphNode>().ToList();

        public static GraphSaveUtility GetInstance(BehaviourTreeGraphView targetGraphView, BehaviourTreeContainer _targetContainer)
        {
            return new GraphSaveUtility
            {
                _targetGraphView = targetGraphView,
                Container = _targetContainer,
            };
        }

        public void SaveGraph()
        {
            if (!Edges.Any()) return;

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
                    BaseNodeGuid = inputNode.GUID,
                    PortName = connectedPorts[i].input.portName,
                    TargetNodeGuid = outputNode.GUID,
                });
            }

            //Save Nodes
            var rootNode = Nodes.First(node => node.rootNode);
            savedRootNode = new BehaviourTreeNodeData()
            {
                Guid = rootNode.GUID,
                Position = rootNode.GetPosition().position,
                NodeType = rootNode.nodeType,
            };

            foreach (var behaviourTreeNode in Nodes.Where(node=>!node.rootNode))
            {
                var nodeData = new BehaviourTreeNodeData()
                {
                    Guid = behaviourTreeNode.GUID,
                    Position = behaviourTreeNode.GetPosition().position,
                    NodeType = behaviourTreeNode.nodeType,
                };
                switch (behaviourTreeNode.nodeType)
                {
                    case BehaviourTreeNodeType.Selector:
                        break;
                    case BehaviourTreeNodeType.Sequence:
                        break;
                    case BehaviourTreeNodeType.Leaf:
                        nodeData.leafScript = ((LeafGraphNode)behaviourTreeNode).leafScript;
                        break;
                    case BehaviourTreeNodeType.Repeater:
                        int repeats = 0;
                        Int32.TryParse(((RepeaterGraphNode)behaviourTreeNode).maxNumberOfRepeats, out repeats);
                        nodeData.maxNumberOfRepeats = repeats;
                        nodeData.repeatType = ((RepeaterGraphNode)behaviourTreeNode).repeatMode;
                        break;
                    default:
                        break;
                }
                savedNodes.Add(nodeData);
            }

            Container.BehaviourTreeNodes = savedNodes;
            Container.NodeLinks = savedLinks;
            Container.RootNodeData = savedRootNode;

            EditorUtility.SetDirty(Container);
        }

        public void LoadGraph()
        {
            if (Container == null)
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
                Edges.Where(x => x.output.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

                _targetGraphView.RemoveElement(node);
            }
        }

        private void GenerateNodes()
        {
            var rootNode = _targetGraphView.GenerateEntryPointNode();

            if (Container.RootNodeData != null)
            {
                if (string.IsNullOrWhiteSpace(Container.RootNodeData.Guid))
                {
                    Container.RootNodeData.Guid = Guid.NewGuid().ToString();
                }

                rootNode.GUID = Container.RootNodeData.Guid;

                var rootPorts = Container.NodeLinks.Where(x => x.BaseNodeGuid == rootNode.GUID).ToList();
                _targetGraphView.RemovePort(rootNode, rootNode.inputContainer[0].Q<Port>());
                rootPorts.Sort((a, b) => a.PortName.CompareTo(b.PortName));
                rootPorts.ForEach(x => _targetGraphView.AddChoicePort(rootNode, x.PortName));
            }

            _targetGraphView.AddElement(rootNode);

            foreach (var nodeData in Container.BehaviourTreeNodes)
            {
                switch (nodeData.NodeType)
                {
                    case BehaviourTreeNodeType.Selector:

                        var tempSelectorNode = _targetGraphView.CreateSelectorGraphNode(nodeData.Position);
                        tempSelectorNode.GUID = nodeData.Guid;

                        var selectorNodePorts = Container.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();

                        selectorNodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempSelectorNode, x.PortName));
                        break;

                    case BehaviourTreeNodeType.Sequence:

                        var tempSequenceNode = _targetGraphView.CreateSequenceGraphNode(nodeData.Position);
                        tempSequenceNode.GUID = nodeData.Guid;

                        var sequenceNodePorts = Container.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();

                        sequenceNodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempSequenceNode, x.PortName));
                        break;

                    case BehaviourTreeNodeType.Leaf:
                        var tempLeafNode = _targetGraphView.CreateLeafGraphNode(nodeData.Position, nodeData.leafScript);
                        tempLeafNode.GUID = nodeData.Guid;
                        break;
                    case BehaviourTreeNodeType.Repeater:
                        var tempRepeaterNode = _targetGraphView.CreateRepeaterGraphNode(nodeData.Position, nodeData.repeatType, nodeData.maxNumberOfRepeats.ToString());
                        tempRepeaterNode.GUID = nodeData.Guid;
                        break;
                }
            }
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                var leafNode = Nodes[i] as LeafGraphNode;
                if (leafNode != null) continue;

                var connections = Container.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();
                connections.Sort((a, b) => a.PortName.CompareTo(b.PortName));

                for (int j = 0; j < connections.Count; j++)
                {
                    var targetNodeGuid = connections[j].TargetNodeGuid;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                    LinkNodes(Nodes[i].inputContainer[j].Q<Port>(), (Port)targetNode.outputContainer[0]);

                    targetNode.SetPosition(new Rect(Container.BehaviourTreeNodes.First(x => x.Guid == targetNodeGuid).Position, _targetGraphView.defaultNodeSize));
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
            _targetGraphView.Add(tempEdge);
        }
    }
}