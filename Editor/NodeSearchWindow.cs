using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviourTree.Editor 
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private BehaviourTreeGraphView m_graphView;
        private EditorWindow m_window;

        public void Init(BehaviourTreeGraphView graphView, EditorWindow window)
        {
            m_graphView = graphView;
            m_window = window;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
                new SearchTreeGroupEntry(new GUIContent("Nodes"), 1),
                new SearchTreeEntry(new GUIContent("Selector Node"))
                {
                    userData = new SelectorGraphNode(), level = 2
                },
                new SearchTreeEntry(new GUIContent("Sequence Node"))
                {
                    userData = new SequenceGraphNode(), level = 2
                },
                new SearchTreeEntry(new GUIContent("Leaf Node"))
                {
                    userData = new LeafGraphNode(), level = 2
                },
                new SearchTreeEntry(new GUIContent("Repeater Node"))
                {
                    userData = new RepeaterGraphNode(), level = 2
                }
            };

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var worldMousePosition = m_window.rootVisualElement.ChangeCoordinatesTo(m_window.rootVisualElement.parent, context.screenMousePosition - m_window.position.position);
            var localMousePosition = m_graphView.contentViewContainer.WorldToLocal(worldMousePosition);
            switch (SearchTreeEntry.userData)
            {
                case SelectorGraphNode selectorGraphNode:
                    m_graphView.CreateSelectorGraphNode(localMousePosition);
                    return true;
                case SequenceGraphNode sequenceGraphNode:
                    m_graphView.CreateSequenceGraphNode(localMousePosition);
                    return true;
                case LeafGraphNode leafGraphNode:
                    m_graphView.CreateLeafGraphNode(localMousePosition);
                    return true;
                case RepeaterGraphNode repeaterGraphNode:
                    m_graphView.CreateRepeaterGraphNode(localMousePosition);
                    return true;
                default:
                    return false;
            }
        }
    }
}



