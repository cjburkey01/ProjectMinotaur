using UnityEngine;

public class MazeHandler : MonoBehaviour {

	public int chunkSize = 16;
	public int chunksX = 2;
	public int chunksY = 2;

	private Maze maze;

	void Start() {
		maze = new Maze(new DepthFirstMaze(), chunkSize, chunksX, chunksY);
		maze.Generate(new MazePos(0, 0));

		for (int x = 0; x < maze.GetSizeX(); x++) {
			for (int y = 0; y < maze.GetSizeY(); y++) {
				MazeNode node = maze.GetNode(x, y);
				if (node == null) {
					Debug.LogError("How did this happen?");
					return;
				}
				bool[] walls = node.GetWalls();
				if (walls[0]) { // TOP
					Debug.DrawLine(new Vector3(x, y, 0.0f), new Vector3(x + 1.0f, y, 0.0f), Color.red, 70.0f, false);
				}
				if (walls[1]) { // BOTTOM
					Debug.DrawLine(new Vector3(x, y + 1.0f, 0.0f), new Vector3(x + 1.0f, y + 1.0f, 0.0f), Color.red, 70.0f, false);
				}
				if (walls[2]) { // RIGHT
					Debug.DrawLine(new Vector3(x + 1.0f, y, 0.0f), new Vector3(x + 1.0f, y + 1.0f, 0.0f), Color.red, 70.0f, false);
				}
				if (walls[3]) { // LEFT
					Debug.DrawLine(new Vector3(x, y, 0.0f), new Vector3(x, y + 1.0f, 0.0f), Color.red, 70.0f, false);
				}
			}
		}
	}

}