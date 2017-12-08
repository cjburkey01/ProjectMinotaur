using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStarMaze {

	// Will return null on failure to find the end node.
	public static IEnumerator PathFind(Maze maze, MazePos start, MazePos destination) {
		double startTime = Util.GetMillis();
		List<MazePos> closed = new List<MazePos>();
		List<MazePos> testing = new List<MazePos> { start };
		Dictionary<MazePos, MazePos> cameFrom = new Dictionary<MazePos, MazePos>();
		Dictionary<MazePos, float> localScores = new Dictionary<MazePos, float>();
		Dictionary<MazePos, float> globalScores = new Dictionary<MazePos, float>();

		// Default the values
		for (int x = 0; x < maze.GetSizeX(); x++) {
			for (int y = 0; y < maze.GetSizeY(); y++) {
				MazePos pos = new MazePos(x, y);
				cameFrom.Add(pos, MazePos.NONE);
				localScores.Add(pos, Mathf.Infinity);
				globalScores.Add(pos, Mathf.Infinity);
			}
		}

		MazePos current = MazePos.NONE;
		localScores[start] = 0.0f;
		globalScores[start] = GetDistSq(start, destination);

		bool done = false;
		while (testing.Count > 0 && !done) {
			current = GetLowestGlobal(testing, globalScores);
			if (current.Equals(destination)) {
				// Success!
				done = true;
				break;
			}
			testing.Remove(current);
			closed.Add(current);

			MazePos[] neighbors = maze.GetConnectedNeighbors(current);
			foreach (MazePos neighbor in neighbors) {
				if (closed.Contains(neighbor)) {
					continue;
				}
				if (!testing.Contains(neighbor)) {
					testing.Add(neighbor);
				}
				float tentativeScore = localScores[current] + 1;
				if (tentativeScore >= localScores[neighbor]) {
					continue;
				}
				cameFrom[neighbor] = current;
				localScores[neighbor] = tentativeScore;
				globalScores[neighbor] = localScores[neighbor] + GetDistSq(neighbor, destination);
			}
		}

		Stack<MazePos> path = BuildPath(cameFrom, current);

		double timeTakenMs = Util.GetMillis() - startTime;
		Debug.Log("Done, path has " + path.Count + " nodes. Took " + (timeTakenMs / 1000.0d).ToString("0.##") + "s.");

		// Just draw a path, for now.
		MazePos prev = MazePos.NONE;
		int i = 0;
		while (path.Count > 0) {
			MazePos node = path.Pop();
			if (!prev.Equals(MazePos.NONE)) {
				Debug.DrawLine(new Vector3(prev.GetX() + 0.5f, 0.1f, prev.GetY() + 0.5f), new Vector3(node.GetX() + 0.5f, 0.1f, node.GetY() + 0.5f), Color.blue, 120.0f, false);
			}
			i++;
			if (i >= 60) {
				i = 0;
				yield return null;
			}
			prev = node;
		}

		yield break;
	}

	private static float GetDistSq(MazePos start, MazePos end) {
		return Mathf.Pow((end.GetX() - start.GetX()), 2) + Mathf.Pow((end.GetY() - start.GetY()), 2);
	}

	private static MazePos GetLowestGlobal(List<MazePos> open, Dictionary<MazePos, float> dict) {
		MazePos lowest = MazePos.NONE;
		float min = Mathf.Infinity;
		foreach (MazePos pos in open) {
			if (dict[pos] < min) {
				lowest = pos;
				min = dict[pos];
			}
		}
		return lowest;
	}

	private static Stack<MazePos> BuildPath(Dictionary<MazePos, MazePos> cameFrom, MazePos current) {
		Stack<MazePos> path = new Stack<MazePos>();
		while (cameFrom.ContainsKey(current)) {
			current = cameFrom[current];
			path.Push(current);
		}
		return path;
	}

}