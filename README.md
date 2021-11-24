# Unity BehaviourTree Editor

## Creating the Behaviour Tree

1. Create new behaviour tree by Right-Click->Create->BehaviourTree->New Behaviour Tree in the unity editor project tab.
2. Double click on the created object to open the behaviour tree editor.
3. Inside the editor, right-click->Create Node to open the NodeSearchWindow to select nodes.
4. Drag connections between ports to construct graph. When done ensure you press "Save" on the toolbar to save the graph.

## Creating Leaf Scripts

To implement logic at the leaves of the tree you need to create your own leaf scripts.

1. To implement your own leaf nodes create a new class that inherits from LeafScript and implement the abstract method.

3. LeafScript is a scriptableobject so to add it to your graph you need to make an object, this can be done by adding the CreateAssetMenu attribute to the class. An example of this attribute can be seen below.

[CreateAssetMenu(menuName = "BehaviourTree/LeafScripts/YourLeafScriptName")]

This allows you to create an object in the project tab by Right-Click->Create->BehaviourTree->LeafScripts->YourLeafScriptName.

## Using Behaviour Tree at Runtime

To evaluate the tree at runtime you must first construct it by calling .ConstructTree() on the BehaviourTreeContainer you wish to evaluate. This returns a rootnode of type Node that can be evaluated using .Evaluate(Context) and passing in a context.

It is expected you created your own context by inheriting from the Class Context.
