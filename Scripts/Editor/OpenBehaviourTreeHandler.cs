using UnityEditor;
using UnityEditor.Callbacks;

namespace BehaviourTree.Editor
{
    public class OpenBehaviourTreeHandler
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var container = EditorUtility.InstanceIDToObject(instanceID) as BehaviourTreeContainer;
            if (container == null)
            {
                return false; // we did not handle the open
            }
            else
            {
                BehaviourTreeGraph.OpenBehaviourTreeGraphWindow(container);
                return true; // we did handle the open
            }
        }
    }
}