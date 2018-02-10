using System.Collections;
using UnityEngine;

public interface IAlgorithm {

	string GetName();
	IEnumerator Generate(MazeHandler handler, bool items, Maze maze, MazePos starting);

}