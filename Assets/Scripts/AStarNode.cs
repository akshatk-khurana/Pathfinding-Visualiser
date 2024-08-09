using System;

public class AStarNode {
    public Tuple<int, int> state;
    public AStarNode parent;
    public int F;
    public int G;
    private int H;
    public AStarNode(Tuple<int, int> givenState, AStarNode givenParent, int g, int h) {
        state = givenState;
        parent = givenParent;

        G = g; 
        H = h;
        F = g + h;
    }
}