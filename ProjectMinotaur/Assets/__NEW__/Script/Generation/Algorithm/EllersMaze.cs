using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllersMaze : IAlgorithm {

	public static readonly double UpdatesPerSecond = 30.0d;
	public static readonly float JoinRightChance = 0.5f;
	public static readonly float DownChance = 0.5f;

	private Maze maze;

	private readonly List<List<int>> sets = new List<List<int>>();

	public string GetName() {
		return "Eller's Maze";
	}

	public IEnumerator Generate(Maze maze, MazePos starting) {
		this.maze = maze;
		double time = Util.GetMillis();
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationBegin(maze));
		int rowN = 0;

		for (int col = 0; col < maze.GetSizeX(); col++) {
			sets.Add(new List<int>());
			sets[col].Add(col);
		}

		for (int row = 0; row < maze.GetSizeY(); row++) {
			for (int col = 0; col < maze.GetSizeX(); col++) {
				MazePos pos = new MazePos(col, row);
				if (ShouldCombine(GetContainingSet(col), GetContainingSet(col + 1))) {
					CombineSets(GetContainingSet(col), GetContainingSet(col + 1));
				}
			}
		}

		if (Util.GetMillis() > time + (1000.0d / UpdatesPerSecond)) {
			time = Util.GetMillis();
			PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationUpdate(maze, rowN / (float) maze.GetSizeY()));
			yield return null;
		}

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
