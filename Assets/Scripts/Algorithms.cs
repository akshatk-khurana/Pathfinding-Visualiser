using System; 
using System.Collections.Generic;
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
    private static bool StatePresent(List<AStarNode> frontier, Tuple<int, int> state) {
        foreach (AStarNode node in frontier) {
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
    public static Tuple<string[,], Tuple<int, float>> BreadthFirstSearch(string[,] tiles, Tuple<int, int> start) {
        HashSet<Tuple<int, int>> exploredStates = new HashSet<Tuple<int, int>>(); 
        Queue<Node> queueFrontier = new Queue<Node>();

        float timeTaken = 0;
        int exploredCount = 0;

        Node startNode = new Node(start, null);
        queueFrontier.Enqueue(startNode);

        while (queueFrontier.Count > 0) {
            Node current = queueFrontier.Dequeue();

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
                        if (!StatePresent(queueFrontier, state)) {
                            Node child = new Node(state, current);
                            queueFrontier.Enqueue(child);
                        } 
                    }
                }
            } 
        }

        Tuple<string[,], Tuple<int, float>> results = new Tuple<string[,], Tuple<int, float>>(
            tiles,
            new Tuple<int, float>(exploredCount, timeTaken)
        );

        return results;
    }
    public static Tuple<string[,], Tuple<int, float>> DepthFirstSearch(string[,] tiles, Tuple<int, int> start) {
        HashSet<Tuple<int, int>> exploredStates = new HashSet<Tuple<int, int>>(); 
        Stack<Node> stackFrontier = new Stack<Node>();

        float timeTaken = 0;
        int exploredCount = 0;

        Node startNode = new Node(start, null);
        stackFrontier.Push(startNode);

        while (stackFrontier.Count > 0) {
            Node current = stackFrontier.Pop();

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
                        if (!StatePresent(stackFrontier, state)) {
                            Node child = new Node(state, current);
                            stackFrontier.Push(child);
                        }
                    }
                }
            } 
        }

        Tuple<string[,], Tuple<int, float>> results = new Tuple<string[,], Tuple<int, float>>(
            tiles,
            new Tuple<int, float>(exploredCount, timeTaken)
        );

        return results;
    } 
    public static Tuple<string[,], Tuple<int, float>> AStarSearch(string[,] tiles, Tuple<int, int> start, Tuple<int, int> target) {
        List<AStarNode> openList = new List<AStarNode>(); 
        List<AStarNode> closedList = new List<AStarNode>();

        float timeTaken = 0;
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

                if (tiles[x, y] == "x" || StatePresent(closedList, state)) {
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
                    bool inOpen = StatePresent(openList, state);
                    AStarNode existing = openList.Find(n => n.state.Equals(newNode.state));

                    if (!inOpen) {
                        openList.Add(newNode); 
                    } else if (newNode.F < existing.F) {
                        existing.parent = lowestFNode;
                        existing.G = newNode.G;
                        existing.H = newNode.H;
                    }
                }
            }

            exploredCount++;
        }


        Tuple<string[,], Tuple<int, float>> results = new Tuple<string[,], Tuple<int, float>>(
            tiles,
            new Tuple<int, float>(exploredCount, timeTaken)
        );

        return results;
    }
}