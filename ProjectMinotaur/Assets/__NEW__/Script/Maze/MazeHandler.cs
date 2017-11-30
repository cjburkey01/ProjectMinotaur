using UnityEngine;

public class MazeHandler : MonoBehaviour {

	public int chunkSize = 16;
	public int chunksX = 4;
	public int chunksY = 4;
	public float pathWidth = 15.0f;
	public float distanceVariability = 1.0f;
	public float padding = 15.0f;

	private Maze maze;

	void Start() {
		maze = new Maze(new DepthFirstMaze(), chunkSize, chunksX, chunksY);
		GenerateMaze();
		RenderMaze();
	}

	public void GenerateMaze() {
		maze.Generate(new MazePos(0, 0));
		Debug.Log("Generted maze");
	}

	public void RenderMaze() {
		// TMP Draw Code
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