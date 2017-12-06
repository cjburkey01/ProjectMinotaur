using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderedChunk : MonoBehaviour {

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;

	public Material wallMaterial;

	public IEnumerator Render(MazeHandler handler, MazeChunk chunk) {
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeRenderChunkBegin(handler.GetMaze(), chunk));
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();
		for (int x = 0; x < chunk.GetSize(); x++) {
			for (int y = 0; y < chunk.GetSize(); y++) {
				DrawNode(verts, tris, uvs, handler, chunk.GetNode(x, y));
			}
			yield return null;
		}
		GenerateMesh(verts, tris, uvs);
		PMEventSystem.GetEventSystem().TriggerEvent(new EventMazeRenderChunkFinish(handler.GetMaze(), chunk));
	}

	private void DrawNode(List<Vector3> verts, List<int> tris, List<Vector2> uvs, MazeHandler handler, MazeNode node) {
		float width = handler.pathWidth;
		Vector3 pos = new Vector3(node.GetGlobalPos().GetX() * width, 0.0f, node.GetGlobalPos().GetY() * width);
		Vector3 up = Vector3.up * handler.pathHeight;
		Vector3 right = Vector3.right * width;
		Vector3 forward = Vector3.forward * width;
		if (node.HasWall(MazeNode.BOTTOM)) {
			AddQuad(verts, tris, uvs, pos + new Vector3(0.0f, 0.0f, width), up, right, true);// (BOTTOM wall)
		}
		if (node.HasWall(MazeNode.LEFT)) {
			AddQuad(verts, tris, uvs, pos, up, forward, true);// (LEFT wall)
		}
		if (node.HasWall(MazeNode.TOP)) {
			AddQuad(verts, tris, uvs, pos + new Vector3(width, 0.0f, 0.0f), up, -right, true);// (TOP wall)
		} else {
			Debug.DrawLine(new Vector3(pos.x + 0.5f, 2.0f, pos.z + 0.5f), new Vector3(pos.x + 0.5f, 2.0f, pos.z - 0.5f), Color.red, 60.0f, false);
		}
		if (node.HasWall(MazeNode.RIGHT)) {
			AddQuad(verts, tris, uvs, pos + new Vector3(width, 0.0f, width), up, -forward, true);// (RIGHT wall)
		} else {
			Debug.DrawLine(new Vector3(pos.x + 0.5f, 2.0f, pos.z + 0.5f), new Vector3(pos.x + 1.5f, 2.0f, pos.z + 0.5f), Color.red, 60.0f, false);
		}
	}

	private void AddQuad(List<Vector3> verts, List<int> tris, List<Vector2> uvs, Vector3 corner, Vector3 up, Vector3 right, bool drawTexture) {
		int i = verts.Count;
		verts.AddRange(new Vector3[] { corner, corner + up, corner + up + right, corner + right });
		tris.AddRange(new int[] { i + 1, i + 2, i, i + 3, i, i + 2 });
		if (drawTexture) {
			uvs.AddRange(new Vector2[] { new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f) });
		} else {
			uvs.AddRange(new Vector2[] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero });
		}
	}

	private void GenerateMesh(List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
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
		meshRenderer.material = wallMaterial;
	}

}