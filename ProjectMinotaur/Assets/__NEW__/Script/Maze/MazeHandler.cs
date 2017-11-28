using UnityEngine;

public class MazeHandler : MonoBehaviour {

	public int chunkSize = 16;
	public int chunksX = 2;
	public int chunksY = 2;

	private Maze maze;

	void Start() {
		maze = new Maze(chunkSize, chunksX, chunksY);
		maze.Generate();
	}

}