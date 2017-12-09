using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderedChunk : MonoBehaviour {

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;

	public Material wallMaterial;
	public bool destroyed;

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
		float spread = handler.pathSpread;
		Vector3 corner = handler.GetWorldPosOfNode(node.GetGlobalPos(), 0.0f);
		Vector3 up = Vector3.up * handler.pathHeight;
		Vector3 right = Vector3.right * width;
		Vector3 forward = Vector3.forward * width;
		if (node.HasWall(MazeNode.TOP)) {
			AddQuad(verts, tris, uvs, corner + right, up, -right, true);
		}
		if (node.HasWall(MazeNode.LEFT)) {
			AddQuad(verts, tris, uvs, corner, up, forward, true);
		}
		if (node.HasWall(MazeNode.BOTTOM)) {
			AddQuad(verts, tris, uvs, corner + forward, up, right, true);
		} else {
			AddQuad(verts, tris, uvs, corner + forward, up, Vector3.forward * spread, true);
			AddQuad(verts, tris, uvs, corner + (Vector3.forward * spread) + forward + right, up, -Vector3.forward * spread, true);
		}
		if (node.HasWall(MazeNode.RIGHT)) {
			AddQuad(verts, tris, uvs, corner + right + forward, up, -forward, true);
		} else {
			AddQuad(verts, tris, uvs, corner + right + forward, up, Vector3.right * spread, true);
			AddQuad(verts, tris, uvs, corner + (Vector3.right * spread) + right, up, -Vector3.right * spread, true);
		}
	}

	private void AddQuad(List<Vector3> verts, List<int> tris, List<Vector2> uvs, Vector3 corner, Vector3 up, Vector3 right, bool drawBack) {
		AddQuad(verts, tris, uvs, corner, up, right, true, false);
		if (drawBack) {
			AddQuad(verts, tris, uvs, corner, up, right, false, true);
		}
	}

	private void AddQuad(List<Vector3> verts, List<int> tris, List<Vector2> uvs, Vector3 corner, Vector3 up, Vector3 right, bool drawTexture, bool reverse) {
		int i = verts.Count;
		verts.AddRange(new Vector3[] { corner, corner + up, corner + up + right, corner + right });
		if (!reverse) {
			tris.AddRange(new int[] { i + 1, i + 2, i, i + 3, i, i + 2 });
		} else {
			tris.AddRange(new int[] { i, i + 2, i + 1, i + 2, i, i + 3 });
		}
		if (drawTexture) {
			uvs.AddRange(new Vector2[] { new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f) });
		} else {
			uvs.AddRange(new Vector2[] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero });
		}
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