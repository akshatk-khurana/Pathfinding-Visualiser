using System; 
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Algorithms {
    private static int ManhattanDistance(Tuple<int, int> start, Tuple<int, int> end) {
        int distanceX = start.Item1 - end.Item1;
        int distanceY = start.Item2 - end.Item2;

        distanceX =  Math.Abs(distanceX);
        distanceY = Math.Abs(distanceY);

        return distanceX + distanceY;
    }
    private static bool StatePresent(Queue<Node> frontier, Tuple<int, int> state) {
        foreach (Node node in frontier) {
            if (node.state.Equals(state)) {
                return true;
            }
        }
        return false;
    }
    private static bool StatePresent(Stack<Node> frontier, Tuple<int, int> state) {
        foreach (Node node in frontier) {
            if (node.state.Equals(state)) {
                return true;
            }
        }
        return false;
    }
    public static List<Tuple<int, int>> FindNeighbours(Tuple<int, int> tile) {
        int x = tile.Item1;
        int y = tile.Item2;

        List<Tuple<int, int>> possible = new List<Tuple<int, int>>();

        if (y + 1 < UIManager.Instance.rows) {
            possible.Add(new Tuple<int, int>(x, y + 1));
        } 

        if (y - 1 >= 0) {
            possible.Add(new Tuple<int, int>(x, y - 1));
        }

        if (x - 1 >= 0) {
            possible.Add((new Tuple<int, int>(x - 1, y)));
        }

        if (x + 1 < UIManager.Instance.cols) {
            possible.Add(new Tuple<int, int>(x + 1, y));
        }

        return possible;
    }
    public static Tuple<string[,], int> AStarSearch(string[,] tiles, Tuple<int, int> start, Tuple<int, int> target) {
        List<AStarNode> openList = new List<AStarNode>(); 
        List<AStarNode> closedList = new List<AStarNode>();

        int exploredCount = 0;

        AStarNode startNode = new AStarNode(start, null, 0, ManhattanDistance(start, target));
        openList.Add(startNode);

        bool solutionFound = false;

        while (openList.Count > 0 && !solutionFound) {
            AStarNode lowestFNode = null;

            foreach (AStarNode node in openList) {
                if (lowestFNode == null || node.F < lowestFNode.F) {
                    lowestFNode = node;
                }
            }

            openList.Remove(lowestFNode);
            closedList.Add(lowestFNode);

            foreach (Tuple<int, int> state in FindNeighbours(lowestFNode.state)) { 
                int x = state.Item1;
                int y = state.Item2;

                AStarNode existingClosed = closedList.Find(n => n.state.Equals(state));
                if (tiles[x, y] == "x" || existingClosed != null) {
                    continue;
                }

                AStarNode newNode = new AStarNode(state, lowestFNode, lowestFNode.G + 1, ManhattanDistance(state, target));

                if (tiles[x, y] == "!") {
                    solutionFound = true;
                    AStarNode current = newNode.parent;

                    while (current.parent != null) {
                        tiles[current.state.Item1, current.state.Item2] = ",";
                        current = current.parent;
                    }
                    break;
                } else {
                    AStarNode existingOpen = openList.Find(n => n.state.Equals(newNode.state));

                    if (existingOpen == null) {
                        openList.Add(newNode); 
                    } else if (newNode.F < existingOpen.F) {
                        existingOpen.parent = lowestFNode;
                        existingOpen.G = newNode.G;
                        existingOpen.H = newNode.H;
                    }
                }
            }

            exploredCount++;
        }


        Tuple<string[,], int> results = new Tuple<string[,], int>(
            tiles,
            exploredCount
        );

        return results;
    }
    public static Tuple<string[,], int> DepthAndBreadthFirstSearch(string[,] tiles, Tuple<int, int> start, string mode) {
        HashSet<Tuple<int, int>> exploredStates = new HashSet<Tuple<int, int>>();
        int exploredCount = 0;

        IEnumerable<Node> frontier;
        Queue<Node> bfsFrontier = null;
        Stack<Node> dfsFrontier = null;

        Node startNode = new Node(start, null);

        if (mode == "BFS") {
            bfsFrontier = new Queue<Node>();
            bfsFrontier.Enqueue(startNode);
            frontier = bfsFrontier;
        } else {
            dfsFrontier = new Stack<Node>();
            dfsFrontier.Push(startNode);
            frontier = dfsFrontier;
        }

        while ((mode == "BFS" && bfsFrontier.Count > 0) || (mode != "BFS" && dfsFrontier.Count > 0))
        {
            Node current = mode == "BFS" ? bfsFrontier.Dequeue() : dfsFrontier.Pop();

            int cx = current.state.Item1;
            int cy = current.state.Item2;

            if (tiles[cx, cy] == "!") {
                current = current.parent;
                while (current.parent != null) {
                    tiles[current.state.Item1, current.state.Item2] = ",";
                    current = current.parent;
                }
                break;
            } else {
                if (tiles[cx, cy] != "*") {
                    tiles[cx, cy] = "^";
                }    
            }

            exploredCount++;
            exploredStates.Add(current.state);

            foreach(Tuple<int, int> state in FindNeighbours(current.state)) { 
                int x = state.Item1;
                int y = state.Item2;

                if (!exploredStates.Contains(state)) {
                    if (tiles[x, y] != "x") {
                        bool present = false;
                        if (mode == "BFS") {
                            present = StatePresent((Queue<Node>) frontier, state);
                        } else {
                            present = StatePresent((Stack<Node>) frontier, state);
                        }

                        if (!present) {
                            Node child = new Node(state, current);

                            if (mode == "BFS") {
                                bfsFrontier.Enqueue(child);
                            } else {
                                dfsFrontier.Push(child);
                            }
                        } 
                    }
                }
            } 
        }

        
        Tuple<string[,], int> results = new Tuple<string[,], int>(
            tiles,
            exploredCount
        );

        return results;
    }
}