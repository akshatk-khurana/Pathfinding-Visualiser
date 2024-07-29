using System;

public class Node {
    public Tuple<int, int> state;
    public Node parent;
    public Node(Tuple<int, int> givenState, Node givenParent) {
        state = givenState;
        parent = givenParent;
    }
}
