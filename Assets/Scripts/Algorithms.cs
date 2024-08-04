using System; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Algorithms {
    
    private static bool statePresent(Queue frontier, Tuple<int, int> state) {
        bool contains = false;
        while (frontier.Count > 0) {
            Tuple<int, int> current = ((Node) frontier.Dequeue()).state;
            if (current.Item1 == state.Item1) {
                if (current.Item2 == state.Item2) {
                    contains = true;
                    break;
                }
            }
        }
        return contains;
    }
    private static bool statePresent(Stack frontier, Tuple<int, int> state) {
        bool contains = false;
        while (frontier.Count > 0) {
            Tuple<int, int> current = ((Node) frontier.Pop()).state;
            if (current.Item1 == state.Item1) {
                if (current.Item2 == state.Item2) {
                    contains = true;
                    break;
                }
            }
        }
        return contains;
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
        Queue queueFrontier = new Queue();

        Node startNode = new Node(start, null);
        queueFrontier.Enqueue(startNode);

        while (true) {
            if (queueFrontier.Count == 0) {
                Debug.Log("No possible solution.");
                break;
            }

            Node current = (Node) queueFrontier.Dequeue();

            if (tiles[current.state.Item1, current.state.Item2] == "!") {
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
        Stack stackFrontier = new Stack();

        Node startNode = new Node(start, null);
        stackFrontier.Push(startNode);

        while (true) {
            if (stackFrontier.Count == 0) {
                Debug.Log("No possible solution.");
                break;
            }

            Node current = (Node) stackFrontier.Pop();

            if (tiles[current.state.Item1, current.state.Item2] == "!") {
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
                        if (!statePresent(stackFrontier, state)) {
                            Node child = new Node(state, current);
                            stackFrontier.Push(child);
                        }
                    }
                }
            } 
        }

        return tiles;
    } 

    public static string[,] aStarSearch(string[,] tiles, Tuple<int, int> start) {
        return tiles;
    }
}