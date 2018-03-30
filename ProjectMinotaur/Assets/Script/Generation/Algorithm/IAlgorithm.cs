using System.Collections;

public interface IAlgorithm {

	string GetName();
	IEnumerator Generate(MazeHandler handler, bool items, Maze maze, MazePos starting, bool trueGen);

}