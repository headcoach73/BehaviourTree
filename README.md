## Unity BehaviourTree Editor

# How to use

1. Create new behaviour tree by Right-Click->Create->BehaviourTree->New Behaviour Tree in the unity editor project tab.
2. Double click on the created object to open the behaviour tree editor
3. Inside the editor, right-click->Create Node to open the NodeSearchWindow to select nodes
4. Drag connections between ports to construct graph. When done ensure you press "Save" on the toolbar to save the graph

To implement your own leaf nodes create a new class that inherits from LeafScript and implement the abstract method.
LeafScript is a scriptableobject so to add it to your graph you need to make an object, this can be done by adding the below attribute to your class
[CreateAssetMenu(menuName = "BehaviourTree/LeafScripts/YourLeafScriptName")]
This allows you to create an object in the project tab by Right-Click->Create->BehaviourTree->LeafScripts->YourLeafScriptName.
