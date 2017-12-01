using System.Collections;

public interface IAlgorithm {

	string GetName();
	IEnumerator Generate(Maze maze, MazePos starting);

}