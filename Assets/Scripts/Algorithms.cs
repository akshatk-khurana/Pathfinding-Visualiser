using System; 
using System.Collections;

public class Algorithms { 
    public static string[,] aStarSearch(string[,] tiles, int[] start, int[] end) {
        return tiles;
    }

    public static string[,] breadthFirstSearch(string[,] tiles, Tuple<int, int> start, Tuple<int, int> end) {
        Queue<Node> queueFrontier;

        int exploredStates = 0;
        HashSet<int> odd = new HashSet<int>(); 

        Node startNode = Node();

        return tiles;
    } 

    public static string[,] depthFirstSearch(string[,] tiles, int[] start, int[] end) {
        // Stack<Node> stack;
        return tiles;
    } 
}


// def solve(self):
//         self.num_explored = 0

//         start = Node(state=self.start, parent=None, action=None)
//         frontier = StackFrontier()
//         frontier.add(start)

//         self.explored = set()

//         while True:
//             if frontier.empty():
//                 raise Exception("no solution")

//             node = frontier.remove()
//             self.num_explored += 1

//             if node.state == self.goal:
//                 actions = []
//                 cells = []
//                 while node.parent is not None:
//                     actions.append(node.action)
//                     cells.append(node.state)
//                     node = node.parent
//                 actions.reverse()
//                 cells.reverse()
//                 self.solution = (actions, cells)
//                 return

//             self.explored.add(node.state)

//             for action, state in self.neighbors(node.state):
//                 if not frontier.contains_state(state) and state not in self.explored:
//                     child = Node(state=state, parent=node, action=action)
//                     frontier.add(child)