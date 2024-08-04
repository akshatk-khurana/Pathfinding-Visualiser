using System; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Algorithms {

    private static bool containsState(Queue frontier, Tuple<int, int> state) {
        bool contains = false;
        while (frontier.Count > 0) {
            Tuple<int, int> currState = ((Node) frontier.Dequeue()).state;
            if (currState.Item1 == state.Item1 && currState.Item2 == state.Item2) {
                contains = true;
                break;
            }
        }
        return contains;
    }
    public static List<Tuple<int, int>> FindNeighbours(Tuple<int, int> tile) {
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
    public static string[,] aStarSearch(string[,] tiles, Tuple<int, int> start) {
        return tiles;
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
            // if (tiles[currentNode.state.Item1, currentNode.state.Item2] == "!") {
            //     Debug.Log("Solved!");

            //     while (currentNode.parent != null) {
            //         currentNode = currentNode.parent;
            //     }
            //     break;
            // }

            exploredStates.Add(current.state);
            
            List<Tuple<int, int>> neighbours = FindNeighbours(current.state);

            foreach(Tuple<int, int> state in neighbours) { 
                int x = state.Item1;
                int y = state.Item2;

                bool inExplored = exploredStates.Contains(state);
                bool isWall = tiles[x, y] == "x";
                bool inFrontier = containsState(queueFrontier, state);

                string one = $"Coordinates: {x} {y}\n";
                string two = $"Has been explored: {inExplored}\n";
                string three = $"Is a wall: {isWall}\n";
                string four = $"Is already in the frontier: {inFrontier}\n";

                Debug.Log(one+two+three+four);
                
                if (!inExplored && !isWall && !inFrontier) {
                    Node child = new Node(state, current);
                    queueFrontier.Enqueue(child);

                    tiles[x, y] = ",";
                }
            } 
        }

        return tiles;
    }

    public static string[,] depthFirstSearch(string[,] tiles, Tuple<int, int> start) {
        // Stack<Node> stack;
        return tiles;
    } 
}