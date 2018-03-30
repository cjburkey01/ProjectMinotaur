using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllersMaze : IAlgorithm {

	public static readonly double UpdatesPerSecond = 30.0d;
	public static readonly float JoinRightChance = 0.5f;
	public static readonly float DownChance = 0.5f;

	private int rowN;

	public string GetName() {
		return "Eller's Maze";
	}

	public IEnumerator Generate(MazeHandler handler, bool items, Maze maze, MazePos starting, bool trueGen) {
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationBegin(maze));
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationUpdate(maze, 0.0f));
		yield return null;
		if (trueGen) {
			Debug.Log("Begining external eller's maze generation.");
			EllerMaze.Maze m = EllerMaze.Eller.Generate(maze.GetSizeX(), maze.GetSizeY());
			Debug.Log("Maze generated, loading into maze memory.");
			PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationUpdate(maze, 0.25f));
			yield return null;
			for (int x = 0; x < maze.GetSizeX(); x++) {
				for (int y = 0; y < maze.GetSizeY(); y++) {
					MazeNode node = maze.GetNode(x, y);
					EllerMaze.Cell cell = m.At(y, x);
					node.SetWalls(0);
					if (cell.up) {
						node.AddWall(MazeNode.TOP);
					}
					if (cell.down) {
						node.AddWall(MazeNode.BOTTOM);
					}
					if (cell.right) {
						node.AddWall(MazeNode.RIGHT);
					}
					if (cell.left) {
						node.AddWall(MazeNode.LEFT);
					}
				}
				if (x % 3 == 0) {
					PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationUpdate(maze, (float) x / maze.GetSizeX()));
					yield return null;
				}
			}
			Debug.Log("Loaded maze nodes from maze cells. Done.");
		}
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationFinish(maze));
	}

}