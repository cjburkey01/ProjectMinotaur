using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesher : MonoBehaviour {

	private List<Vector3> verts;
	private List<int> tris;
	private List<Vector2> uvs;

	private MeshFilter filter;
	//private MeshRenderer meshRenderer;

	void Start() {
		verts = new List<Vector3>();
		tris = new List<int>();
		uvs = new List<Vector2>();

		filter = GetComponent<MeshFilter>();
		//meshRenderer = GetComponent<MeshRenderer>();

		ClearMesh();
		BuildMesh();
	}

	public void BuildMesh() {
		AddRectangle(Vector3.zero, new Vector3(1.0f, 1.0f, 1.0f));
		UpdateMesh();
	}

	public void ClearMesh() {
		verts.Clear();
		tris.Clear();
		uvs.Clear();
		filter.mesh = null;
		filter.sharedMesh = null;
	}

	private void AddRectangle(Vector3 start, Vector3 size) {
		BuildFace(start, Vector3.zero, size, Vector3.right, Vector3.up);				// Front
		BuildFace(start, new Vector3(1, 0, 1), size, Vector3.left, Vector3.up);			// Back
		BuildFace(start, new Vector3(0, 0, 1), size, Vector3.back, Vector3.up);			// Left
		BuildFace(start, new Vector3(1, 0, 0), size, Vector3.forward, Vector3.up);		// Right
		BuildFace(start, new Vector3(0, 0, 1), size, Vector3.right, Vector3.back);		// Bottom
		BuildFace(start, new Vector3(0, 1, 0), size, Vector3.right, Vector3.forward);	// Top
	}

	private void BuildFace(Vector3 corner, Vector3 offset, Vector3 size, Vector3 right, Vector3 up) {
		AddTriangles(verts.Count);
		Vector3 offSize = Vector3.Scale(offset, size);
		verts.Add(corner + offSize);
		verts.Add(corner + offSize + Vector3.Scale(right, size));
		verts.Add(corner + offSize + Vector3.Scale(right + up, size));
		verts.Add(corner + offSize + Vector3.Scale(up, size));
	}

	private void UpdateMesh() {
		Mesh mesh = new Mesh();
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		filter.mesh = mesh;
	}

	private void AddTriangles(int i) {
		tris.AddRange(new int[] { i + 1, i, i + 2, i + 3, i + 2, i });
	}

}