using System;

public class testing {
    private static int ManhattanDistance(Tuple<int, int> start, Tuple<int, int> end) {
        int distanceX = start.Item1 - end.Item1;
        int distanceY = start.Item2 - end.Item2;

        distanceX =  Math.Abs(distanceX);
        distanceY = Math.Abs(distanceY);

        return distanceX + distanceY;
    }

    static void Main(string[] args) {
        Tuple<int, int> start = new Tuple<int, int>(0, 0);
        Tuple<int, int> end = new Tuple<int, int>(0, 10); 
        Console.WriteLine(ManhattanDistance(start, end));    
    }
}