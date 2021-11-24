using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviourTree.Editor
{
    public class BehaviourTreeGraph : EditorWindow
    {
        private BehaviourTreeGraphView m_graphView;
        private BehaviourTreeContainer m_treeContainer;

        // Don't need this anymore as double clicking on the container object opens the graph, see OpenBehaviourTreeHandler.cs

        //[MenuItem("Graph/Behaviour Tree Editor")]
        //public static void OpenBehaviourTreeGraphWindow()
        //{
        //    var window = CreateWindow<BehaviourTreeGraph>();
        //    window.titleContent = new GUIContent("Behaviour Tree Graph");
        //    window.ConstructGraph();
        //}

        public static void OpenBehaviourTreeGraphWindow(BehaviourTreeContainer _treeContainer)
        {
            var window = CreateWindow<BehaviourTreeGraph>();
            window.m_treeContainer = _treeContainer;
            window.LoadGraph();

        }

        private void OnEnable()
        {
            if (m_treeContainer != null)
            {
                LoadGraph();
            }
        }

        private void LoadGraph()
        {
            titleContent = new GUIContent($"{m_treeContainer.name} - Behaviour Tree Graph");
            ConstructGraph();
            var saveUtility = GraphSaveUtility.GetInstance(m_graphView, m_treeContainer);
            saveUtility.LoadGraph();
        }

        private void ConstructGraph()
        {
            ConstructGraphView();
            GenerateToolbar();
        }

        private void ConstructGraphView()
        {
            m_graphView = new BehaviourTreeGraphView(this)
            {
                name = "Behaviour Tree Graph",
            };

            m_graphView.StretchToParentSize();
            rootVisualElement.Add(m_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            var fileNameLabel = new Label(m_treeContainer.name);
            toolbar.Add(fileNameLabel);
            toolbar.Add(new Button(() => SaveCurrentGraph()) { text = "Save" });

            rootVisualElement.Add(toolbar);
        }

        private void SaveCurrentGraph()
        {
            var saveUtility = GraphSaveUtility.GetInstance(m_graphView, m_treeContainer);
            saveUtility.SaveGraph();
        }

        private void OnDisable()
        {
            if (m_graphView != null) 
            {
                SaveCurrentGraph();
                rootVisualElement.Remove(m_graphView);
            } 
        }
    }
}

