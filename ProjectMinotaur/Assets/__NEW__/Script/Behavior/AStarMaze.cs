using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStarMaze {

	// Will return null on failure to find the end node.
	public static IEnumerator PathFind(Maze maze, MazePos start, MazePos destination) {
		List<MazePos> closed = new List<MazePos>();
		List<MazePos> open = new List<MazePos> { destination };
		Dictionary<MazePos, MazePos> cameFrom = new Dictionary<MazePos, MazePos>();
		Dictionary<MazePos, float> gScore = new Dictionary<MazePos, float>();
		Dictionary<MazePos, float> fScore = new Dictionary<MazePos, float>();

		for (int y = 0; y < maze.GetSizeY(); y++) {
			for (int x = 0; x < maze.GetSizeX(); x++) {
				cameFrom.Add(new MazePos(x, y), new MazePos(-1, -1));
				gScore.Add(new MazePos(x, y), Mathf.Infinity);
				fScore.Add(new MazePos(x, y), Mathf.Infinity);
			}
		}

		gScore[start] = 0.0f;
		fScore[start] = GetHeuristic(start, destination);

		MazePos current;
		while (open.Count > 0) {
			current = GetLowestHeuristic(fScore);

			if (current.Equals(destination)) {
				//return BuildPath(cameFrom, current);
				yield break;
			}

			open.Remove(current);
			closed.Add(current);

			MazePos[] neighbors = maze.GetConnectedNeighbors(current);
			foreach (MazePos neighbor in neighbors) {
				Debug.Log("On node: " + current + ". Neighbor: " + neighbor);
				yield return null;
				if (closed.Contains(neighbor)) {
					continue;
				}

				if (!open.Contains(neighbor)) {
					open.Add(neighbor);
				}

				float tGScore = gScore[current] + GetHeuristic(current, neighbor);
				if (tGScore >= gScore[neighbor]) {
					continue;
				}

				cameFrom[neighbor] = current;

				gScore[neighbor] = tGScore;
				fScore[neighbor] = gScore[neighbor] + GetHeuristic(neighbor, destination);
			}
		}

		// No possible path was found.
		//return null;
		yield break;
	}

	public static float GetHeuristic(MazePos start, MazePos end) {
		return (end.GetX() - start.GetX()) + (end.GetY() - start.GetY());
		//return -Mathf.Infinity;
	}

	public static MazePos GetLowestHeuristic(Dictionary<MazePos, float> dict) {
		KeyValuePair<MazePos, float> lowest = new KeyValuePair<MazePos, float>();
		foreach (KeyValuePair<MazePos, float> entry in dict) {
			if (lowest.Value < entry.Value) {
				lowest = entry;
			}
		}
		return lowest.Key;
	}

	public static Stack<MazePos> BuildPath(Dictionary<MazePos, MazePos> cameFrom, MazePos current) {
		Stack<MazePos> path = new Stack<MazePos>();
		while (cameFrom.ContainsKey(current)) {
			current = cameFrom[current];
			path.Push(current);
		}
		return path;
	}

}