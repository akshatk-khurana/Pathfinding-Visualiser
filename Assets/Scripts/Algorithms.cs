using System; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Algorithms {
    private List<Tuple<int, int>> findNeighbours(string[,] tiles, Tuple<int, int> tile) {
        List<Tuple<int, int>>
        //
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
        }

        return tiles;
    }

    public static string[,] depthFirstSearch(string[,] tiles, Tuple<int, int> start) {
        // Stack<Node> stack;
        return tiles;
    } 
}

//                 while node.parent is not None:
//                     actions.append(node.action)
//                     cells.append(node.state)
//                     node = node.parent
//                 return

//             self.explored.add(node.state)

//             for action, state in self.neighbors(node.state):
//                 if not frontier.contains_state(state) and state not in self.explored:
//                     child = Node(state=state, parent=node, action=action)
//                     frontier.add(child)