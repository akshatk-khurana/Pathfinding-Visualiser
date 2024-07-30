using System; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Algorithms {
    private static bool containsState(Queue frontier, Tuple<int, int> state) {
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
    private static List<Tuple<int, int>> findNeighbours(Tuple<int, int> tile) {
        int x = tile.Item1;
        int y = tile.Item2;

        List<Tuple<int, int>> possible = new List<Tuple<int, int>>();
        List<Tuple<int, int>> candidates = new List<Tuple<int, int>>();

        candidates.Add(new Tuple<int, int>(x + 1, y));
        candidates.Add(new Tuple<int, int>(x - 1, y));
        candidates.Add(new Tuple<int, int>(x, y + 1));
        candidates.Add(new Tuple<int, int>(x, y - 1));

        foreach(Tuple<int, int> pos in candidates) {
            bool validX = pos.Item1 >= 0 && pos.Item1 <= UIManager.Instance.rows;
            bool validY = pos.Item2 >= 0 && pos.Item1 <= UIManager.Instance.cols;

            if (validX && validY) {
                Debug.Log($"{pos.Item1} {pos.Item2}");
                possible.Add(pos);
            }
        }

        return possible;
    }
    public static string[,] aStarSearch(string[,] tiles, Tuple<int, int> start) {
        return tiles;
    }

    public static string[,] breadthFirstSearch(string[,] tiles, Tuple<int, int> start) {
        HashSet<Tuple<int, int>> exploredStates = new HashSet<Tuple<int, int>>(); 
        int numberExplored = 0;
        Node startNode = new Node(start, null);

        Queue queueFrontier = new Queue();
        queueFrontier.Enqueue(startNode);

        while (true) {
            if (queueFrontier.Count == 0) {
                Debug.Log("No possible solution.");
                break;
            }

            Node currentNode = (Node) queueFrontier.Dequeue();
            numberExplored++;

            if (tiles[currentNode.state.Item1, currentNode.state.Item2] == "!") {
                Debug.Log("Solved!");
                while (currentNode.parent != null) {
                    tiles[currentNode.state.Item1, currentNode.state.Item2] = ",";
                    currentNode = currentNode.parent;
                }
                break;
            }

            exploredStates.Add(currentNode.state);
            
            List<Tuple<int, int>> neighbours = findNeighbours(currentNode.state);

            foreach(Tuple<int, int> neighbourState in neighbours) { 
                int currX = neighbourState.Item1;
                int currY = neighbourState.Item2;

                bool inExplored = exploredStates.Contains(neighbourState);
                bool isWall = tiles[currX, currY] == "x";
                bool inFrontier = containsState(queueFrontier, neighbourState);

                Debug.Log("-------");
                Debug.Log($"{currX} {currY}");
                Debug.Log($"Has been explored: {inExplored}");
                Debug.Log($"Is a wall: {isWall}");
                Debug.Log($"Is already in the frontier: {inFrontier}");
                Debug.Log("-------");

                if (!inExplored && !isWall && !inFrontier) {
                    Debug.Log("Child node added!");
                    tiles[currX, currY] = ",";
                    
                    Node child = new Node(neighbourState, currentNode);
                    queueFrontier.Enqueue(child);
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