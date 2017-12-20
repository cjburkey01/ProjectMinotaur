using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour {

	// Units per second.
	public float speed = 1.0f;
	public float rotationSpeed = 5.0f;
	public float yPosRatio = 0.5f;
	public MazeHandler mazeHandler;
	public GameObject loadingScreen;

	private readonly Dictionary<MazePos, bool> visited = new Dictionary<MazePos, bool>();
	private readonly Stack<MazePos> path = new Stack<MazePos>();
	private MazePos start = MazePos.NONE;
	private MazePos goal = MazePos.NONE;
	private MazePos previous = MazePos.NONE;
	private MazePos previous2 = MazePos.NONE;
	private MazePos movingTowards = MazePos.NONE;
	private float beginTime;
	private float length;
	private Vector3 refVel = Vector3.zero;

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
		if (transform.position.Equals(GoalNodePos(movingTowards, previous))) {
			BeginMovingTowardsNewNeighbor(movingTowards);
		}
		float covered = (Time.time - beginTime) * speed;
		float fracDone = covered / length;
		transform.position = Vector3.Lerp(GoalNodePos(previous, previous2), GoalNodePos(movingTowards, previous), fracDone);
		DoRotation();
	}

	private void BeginMovingTowardsNewNeighbor(MazePos currentNode) {
		visited[currentNode] = true;
		movingTowards = RandNeighbor(currentNode);
		previous2 = previous;
		previous = currentNode;
		if (movingTowards.Equals(MazePos.NONE)) {
			movingTowards = path.Pop();
		} else {
			path.Push(currentNode);
		}
		length = Vector3.Distance(GoalNodePos(currentNode, previous2), GoalNodePos(movingTowards, currentNode));
		beginTime = Time.time;
	}

	private void DoRotation() {
		if (transform.position == GoalNodePos(movingTowards, previous)) {
			return;
		}
		Vector3 lookAt = Quaternion.LookRotation(GoalNodePos(movingTowards, previous) - transform.position, Vector3.up).eulerAngles;
		Vector3 current = transform.eulerAngles;
		current.x = Mathf.SmoothDampAngle(current.x, lookAt.x, ref refVel.x, rotationSpeed);
		current.y = Mathf.SmoothDampAngle(current.y, lookAt.y, ref refVel.y, rotationSpeed);
		current.z = Mathf.SmoothDampAngle(current.z, lookAt.z, ref refVel.z, rotationSpeed);
		transform.rotation = Quaternion.Euler(current);
	}

	private void OnMazeGenerated<T>(T e) where T : EventMazeRenderChunkFinish {
		PlayerMove ply = FindObjectOfType<PlayerMove>();
		if (ply != null) {
			ply.locked = false;
		}
		loadingScreen.SetActive(false);
		e.ToString();
		if (start.Equals(MazePos.NONE) && goal.Equals(MazePos.NONE) && gameObject.activeSelf && gameObject.activeInHierarchy) {
			DoInit();
		}
	}

	private void DoInit() {
		start = MazePos.ZERO;
		goal = new MazePos(mazeHandler.GetMaze().GetSizeX() - 1, mazeHandler.GetMaze().GetSizeY() - 1);
	}

	private Vector3 TrueNodePos(MazePos node) {
		Vector3 o = mazeHandler.GetWorldPosOfNode(node, yPosRatio * mazeHandler.pathHeight);
		o.x += mazeHandler.pathWidth / 2.0f;
		o.z += mazeHandler.pathWidth / 2.0f;
		return o;
	}

	private Vector3 GoalNodePos(MazePos node, MazePos prev) {
		Vector3 n = TrueNodePos(node);
		if (node.GetX() > prev.GetX()) {
			n.x -= mazeHandler.pathWidth / 2.0f;
		} else if (node.GetX() < prev.GetX()) {
			n.x += mazeHandler.pathWidth / 2.0f;
		} else if (node.GetY() > prev.GetY()) {
			n.z -= mazeHandler.pathWidth / 2.0f;
		} else if (node.GetY() < prev.GetY()) {
			n.z += mazeHandler.pathWidth / 2.0f;
		}
		return n;
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