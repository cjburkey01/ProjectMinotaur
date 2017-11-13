using UnityEngine;

public class MazeChunk : MonoBehaviour {

	private MazeNode[,] nodes;
	private Maze maze;

	void Start() {
		maze = GetComponentInParent<Maze>();
		if (maze == null) {
			return;
		}
		nodes = new MazeNode[maze.chunkSize, maze.chunkSize];
	}

	public void setWalls(int x, int y, int walls) {
		if (inChunk(x, y)) {
			if (nodes[x, y] == null) {
				nodes[x, y] = new MazeNode();
			}
			nodes[x, y].setWalls(walls);
		}
	}

	public MazeNode getNode(int x, int y) {
		if (inChunk(x, y)) {
			return nodes[x, y];
		}
		return null;
	}

	public bool inChunk(int x, int y) {
		return x >= 0 && x < maze.chunkSize && y >= 0 && y < maze.chunkSize;
	}

}