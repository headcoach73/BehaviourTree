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
        private BehaviourTreeGraphView _graphView;
        private EditorWindow _window;

        public void Init(BehaviourTreeGraphView graphView, EditorWindow window)
        {
            _graphView = graphView;
            _window = window;
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
            var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent, context.screenMousePosition - _window.position.position);
            var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
            switch (SearchTreeEntry.userData)
            {
                case SelectorGraphNode selectorGraphNode:
                    _graphView.CreateSelectorGraphNode(localMousePosition);
                    return true;
                case SequenceGraphNode sequenceGraphNode:
                    _graphView.CreateSequenceGraphNode(localMousePosition);
                    return true;
                case LeafGraphNode leafGraphNode:
                    _graphView.CreateLeafGraphNode(localMousePosition);
                    return true;
                case RepeaterGraphNode repeaterGraphNode:
                    _graphView.CreateRepeaterGraphNode(localMousePosition);
                    return true;
                default:
                    return false;
            }
        }
    }
}



