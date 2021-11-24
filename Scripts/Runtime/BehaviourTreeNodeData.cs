using System;
using UnityEngine;


namespace BehaviourTree
{
    [Serializable]
    public class BehaviourTreeNodeData
    {
        public string Guid;
        public Vector2 Position;
        public BehaviourTreeNodeType NodeType;
        public LeafScript LeafScript;
        public int MaxNumberOfRepeats;
        public Repeater.Mode RepeatType;
    }
}