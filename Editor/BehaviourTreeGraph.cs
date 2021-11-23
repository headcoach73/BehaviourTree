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
        private BehaviourTreeGraphView _graphView;
        private BehaviourTreeContainer treeContainer;

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
            window.treeContainer = _treeContainer;
            window.LoadGraph();

        }

        private void OnEnable()
        {
            if (treeContainer != null)
            {
                LoadGraph();

            }
        }

        private void LoadGraph()
        {
            titleContent = new GUIContent($"{treeContainer.name} - Behaviour Tree Graph");
            ConstructGraph();
            RequestDataOperation(false);
        }

        private void ConstructGraph()
        {
            ConstructGraphView();
            GenerateToolbar();
        }

        private void ConstructGraphView()
        {
            _graphView = new BehaviourTreeGraphView(this)
            {
                name = "Behaviour Tree Graph",
            };

            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            var fileNameLabel = new Label(treeContainer.name);
            toolbar.Add(fileNameLabel);
            toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save" });

            rootVisualElement.Add(toolbar);
        }

        private void RequestDataOperation(bool save)
        {
            if (treeContainer == null)
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
                return;
            }

            var saveUtility = GraphSaveUtility.GetInstance(_graphView, treeContainer);
            if (save)
            {
                saveUtility.SaveGraph();
            }
            else
            {
                saveUtility.LoadGraph();
            }
        }

        private void OnDisable()
        {
            if (_graphView != null) 
            {
                RequestDataOperation(true);
                rootVisualElement.Remove(_graphView);
            } 
        }
    }
}

