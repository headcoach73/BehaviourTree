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
        public string Guid;

        public bool RootNode = false;

        public AllowedChildren AllowedChildren;

        public BehaviourTreeNodeType NodeType;
    }

    public enum AllowedChildren {Single, Multi, None }

    public class SelectorGraphNode : BehaviourTreeGraphNode
    {
        public SelectorGraphNode()
        {
            AllowedChildren = AllowedChildren.Multi;
            NodeType = BehaviourTreeNodeType.Selector;
        }
    }

    public class SequenceGraphNode : BehaviourTreeGraphNode
    {
        public SequenceGraphNode()
        {
            AllowedChildren = AllowedChildren.Multi;
            NodeType = BehaviourTreeNodeType.Sequence;
        }
    }
    public class RepeaterGraphNode : BehaviourTreeGraphNode
    {
        public Repeater.Mode RepeatMode;
        public string MaxNumberOfRepeats;
        public RepeaterGraphNode()
        {
            AllowedChildren = AllowedChildren.Single;
            NodeType = BehaviourTreeNodeType.Repeater;
        }

        public void CheckNumberOfRepeats(string inputString, TextField field) 
        {
            MaxNumberOfRepeats = Regex.Replace(inputString, @"[^0-9]", "");
            field.SetValueWithoutNotify(MaxNumberOfRepeats);
        }

        public void CreateEnumField(string label, Enum defaultEnum)
        {
            var enumLabel = new Label(label);
            contentContainer.Add(enumLabel);
            var enumField = new EnumField(defaultEnum);
            enumField.RegisterValueChangedCallback(evt => RepeatMode = (Repeater.Mode)evt.newValue);
            contentContainer.Add(enumField);
        }
    }


    public class LeafGraphNode : BehaviourTreeGraphNode
    {
        public LeafScript LeafScript;
        public LeafGraphNode()
        {
            AllowedChildren = AllowedChildren.None;
            NodeType = BehaviourTreeNodeType.Leaf;
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


