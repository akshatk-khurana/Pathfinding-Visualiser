using System; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Algorithms {

    private static bool statePresent(Queue frontier, Tuple<int, int> state) {
        bool contains = false;
        while (frontier.Count > 0) {
            Tuple<int, int> currState = ((Node) frontier.Dequeue()).state;
            if (currState == state) {
                contains = true;
                break;
            }
        }
        return contains;
    }
    public static List<Tuple<int, int>> findNeighbours(Tuple<int, int> tile) {
        int x = tile.Item1;
        int y = tile.Item2;

        List<Tuple<int, int>> possible = new List<Tuple<int, int>>();

        if (x - 1 >= 0) {
            possible.Add(new Tuple<int, int>(x - 1, y));
        }

        if (x + 1 < UIManager.Instance.cols) {
            possible.Add(new Tuple<int, int>(x + 1, y));
        } 

        if (y - 1 >= 0) {
            possible.Add(new Tuple<int, int>(x, y - 1));
        } 

        if (y + 1 < UIManager.Instance.rows) {
            possible.Add(new Tuple<int, int>(x, y + 1));
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

            exploredStates.Add(current.state);

            if (tiles[current.state.Item1, current.state.Item2] == "!") {
                Debug.Log("Solved!");

                while (current.parent != null) {
                    tiles[current.state.Item1, current.state.Item2] = ",";
                    current = current.parent;
                }
                break;
            }
            
            List<Tuple<int, int>> neighbours = findNeighbours(current.state);

            foreach(Tuple<int, int> state in neighbours) { 
                int x = state.Item1;
                int y = state.Item2;

                Debug.Log($"{x} {y}");

                if (!exploredStates.Contains(state)) {
                    if (tiles[x, y] != "x") {
                        if (!statePresent(queueFrontier, state)) {
                            Node child = new Node(state, current);
                            queueFrontier.Enqueue(child);
                        }
                    }
                }
            } 
        }

        return tiles;
    }

    public static string[,] depthFirstSearch(string[,] tiles, Tuple<int, int> start) {
        // Stack<Node> stack;
        return tiles;
    } 

    public static string[,] aStarSearch(string[,] tiles, Tuple<int, int> start) {
        return tiles;
    }
}