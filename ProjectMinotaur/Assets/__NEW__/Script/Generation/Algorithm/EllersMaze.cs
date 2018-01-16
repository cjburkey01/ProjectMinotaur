using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllersMaze : IAlgorithm {

	public readonly double UpdatesPerSecond = 30.0d;

	private Maze maze;
	private int rowNumber;

	private readonly List<List<MazePos>> sets = new List<List<MazePos>>();
	private readonly List<MazePos> row = new List<MazePos>();

	public string GetName() {
		return "Ellers Maze";
	}

	public IEnumerator Generate(Maze maze, MazePos starting) {
		this.maze = maze;
		double time = Util.GetMillis();
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationBegin(maze));

		if (Util.GetMillis() > time + (1000.0d / UpdatesPerSecond)) {
			time = Util.GetMillis();
			PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationUpdate(maze, rowNumber / (float) maze.GetSizeY()));
			yield return null;
		}

		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationFinish(maze));
	}

	private void RemoveWallBetween(MazePos currPos, MazePos chosPos) {
		MazeNode current = maze.GetNode(currPos.GetX(), currPos.GetY());
		MazeNode next = maze.GetNode(chosPos.GetX(), chosPos.GetY());
		if (chosPos.GetX() > currPos.GetX()) {
			if (current != null) {
				current.RemoveWall(MazeNode.RIGHT);
			}
			if (next != null) {
				next.RemoveWall(MazeNode.LEFT);
			}
		} else if (chosPos.GetX() < currPos.GetX()) {
			if (current != null) {
				current.RemoveWall(MazeNode.LEFT);
			}
			if (next != null) {
				next.RemoveWall(MazeNode.RIGHT);
			}
		} else if (chosPos.GetY() < currPos.GetY()) {
			if (current != null) {
				current.RemoveWall(MazeNode.TOP);
			}
			if (next != null) {
				next.RemoveWall(MazeNode.BOTTOM);
			}
		} else {
			if (current != null) {
				current.RemoveWall(MazeNode.BOTTOM);
			}
			if (next != null) {
				next.RemoveWall(MazeNode.TOP);
			}
		}
	}

}
