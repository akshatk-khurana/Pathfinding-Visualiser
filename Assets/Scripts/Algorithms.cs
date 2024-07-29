using System; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Algorithms {
    private static List<Tuple<int, int>> findNeighbours(Tuple<int, int> tile) {
        List<Tuple<int, int>> candidates = new List<Tuple<int, int>>();
        int x = tile.Item1;
        int y = tile.Item2;

        if (x + 1 <= 31) {
            candidates.Add(new Tuple<int, int>(x + 1, y));
        }

        if (0 <= x - 1) {
            candidates.Add(new Tuple<int, int>(x - 1, y));
        }

        if (y + 1 <= 14) {
            candidates.Add(new Tuple<int, int>(x, y + 1));
        }

        if (0 <= y - 1) {
            candidates.Add(new Tuple<int, int>(x, y - 1));
        }

        return candidates;
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
            Tuple<int, int> currentPos = currentNode.state;
            numberExplored++;

            if (tiles[currentPos.Item1, currentPos.Item2] == "!") {
                while (currentNode.parent != null) {
                    currentPos = currentNode.state;
                    tiles[currentPos.Item1, currentPos.Item2] = ",";
                    currentNode = currentNode.parent;
                }
                break;
            }

            exploredStates.Add(currentNode.state);
            
            List<Tuple<int, int>> neighbours = findNeighbours(currentNode.state);

            foreach(Tuple<int, int> n in neighbours) { 
                Debug.Log("sfdsf");
            } 
        }

        return tiles;
    }

    public static string[,] depthFirstSearch(string[,] tiles, Tuple<int, int> start) {
        // Stack<Node> stack;
        return tiles;
    } 
}

//             for action, state in self.neighbors(node.state):
//                 if not frontier.contains_state(state) and state not in self.explored:
//                     child = Node(state=state, parent=node, action=action)
//                     frontier.add(child)