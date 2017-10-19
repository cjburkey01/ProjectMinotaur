using System.Collections.Generic;
using UnityEngine;

public class ChunkMesher : MonoBehaviour {

	private List<Vector3> verts;
	private List<int> tris;
	private List<Vector2> uvs;
	private MeshFilter filter;

	public float textureSize = 1.0f;

	void Start() {
		verts = new List<Vector3>();
		tris = new List<int>();
		uvs = new List<Vector2>();
		filter = GetComponent<MeshFilter>();

		RemoveMesh();
		ClearCurrent();

		print(0x10);
	}

	public void DrawMesh() {
		Mesh mesh = new Mesh();
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();
		ClearCurrent();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		RemoveMesh();
		filter.mesh = mesh;
	}

	public void ClearCurrent() {
		verts.Clear();
		tris.Clear();
		uvs.Clear();
	}

	public void RemoveMesh() {
		filter.mesh = null;
		filter.sharedMesh = null;
	}

	// Faces: { front, back, left, right, bottom, top }
	public void DrawRectangle(Vector3 start, Vector3 end, bool[] faces) {
		bool draw = false;
		for (int i = 0; i < faces.Length; i++) {
			if (faces[i]) {
				draw = true;
				break;
			}
		}
		if (draw && faces.Length == 6) {
			Vector3 size = end - start;
			if (faces[0]) {
				BuildFace(start, Vector3.zero, size, new Vector2(size.x, size.y), Vector3.right, Vector3.up);				// Front
			}
			if (faces[1]) {
				BuildFace(start, new Vector3(1, 0, 1), size, new Vector2(size.x, size.y), Vector3.left, Vector3.up);		// Back
			}
			if (faces[2]) {
				BuildFace(start, new Vector3(0, 0, 1), size, new Vector2(size.z, size.y), Vector3.back, Vector3.up);		// Left
			}
			if (faces[3]) {
				BuildFace(start, new Vector3(1, 0, 0), size, new Vector2(size.z, size.y), Vector3.forward, Vector3.up);		// Right
			}
			if (faces[4]) {
				BuildFace(start, new Vector3(0, 0, 1), size, new Vector2(size.x, size.z), Vector3.right, Vector3.back);		// Bottom
			}
			if (faces[5]) {
				BuildFace(start, new Vector3(0, 1, 0), size, new Vector2(size.x, size.z), Vector3.right, Vector3.forward);	// Top
			}
			return;
		} else {
			Debug.LogError("Faces array incorrect size: " + faces.Length);
		}
		Debug.LogError("Rectangle not drawn");
	}

	private void BuildFace(Vector3 corner, Vector3 offset, Vector3 size, Vector2 flatSize, Vector3 right, Vector3 up) {
		int odds = 0;
		odds += (size.x < 0) ? 1 : 0;
		odds += (size.y < 0) ? 1 : 0;
		odds += (size.z < 0) ? 1 : 0;
		AddTriangles(verts.Count, odds % 2 != 0);

		Vector3 origin = corner + Vector3.Scale(offset, size);
		Vector3 oRight = origin + Vector3.Scale(right, size);
		Vector3 oRightUp = origin + Vector3.Scale(right + up, size);
		Vector3 oUp = origin + Vector3.Scale(up, size);

		verts.Add(origin);
		verts.Add(oRight);
		verts.Add(oRightUp);
		verts.Add(oUp);

		uvs.Add(new Vector2(0.0f, 0.0f));
		uvs.Add(new Vector2(0.0f, flatSize.x / textureSize));
		uvs.Add(new Vector2(flatSize.y / textureSize, flatSize.x / textureSize));
		uvs.Add(new Vector2(flatSize.y / textureSize, 0.0f));
	}

	private void AddTriangles(int i, bool reverse) {
		if (reverse) {
			tris.AddRange(new int[] { i + 2, i, i + 1, i, i + 2, i + 3 });
		} else {
			tris.AddRange(new int[] { i + 1, i, i + 2, i + 3, i + 2, i });
		}
	}

}