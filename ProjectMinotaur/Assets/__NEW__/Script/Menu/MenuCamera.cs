using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour {

	// Units per second.
	public float speed = 1.0f;
	public float rotationSpeed = 5.0f;
	public float yPosRatio = 0.5f;
	public MazeHandler mazeHandler;

	private readonly Dictionary<MazePos, bool> visited = new Dictionary<MazePos, bool>();
	private readonly Stack<MazePos> path = new Stack<MazePos>();
	private MazePos start = MazePos.NONE;
	private MazePos goal = MazePos.NONE;
	private MazePos previous = MazePos.NONE;
	private MazePos movingTowards = MazePos.NONE;
	private float beginTime;
	private float length;

	void Start() {
		PMEventSystem.GetEventSystem().AddListener<EventMazeRenderChunkFinish>(OnMazeGenerated);
		for (int x = 0; x < mazeHandler.chunksX * mazeHandler.chunkSize; x++) {
			for (int y = 0; y < mazeHandler.chunksY * mazeHandler.chunkSize; y++) {
				visited.Add(new MazePos(x, y), false);
			}
		}
	}

	void Update() {
		if (start.Equals(MazePos.NONE) || goal.Equals(MazePos.NONE)) {
			return;
		}
		if (movingTowards.Equals(MazePos.NONE)) {
			BeginMovingTowardsNewNeighbor(start);
		}
		if (transform.position.Equals(NodePos(movingTowards))) {
			BeginMovingTowardsNewNeighbor(movingTowards);
		}
		float covered = (Time.time - beginTime) * speed;
		float fracDone = covered / length;
		transform.position = Vector3.Lerp(NodePos(previous), NodePos(movingTowards), fracDone);
		DoRotation();
	}

	private void BeginMovingTowardsNewNeighbor(MazePos currentNode) {
		visited[currentNode] = true;
		previous = currentNode;
		movingTowards = RandNeighbor(previous);
		if (movingTowards.Equals(MazePos.NONE)) {
			movingTowards = path.Pop();
		} else {
			path.Push(currentNode);
		}
		length = Vector3.Distance(NodePos(previous), NodePos(movingTowards));
		beginTime = Time.time;
	}

	private void DoRotation() {
		//if (transform.position != NodePos(movingTowards)) {
		//Quaternion lookAt = Quaternion.LookRotation(NodePos(movingTowards) - transform.position, Vector3.up);
		//transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, speed * Time.deltaTime);
		//}
		transform.LookAt(NodePos(movingTowards));
	}

	private void OnMazeGenerated<T>(T e) where T : EventMazeRenderChunkFinish {
		if (start.Equals(MazePos.NONE) && goal.Equals(MazePos.NONE)) {
			DoInit();
		}
	}

	private void DoInit() {
		start = MazePos.ZERO;
		goal = new MazePos(mazeHandler.GetMaze().GetSizeX() - 1, mazeHandler.GetMaze().GetSizeY() - 1);
	}

	private Vector3 NodePos(MazePos node) {
		Vector3 o = mazeHandler.GetWorldPosOfNode(node, yPosRatio * mazeHandler.pathHeight);
		o.x += mazeHandler.pathWidth / 2.0f;
		o.z += mazeHandler.pathWidth / 2.0f;
		return o;
	}

	private MazePos[] Neighbors(MazePos node) {
		List<MazePos> outNodes = new List<MazePos>();
		MazePos[] found = mazeHandler.GetMaze().GetConnectedNeighbors(node);
		foreach (MazePos pos in found) {
			if (!visited[pos]) {
				outNodes.Add(pos);
			}
		}
		return outNodes.ToArray();
	}

	private MazePos RandNeighbor(MazePos node) {
		MazePos[] nodes = Neighbors(node);
		if (nodes.Length == 0) {
			return MazePos.NONE;
		}
		if (nodes.Length == 1) {
			return nodes[0];
		}
		return nodes[Util.NextRand(0, nodes.Length - 1)];
	}

}