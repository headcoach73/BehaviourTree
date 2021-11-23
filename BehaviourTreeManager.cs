using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree 
{
    public class BehaviourTreeManager : MonoBehaviour
    {
        public static BehaviourTreeManager Singleton;

        private void Awake()
        {
            Singleton = this;
        }

        [SerializeField]
        BehaviourTreeContainer ChangeSailStateTreeContainer;
        Selector ChangeSailStateTreeRoot;
        [SerializeField]
        BehaviourTreeContainer GotToWaypointDesitinationTreeContainer;
        Selector GotoWaypointDestinationRoot;

        private void Start()
        {
            ChangeSailStateTreeRoot = ChangeSailStateTreeContainer.ConstructTree() as Selector;
            GotoWaypointDestinationRoot = GotToWaypointDesitinationTreeContainer.ConstructTree() as Selector;
        }

        public static Selector GetChangeSailStateTree()
        {
            return Singleton.ChangeSailStateTreeRoot;
        }

        public static Selector GotoWaypointDestinationTree()
        {
            return Singleton.GotoWaypointDestinationRoot;
        }
    }
}


