using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesher : MonoBehaviour {

	private List<Vector3> verts;
	private List<int> tris;
	private List<Vector2> uvs;

	private MeshFilter filter;
	private MeshRenderer renderer;

	void Start() {
		verts = new List<Vector3>();
		tris = new List<int>();
		uvs = new List<Vector2>();

		filter = GetComponent<MeshFilter>();
		renderer = GetComponent<MeshRenderer>();

		verts.Clear();
		tris.Clear();
		uvs.Clear();
		AddRectangle(Vector3.zero, Vector3.zero, false);
		UpdateMesh();
	}

	private void AddRectangle(Vector3 start, Vector3 end, bool fourfivedeg) {
		// Front face
		AddTriangles(verts.Count, false);
		verts.Add(new Vector3(1.0f, 0.0f, 0.0f));
		verts.Add(new Vector3(0.0f, 0.0f, 0.0f));
		verts.Add(new Vector3(0.0f, 1.0f, 0.0f));
		verts.Add(new Vector3(1.0f, 1.0f, 0.0f));

		// Back face
		AddTriangles(verts.Count, true);
		verts.Add(new Vector3(1.0f, 0.0f, 1.0f));
		verts.Add(new Vector3(0.0f, 0.0f, 1.0f));
		verts.Add(new Vector3(0.0f, 1.0f, 1.0f));
		verts.Add(new Vector3(1.0f, 1.0f, 1.0f));
	}

	private void UpdateMesh() {
		filter.mesh = null;
		Mesh mesh = new Mesh();
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		filter.mesh = mesh;
	}

	private void AddTriangles(int i, bool revTris) {
		if (revTris) {
			tris.AddRange(new int[] { i + 1, i, i + 2, i + 3, i + 2, i });
		} else {
			tris.AddRange(new int[] { i + 2, i, i + 1, i, i + 2, i + 3 });
		}
	}

}