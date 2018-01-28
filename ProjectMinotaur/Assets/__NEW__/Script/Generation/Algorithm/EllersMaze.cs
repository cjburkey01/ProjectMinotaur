using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllersMaze : IAlgorithm {

	public static readonly double UpdatesPerSecond = 30.0d;
	public static readonly float JoinRightChance = 0.5f;
	public static readonly float DownChance = 0.5f;

	private Maze maze;
	private double time;
	private int rowN;

	private readonly List<List<int>> sets = new List<List<int>>();

	public string GetName() {
		return "Eller's Maze";
	}

	public IEnumerator Generate(Maze maze, MazePos starting) {
		this.maze = maze;
		time = Util.GetMillis();
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationBegin(maze));

		Debug.Log("Begining external eller's maze generation.");
		EllerMaze.Maze m = EllerMaze.Eller.Generate(maze.GetSizeX(), maze.GetSizeY());
		Debug.Log("Maze generated, loading into maze memory.");
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationUpdate(maze, 0.5f));
		yield return null;
		for (int x = 0; x < maze.GetSizeX(); x++) {
			for (int y = 0; y < maze.GetSizeY(); y++) {
				MazeNode node = maze.GetNode(x, y);
				EllerMaze.Cell cell = m.At(y, x);
				Debug.Log("Cell at " + x + ", " + y + " = " + cell);
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
		}
		Debug.Log("Loaded maze nodes from maze cells. Done.");

		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationFinish(maze));
	}

	private int GetContainingSet(int col) {
		for (int i = 0; i < sets.Count; i++) {
			foreach (int po in sets[i]) {
				if (col.Equals(po)) {
					return i;
				}
			}
		}
		return -1;
	}

	private bool CombineSets(int set1, int set2) {
		if (set1 < 0 || set2 < 0 || set1 >= sets.Count || set2 >= sets.Count) {
			Debug.LogError("Out of bounds: " + set1 + " or " + set2);
			return false;
		}
		sets[set1].AddRange(sets[set2]);
		sets.RemoveAt(set2);
		return true;
	}

	private bool ShouldCombine(int set1, int set2) {
		if (set1 < 0 || set2 < 0 || set1 >= sets.Count || set2 >= sets.Count) {
			return false;
		}
		return (Util.NextRand(0, 1000) / 1000.0f) <= JoinRightChance;
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
