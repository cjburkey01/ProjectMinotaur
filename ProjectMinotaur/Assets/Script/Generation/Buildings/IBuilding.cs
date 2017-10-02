using UnityEngine;

public interface IBuilding {

	bool ForCell(MazeGenerate generator, int mazeX, int mazeY, int seed);
	void OnGenerate(Vector2 position, MazeGenerate generator, int mazeX, int mazeY, int seed);

}