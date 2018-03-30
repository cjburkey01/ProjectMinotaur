using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstMaze : IAlgorithm {

	private Stack<MazeNode> cells;
	private int totalCells;
	private int visitedCells;

	public readonly double UpdatesPerSecond = 10.0d;

	public DepthFirstMaze() {
		cells = new Stack<MazeNode>();
	}

	public string GetName() {
		return "Recursive Backtracker Maze";
	}

	public IEnumerator Generate(MazeHandler handler, bool items, Maze maze, MazePos starting, bool trueGen) {
		totalCells = maze.GetSizeX() * maze.GetSizeY();
		visitedCells = 0;

		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationBegin(maze));

		if (trueGen) {
			// Make the initial cell the current cell and mark it as visited.
			MazeNode current = maze.GetNode(starting.GetX(), starting.GetY());
			if (current == null) {
				Debug.LogError("Failed to generate maze, the starting node didn't exist.");
			}
			MarkVisited(current);
			int i = 0;
			double time = Util.GetMillis();
			// While there are unvisited cells.
			while (visitedCells < totalCells) {
				if (current == null) {
					Debug.LogError("Failed to generate maze, an unexpected node was null.");
					break;
				}
				MazeNode[] unvisited = GetUnvisitedNeighbors(maze, current.GetGlobalPos());
				// If the current cell has any neighbors which have not been visited.
				if (unvisited.Length > 0) {
					MazeNode chosen = unvisited[Util.NextRand(0, unvisited.Length - 1)];
					cells.Push(current);
					RemoveWallBetween(current, chosen);
					current = chosen;
					MarkVisited(current);
				} else if (cells.Count > 0) { // If we have some cells to process.
					current = cells.Pop();
				} else {
					break;
				}
				i++;
				if (Util.GetMillis() > time + (1000.0d / UpdatesPerSecond)) {
					time = Util.GetMillis();
					PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationUpdate(maze, (float) visitedCells / (float) totalCells));
					yield return null;
				}
			}
		}

		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeGenerationFinish(maze));
	}

	private void RemoveWallBetween(MazeNode current, MazeNode next) {
		MazePos currPos = current.GetGlobalPos();
		MazePos chosPos = next.GetGlobalPos();
		if (chosPos.GetX() > currPos.GetX()) {          // Remove current right wall
			current.RemoveWall(MazeNode.RIGHT);
			next.RemoveWall(MazeNode.LEFT);
		} else if (chosPos.GetX() < currPos.GetX()) {   // Remove current left wall
			current.RemoveWall(MazeNode.LEFT);
			next.RemoveWall(MazeNode.RIGHT);
		} else if (chosPos.GetY() < currPos.GetY()) {   // Remove current top wall
			current.RemoveWall(MazeNode.TOP);
			next.RemoveWall(MazeNode.BOTTOM);
		} else {                                        // Remove current bottom wall
			current.RemoveWall(MazeNode.BOTTOM);
			next.RemoveWall(MazeNode.TOP);
		}
	}

	private void MarkVisited(MazeNode cell) {
		cell.Visited = true;
		visitedCells++;
	}

	private MazeNode[] GetUnvisitedNeighbors(Maze maze, MazePos pos) {
		List<MazeNode> outt = new List<MazeNode>();
		MazeNode up = maze.GetNode(pos.GetUp(1).GetX(), pos.GetUp(1).GetY());
		MazeNode down = maze.GetNode(pos.GetDown(1).GetX(), pos.GetDown(1).GetY());
		MazeNode right = maze.GetNode(pos.GetRight(1).GetX(), pos.GetRight(1).GetY());
		MazeNode left = maze.GetNode(pos.GetLeft(1).GetX(), pos.GetLeft(1).GetY());
		if (up != null && !up.Visited) {
			outt.Add(up);
		}
		if (down != null && !down.Visited) {
			outt.Add(down);
		}
		if (right != null && !right.Visited) {
			outt.Add(right);
		}
		if (left != null && !left.Visited) {
			outt.Add(left);
		}
		return outt.ToArray();
	}

}
