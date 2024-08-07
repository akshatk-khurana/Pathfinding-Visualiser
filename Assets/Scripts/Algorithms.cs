using System; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Algorithms {
    private static bool statePresent(Queue<Node> frontier, Tuple<int, int> state) {
        foreach (Node node in frontier) {
            if (node.state.Equals(state)) {
                return true;
            }
        }
        return false;
    }

    private static bool statePresent(Stack<Node> frontier, Tuple<int, int> state) {
        foreach (Node node in frontier) {
            if (node.state.Equals(state)) {
                return true;
            }
        }
        return false;
    }

    public static List<Tuple<int, int>> findNeighbours(Tuple<int, int> tile) {
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

    public static string[,] breadthFirstSearch(string[,] tiles, Tuple<int, int> start) {
        HashSet<Tuple<int, int>> exploredStates = new HashSet<Tuple<int, int>>(); 
        Queue<Node> queueFrontier = new Queue<Node>();

        Node startNode = new Node(start, null);
        queueFrontier.Enqueue(startNode);

        while (queueFrontier.Count > 0) {
            Node current = queueFrontier.Dequeue();

            int cx = current.state.Item1;
            int cy = current.state.Item2;

            if (tiles[cx, cy] == "!") {
                Debug.Log("Solved!");

                current = current.parent;
                while (current.parent != null) {
                    tiles[current.state.Item1, current.state.Item2] = ",";
                    current = current.parent;
                }

                break;
            }

            exploredStates.Add(current.state);

            foreach(Tuple<int, int> state in findNeighbours(current.state)) { 
                int x = state.Item1;
                int y = state.Item2;

                if (!exploredStates.Contains(state)) {
                    if (tiles[x, y] != "x") {
                        if (!statePresent(queueFrontier, state)) {
                            Node child = new Node(state, current);
                            queueFrontier.Enqueue(child);

                            tiles[x, y] = "^";
                        } else {
                            Debug.Log($"Tile {x} {y} is already in the frontier!");
                        }
                    } else {
                        Debug.Log($"Tile {x} {y} is a wall!");
                    }
                } else {
                    Debug.Log($"Tile {x} {y} has already been explored!");
                }
            } 
        }

        return tiles;
    }
    
    public static string[,] depthFirstSearch(string[,] tiles, Tuple<int, int> start) {
        HashSet<Tuple<int, int>> exploredStates = new HashSet<Tuple<int, int>>(); 
        Stack<Node> stackFrontier = new Stack<Node>();

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
            }

            exploredStates.Add(current.state);

            foreach(Tuple<int, int> state in findNeighbours(current.state)) { 
                int x = state.Item1;
                int y = state.Item2;

                if (!exploredStates.Contains(state)) {
                    if (tiles[x, y] != "x") {
                        if (!statePresent(stackFrontier, state)) {
                            Node child = new Node(state, current);
                            stackFrontier.Push(child);

                            tiles[x, y] = "^";
                        } else {
                            Debug.Log($"Tile {x} {y} is already in the frontier!");
                        }
                    } else {
                        Debug.Log($"Tile {x} {y} is a wall!");
                    }
                } else {
                    Debug.Log($"Tile {x} {y} has already been explored!");
                }
            } 
        }

        return tiles;
    } 

    public static string[,] aStarSearch(string[,] tiles, Tuple<int, int> start) {
        return tiles;
    }
}