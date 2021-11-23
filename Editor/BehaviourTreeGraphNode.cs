using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;
using UnityEditor.Experimental.GraphView;

namespace BehaviourTree.Editor
{
    public class BehaviourTreeGraphNode : UnityEditor.Experimental.GraphView.Node
    {
        public string GUID;

        //public BehaviourTree.Node node;

        public bool rootNode = false;

        public AllowedChildren allowedChildren;

        public BehaviourTreeNodeType nodeType;
    }

    public enum AllowedChildren {Single, Multi, None }

    public class SelectorGraphNode : BehaviourTreeGraphNode
    {
        public SelectorGraphNode()
        {
            allowedChildren = AllowedChildren.Multi;
            nodeType = BehaviourTreeNodeType.Selector;
        }
    }

    public class SequenceGraphNode : BehaviourTreeGraphNode
    {
        public SequenceGraphNode()
        {
            allowedChildren = AllowedChildren.Multi;
            nodeType = BehaviourTreeNodeType.Sequence;
        }
    }
    public class RepeaterGraphNode : BehaviourTreeGraphNode
    {
        public Repeater.Mode repeatMode;
        public string maxNumberOfRepeats;
        public RepeaterGraphNode()
        {
            allowedChildren = AllowedChildren.Single;
            nodeType = BehaviourTreeNodeType.Repeater;
        }

        public void CheckNumberOfRepeats(string inputString, TextField field) 
        {
            maxNumberOfRepeats = Regex.Replace(inputString, @"[^0-9]", "");
            field.SetValueWithoutNotify(maxNumberOfRepeats);
        }

        public void CreateEnumField(string label, Enum defaultEnum)
        {
            var enumLabel = new Label(label);
            contentContainer.Add(enumLabel);
            var enumField = new EnumField(defaultEnum);
            enumField.RegisterValueChangedCallback(evt => repeatMode = (Repeater.Mode)evt.newValue);
            contentContainer.Add(enumField);
        }
    }


    public class LeafGraphNode : BehaviourTreeGraphNode
    {
        public LeafScript leafScript;
        public LeafGraphNode()
        {
            allowedChildren = AllowedChildren.None;
            nodeType = BehaviourTreeNodeType.Leaf;
        }

        public void CreateEnumField(string label, Enum defaultEnum)
        {
            var enumLabel = new Label(label);
            contentContainer.Add(enumLabel);
            var enumField = new EnumField(defaultEnum);
            contentContainer.Add(enumField);
        }
    }
}


