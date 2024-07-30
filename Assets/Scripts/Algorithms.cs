using System; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Algorithms {
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
                possible.Add(pos);
            }
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

            foreach(Tuple<int, int> state in neighbours) { 
                int currX = state.Item1;
                int currY = state.Item2;

                if (!exploredStates.Contains(state) && tiles[currX, currY] != "x") {
                    Node child = new Node(state, currentNode);
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

//             for action, state in self.neighbors(node.state):
//                 if not frontier.contains_state(state) and state not in self.explored:
//                     child = Node(state=state, parent=node, action=action)
//                     frontier.add(child)