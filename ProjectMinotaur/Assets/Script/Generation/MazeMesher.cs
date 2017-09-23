using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MazeGenerate))]
[RequireComponent(typeof(MeshRenderer))]
public class MazeMesher : MonoBehaviour {

	public float wallHeight = 10.0f;
	public float wallWidth = 2.0f;
	public float blockWidth = 10.0f;
	public float textureScale = 0.5f;

	private MazeGenerate generator;
	private bool hasGenerated = false;
	private MeshRenderer meshRenderer;
	private MeshFilter meshFilter;
	private MeshCollider meshCollider;

	void Start() {
		generator = GetComponent<MazeGenerate>();
		if (generator == null) {
			Debug.LogError("Maze generator not found on maze generator object. Can't create mesh.");
			Destroy(gameObject);
		}

		meshRenderer = GetComponent<MeshRenderer>();
		if (meshRenderer == null) {
			Debug.LogError("Mesh renderer not found on maze generator object. Can't create mesh.");
			Destroy(gameObject);
		}

		meshFilter = GetComponent<MeshFilter>();
		if (meshFilter == null) {
			meshFilter = gameObject.AddComponent<MeshFilter>();
		}

		meshCollider = GetComponent<MeshCollider>();
	}

	void Update() {
		if (!hasGenerated && generator.IsBuilt) {
			hasGenerated = true;
			CreateMesh();
		}
	}

	private void CreateMesh() {
		print("Building maze mesh...");
		long startTimeMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

		meshFilter.mesh = null;
		meshFilter.sharedMesh = null;

		Mesh mesh = new Mesh();
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();

		RenderMesh(verts, tris, uvs);

		mesh.name = "MeshMaze";
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		meshFilter.mesh = mesh;

		if (meshCollider != null) {
			meshCollider.convex = false;
			meshCollider.sharedMesh = mesh;
		}

		long endTimeMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		print("Built maze mesh in " + (endTimeMs - startTimeMs) + "ms.");
	}

	private void RenderMesh(List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		float bw = (blockWidth + wallWidth);
		float ww = wallWidth;
		bool[] draws = new bool[] { false, false, true, true, true, true };

		for (int x = 0; x < generator.width; x ++) {
			for (int y = 0; y < generator.height; y ++) {
				MazeCell cell = generator.GetCell(x, y);

				int uy = generator.height - 1 - y;	// Invert Y, maze is generated with +y=Down, world is +y=Up

				// Top Wall
				if (cell.HasWall(0)) {
					float offset = 0.0f;
					float width = 0.0f;
					if (y > 0) {
						MazeCell above = generator.GetCell(x, y - 1);
						if (cell.HasWall(2) || (above != null && above.HasWall(2))) {
							offset -= ww;
							width += ww;
						} else if (!cell.HasWall(2) && (above != null && above.HasWall(2))) {
							offset -= ww;
							width += ww;
						}
						if (generator.GetCell(x - 1, y) == null) {
							offset += ww;
							width -= ww;
						}
						if (generator.GetCell(x + 1, y) == null) {
							width -= ww;
						}
					} else {
						offset -= ww;
						width += ww;
					}
					Vector2 wallPos = new Vector2(x * bw + offset + ww, (uy + 1) * bw - ww);
					Vector2 size = new Vector2(blockWidth + width + ww, ww);
					DrawWallSegment(draws, wallPos, size, verts, tris, uvs);
				}
				
				// Left Wall
				if (cell.HasWall(2)) {
					float offset = 0.0f;
					float height = 0.0f;
					if (x > 0 ) {
						MazeCell left = generator.GetCell(x - 1, y);
						if (!cell.HasWall(0) && (left == null || !left.HasWall(0))) {
							height += ww;
						}
					} else {
						if (generator.GetCell(x, y - 1) != null) {
							height += ww;
						}
					}
					Vector2 wallPos = new Vector2(x * bw, uy * bw + offset);
					Vector2 size = new Vector2(ww, blockWidth + height);
					DrawWallSegment(draws, wallPos, size, verts, tris, uvs);
				}

				if (cell.HasWall(1) && y == generator.height - 1) {
					Vector2 wallPos = new Vector2(x * bw, uy * bw - ww);
					Vector2 size = new Vector2(blockWidth + 2 * ww, ww);
					DrawWallSegment(draws, wallPos, size, verts, tris, uvs);
				}

				if (cell.HasWall(3) && x == generator.width - 1) {
					float offset = 0.0f;
					float height = 0.0f;
					if (generator.GetCell(x, y - 1) != null) {
						height += ww;
					}
					Vector2 wallPos = new Vector2((x + 1) * bw, uy * bw + offset);
					Vector2 size = new Vector2(ww, blockWidth + height);
					DrawWallSegment(draws, wallPos, size, verts, tris, uvs);
				}
			}
		}
	}

	private void DrawWallSegment(bool[] sidesToDraw, Vector2 pos, Vector2 size, List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		if (sidesToDraw.Length != 6) {
			Debug.LogError("Didn't receive right amount of side-drawing data.");
			return;
		}

		float zeroX = pos.x;
		float zeroY = 0.0f;
		float zeroZ = pos.y;
		float oneX = pos.x + size.x;
		float oneY = wallHeight;
		float oneZ = pos.y + size.y;

		// Top
		if (sidesToDraw[0]) {
			AddTrisAndUVs(verts.Count, tris, uvs, textureScale, size.x, size.x);
			verts.Add(new Vector3(oneX, oneY, zeroZ));
			verts.Add(new Vector3(oneX, oneY, oneZ));
			verts.Add(new Vector3(zeroX, oneY, oneZ));
			verts.Add(new Vector3(zeroX, oneY, zeroZ));
		}

		// Bottom
		if (sidesToDraw[1]) {
			AddTrisAndUVs(verts.Count, tris, uvs, textureScale, size.x, size.x);
			verts.Add(new Vector3(oneX, zeroY, zeroZ));
			verts.Add(new Vector3(zeroX, zeroY, zeroZ));
			verts.Add(new Vector3(zeroX, zeroY, oneZ));
			verts.Add(new Vector3(oneX, zeroY, oneZ));
		}

		// Right
		if (sidesToDraw[2]) {
			AddTrisAndUVs(verts.Count, tris, uvs, textureScale, size.y, size.x);
			verts.Add(new Vector3(oneX, zeroY, zeroZ));
			verts.Add(new Vector3(oneX, zeroY, oneZ));
			verts.Add(new Vector3(oneX, oneY, oneZ));
			verts.Add(new Vector3(oneX, oneY, zeroZ));
		}

		// Left
		if (sidesToDraw[3]) {
			AddTrisAndUVs(verts.Count, tris, uvs, textureScale, size.y, size.x);
			verts.Add(new Vector3(zeroX, oneY, zeroZ));
			verts.Add(new Vector3(zeroX, oneY, oneZ));
			verts.Add(new Vector3(zeroX, zeroY, oneZ));
			verts.Add(new Vector3(zeroX, zeroY, zeroZ));
		}

		// Front
		if (sidesToDraw[4]) {
			AddTrisAndUVs(verts.Count, tris, uvs, textureScale, size.x, size.y);
			verts.Add(new Vector3(oneX, zeroY, zeroZ));
			verts.Add(new Vector3(oneX, oneY, zeroZ));
			verts.Add(new Vector3(zeroX, oneY, zeroZ));
			verts.Add(new Vector3(zeroX, zeroY, zeroZ));
		}

		// Back
		if (sidesToDraw[5]) {
			AddTrisAndUVs(verts.Count, tris, uvs, textureScale, size.x, size.y);
			verts.Add(new Vector3(zeroX, zeroY, oneZ));
			verts.Add(new Vector3(zeroX, oneY, oneZ));
			verts.Add(new Vector3(oneX, oneY, oneZ));
			verts.Add(new Vector3(oneX, zeroY, oneZ));
		}
	}

	private void AddTrisAndUVs(int vertCountBefore, List<int> tris, List<Vector2> uvs, float scale, float sizeX, float sizeY) {
		tris.Add(vertCountBefore + 3);
		tris.Add(vertCountBefore + 1);
		tris.Add(vertCountBefore + 0);
		tris.Add(vertCountBefore + 1);
		tris.Add(vertCountBefore + 3);
		tris.Add(vertCountBefore + 2);
		uvs.Add(new Vector2(sizeX * scale, sizeY * scale));
		uvs.Add(new Vector2(sizeX * scale, 0.0f));
		uvs.Add(new Vector2(0.0f, 0.0f));
		uvs.Add(new Vector2(0.0f, sizeY * scale));
	}

}