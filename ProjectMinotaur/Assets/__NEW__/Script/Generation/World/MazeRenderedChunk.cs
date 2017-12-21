using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderedChunk : MonoBehaviour {

	public Material wallMaterial;
	public bool destroyed;
	public GameObject[] lights;
	public float textureSize = 15.0f;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshCollider meshCollider;

	public IEnumerator Render(MazeHandler handler, MazeChunk chunk) {
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeRenderChunkBegin(handler.GetMaze(), chunk));
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();
		for (int x = 0; x < chunk.GetSize(); x++) {
			for (int y = 0; y < chunk.GetSize(); y++) {
				if (lights.Length > 0) {
					GameObject objLight = lights[Util.NextRand(0, lights.Length - 1)];
					GameObject outLight = Instantiate(objLight, handler.GetWorldPosOfNode(chunk.GetNode(x, y).GetGlobalPos(), 0.0f) + new Vector3(handler.pathWidth / 2.0f, 0.5f, handler.pathWidth / 2.0f), Quaternion.identity);
					outLight.transform.parent = transform;
					outLight.transform.name = "Light: " + new MazePos(x, y);
				}
			}
			yield return null;
		}
		for (int x = 0; x < chunk.GetSize(); x++) {
			for (int y = 0; y < chunk.GetSize(); y++) {
				int worldX = chunk.GetPosition().GetX() * handler.chunkSize + x;
				int worldY = chunk.GetPosition().GetY() * handler.chunkSize + y;
				Maze maze = handler.GetMaze();
				DrawNode(verts, tris, uvs, handler, maze.GetNode(worldX, worldY), handler.GetWorldPosOfNode(new MazePos(worldX, worldY + 1), 0.0f), handler.GetWorldPosOfNode(new MazePos(worldX + 1, worldY), 0.0f));
			}
			yield return null;
		}
		GenerateMesh(verts, tris, uvs);
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeRenderChunkFinish(handler.GetMaze(), chunk));
	}

	private void DrawNode(List<Vector3> verts, List<int> tris, List<Vector2> uvs, MazeHandler handler, MazeNode node, Vector3 bNode, Vector3 rNode) {
		float width = handler.pathWidth;
		Vector3 corner = handler.GetWorldPosOfNode(node.GetGlobalPos(), 0.0f);
		Vector3 up = Vector3.up * handler.pathHeight;
		Vector3 right = Vector3.right * width;
		Vector3 forward = Vector3.forward * width;
		if (node.HasWall(MazeNode.TOP)) {
			Debug.DrawLine(corner + right + up, corner + up, Color.red, 60.0f, true);
			AddQuad(verts, tris, uvs, corner + right, up, -right, Vector2.zero, new Vector2(width / textureSize, handler.pathHeight / textureSize));
		}
		if (node.HasWall(MazeNode.LEFT)) {
			Debug.DrawLine(corner + up, corner + forward + up, Color.red, 60.0f, true);
			AddQuad(verts, tris, uvs, corner, up, forward, Vector2.zero, new Vector2(width / textureSize, handler.pathHeight / textureSize));
		}
		if (node.HasWall(MazeNode.BOTTOM)) {
			Debug.DrawLine(corner + forward + up, corner + forward + right + up, Color.red, 60.0f, true);
			AddQuad(verts, tris, uvs, corner + forward, up, right, Vector2.zero, new Vector2(width / textureSize, handler.pathHeight / textureSize));
		} else {
			//AddQuad(verts, tris, uvs, corner + forward, up, Vector3.forward * spread, true);
			//AddQuad(verts, tris, uvs, corner + (Vector3.forward * spread) + forward + right, up, -Vector3.forward * spread, true);
			Vector2 uvMax = new Vector2(Vector3.Distance(corner + forward, bNode) / textureSize, handler.pathHeight / textureSize);
			Debug.DrawLine(corner + forward + up, bNode + up, Color.red, 60.0f, true);
			Debug.DrawLine(bNode + right + up, corner + forward + right + up, Color.red, 60.0f, true);
			AddQuadCorners(verts, tris, uvs, corner + forward, corner + forward + up, bNode + up, bNode, Vector2.zero, uvMax);
			AddQuadCorners(verts, tris, uvs, bNode + right, bNode + right + up, corner + forward + right + up, corner + forward + right, Vector2.zero, uvMax);
		}
		if (node.HasWall(MazeNode.RIGHT)) {
			Debug.DrawLine(corner + right + forward + up, corner + right + up, Color.red, 60.0f, true);
			AddQuad(verts, tris, uvs, corner + right + forward, up, -forward, Vector2.zero, new Vector2(width / textureSize, handler.pathHeight / textureSize));
		} else {
			//AddQuad(verts, tris, uvs, corner + right + forward, up, Vector3.right * spread, true);
			//AddQuad(verts, tris, uvs, corner + (Vector3.right * spread) + right, up, -Vector3.right * spread, true);
			Vector2 uvMax = new Vector2(Vector3.Distance(corner + forward + right, rNode + forward) / textureSize, handler.pathHeight / textureSize);
			Debug.DrawLine(corner + forward + right + up, rNode + forward + up, Color.red, 60.0f, true);
			Debug.DrawLine(rNode + up, corner + right + up, Color.red, 60.0f, true);
			AddQuadCorners(verts, tris, uvs, corner + forward + right, corner + forward + right + up, rNode + forward + up, rNode + forward, Vector2.zero, uvMax);
			AddQuadCorners(verts, tris, uvs, rNode, rNode + up, corner + right + up, corner + right, Vector2.zero, uvMax);
		}
	}

	private void AddQuadCorners(List<Vector3> verts, List<int> tris, List<Vector2> uvs, Vector3 bottomLeft, Vector3 topLeft, Vector3 topRight, Vector3 bottomRight, Vector2 uvMin, Vector3 uvMax) {
		int i = verts.Count;
		verts.AddRange(new Vector3[] { bottomLeft, topLeft, topRight, bottomRight });
		tris.AddRange(new int[] { i + 1, i + 2, i, i + 3, i, i + 2 });
		uvs.AddRange(new Vector2[] { new Vector2(uvMin.x, uvMax.y), uvMin, new Vector2(uvMax.x, uvMin.y), uvMax });
	}

	private void AddQuad(List<Vector3> verts, List<int> tris, List<Vector2> uvs, Vector3 corner, Vector3 up, Vector3 right, Vector2 uvMin, Vector3 uvMax) {
		int i = verts.Count;
		verts.AddRange(new Vector3[] { corner, corner + up, corner + up + right, corner + right });
		tris.AddRange(new int[] { i + 1, i + 2, i, i + 3, i, i + 2 });
		uvs.AddRange(new Vector2[] { new Vector2(uvMin.x, uvMax.y), uvMin, new Vector2(uvMax.x, uvMin.y), uvMax });
	}

	private void GenerateMesh(List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		if (destroyed) {
			return;
		}
		if (meshFilter == null || meshRenderer == null) {
			Init();
		}
		Mesh mesh = new Mesh() {
			vertices = verts.ToArray(),
			triangles = tris.ToArray(),
			uv = uvs.ToArray()
		};
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		meshFilter.mesh = null;
		meshFilter.sharedMesh = null;
		meshFilter.mesh = mesh;
		meshCollider.sharedMesh = mesh;
	}

	private void Init() {
		if (meshFilter == null) {
			meshFilter = GetComponent<MeshFilter>();
			if (meshFilter == null) {
				meshFilter = gameObject.AddComponent<MeshFilter>();
			}
		}
		if (meshRenderer == null) {
			meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer == null) {
				meshRenderer = gameObject.AddComponent<MeshRenderer>();
			}
		}
		if (meshCollider == null) {
			meshCollider = GetComponent<MeshCollider>();
			if (meshCollider == null) {
				meshCollider = gameObject.AddComponent<MeshCollider>();
			}
		}
		meshRenderer.material = wallMaterial;
	}

}