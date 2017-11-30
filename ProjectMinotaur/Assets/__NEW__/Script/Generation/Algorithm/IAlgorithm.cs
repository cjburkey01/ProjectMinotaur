using System.Collections.Generic;

public interface IAlgorithm {

	string GetName();
	void Generate(Maze maze, MazePos starting);

}