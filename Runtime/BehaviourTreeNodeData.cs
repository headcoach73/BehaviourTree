using System;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    [Serializable]
    public class BehaviourTreeNodeData
    {
        public string Guid;
        public Vector2 Position;
        public BehaviourTreeNodeType NodeType;
        public LeafScript leafScript;
        public int maxNumberOfRepeats;
        public Repeater.Mode repeatType;
    }
}