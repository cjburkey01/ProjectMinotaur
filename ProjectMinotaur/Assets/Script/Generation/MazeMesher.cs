using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MazeGenerate))]
public class MazeMesher : MonoBehaviour {

	public bool drawTop = true;
	public Material wallMaterial;
	public float wallHeight = 10.0f;
	public float wallWidth = 2.0f;
	public float blockWidth = 10.0f;
	public GameObject floorPrefab;

	public MazeGenerate generator { private set; get; }

	private bool builtChunks = false;
	private bool buildingChunks = false;
	private LoadingHandler loadingHandler;
	private List<GameObject> children = new List<GameObject> ();

	private int progress = 0;
	private int finished = 0;

	private int totalVert = 0;
	private int totalTri = 0;
	private int totalUv = 0;

	void Start () {
		loadingHandler = GetComponent<LoadingHandler> ();
		generator = GetComponent<MazeGenerate> ();
		if (generator == null) {
			Debug.LogError ("Maze generator not found on maze generator object. Couldn't create maze chunk meshes.");
			Destroy (gameObject);
		}
	}

	void Update () {
		if (!builtChunks && generator.generated) {
			StartCoroutine (CreateMeshes ());
		}

		if (generator.generated && builtChunks && !buildingChunks && loadingHandler != null && loadingHandler.loading) {
			print ("Verts: " + totalVert + ", Tris: " + totalTri + ", UVs: " + totalUv);
			loadingHandler.Set (false);
		}

		if (builtChunks && generator.generated && Input.GetKeyDown (KeyCode.R)) {
			builtChunks = false;
			generator.GenerateMaze ();
		}
	}

	public void CleanUp () {
		builtChunks = false;
		foreach (GameObject obj in children) {
			Destroy (obj);
		}
		children.Clear ();
	}

	IEnumerator CreateMeshes () {
		print ("Building maze meshes...");
		CleanUp ();

		progress = 0;
		finished = 0;

		totalVert = 0;
		totalTri = 0;
		totalUv = 0;

		builtChunks = true;

		for (int x = generator.widthChunks - 1; x >= 0; x--) {
			for (int y = generator.heightChunks - 1; y >= 0; y--) {
				List<Vector3> verts = new List<Vector3> ();
				List<int> tris = new List<int> ();
				List<Vector2> uvs = new List<Vector2> ();

				GameObject chunk = new GameObject ("MazeChunk (" + x + ", " + y + ")");
				chunk.transform.parent = transform;
				chunk.transform.position = GetChunkPos (x, y);
				GameObject obj = (GameObject)Instantiate (floorPrefab);
				obj.transform.position = GetChunkPos (x, y);
				obj.transform.parent = chunk.transform;
				children.Add (chunk);
				StartCoroutine (RenderMesh (chunk, CreateChunk (x, y), x, y, verts, tris, uvs));

				yield return null;
			}
		}
		yield break;
	}

	private MazeCell [,] CreateChunk (int chunkX, int chunkY) {
		int chunkSize = generator.chunkSize;
		int startX = chunkX * chunkSize;
		int startY = chunkY * chunkSize;
		MazeCell [,] data = new MazeCell [chunkSize, chunkSize];
		for (int x = 0; x < chunkSize; x++) {
			for (int y = 0; y < chunkSize; y++) {
				MazeCell cell = generator.GetCell (startX + x, startY + y);
				data [x, y] = new MazeCell (cell);
			}
		}
		return data;
	}

	private void BuildMeshFromData (GameObject obj, List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		totalVert += verts.Count;
		totalTri += tris.Count;
		totalUv += uvs.Count;

		Mesh mesh = new Mesh ();
		mesh.name = "Mesh" + obj.transform.name;
		mesh.vertices = verts.ToArray ();
		mesh.triangles = tris.ToArray ();
		mesh.uv = uvs.ToArray ();
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		mesh.RecalculateTangents ();

		verts.Clear ();
		tris.Clear ();
		uvs.Clear ();

		MeshFilter filter = obj.AddComponent<MeshFilter> ();
		MeshRenderer meshRender = obj.AddComponent<MeshRenderer> ();
		MeshCollider meshCollider = obj.AddComponent<MeshCollider> ();

		filter.mesh = null;
		filter.mesh = mesh;
		meshRender.material = wallMaterial;
		meshCollider.convex = false;
		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = mesh;

		buildingChunks = false;
	}

	IEnumerator RenderMesh (GameObject obj, MazeCell [,] data, int chunkX, int chunkY, List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		bool [] horizontal = new bool [] { drawTop, false, false, false, true, true };
		bool [] vertical = new bool [] { drawTop, false, true, true, false, false };

		for (int x = 0; x < generator.chunkSize; x++) {
			for (int y = 0; y < generator.chunkSize; y++) {
				MazeCell cell = data [x, y];

				int truX = x + chunkX * generator.chunkSize;
				int truY = y + chunkY * generator.chunkSize;

				bool onTop = y + chunkY * generator.chunkSize == generator.height - 1;
				bool onRight = x + chunkX * generator.chunkSize == generator.width - 1;
				bool hasTopWall = cell.HasWall (0);
				bool hasLeftWall = cell.HasWall (2);

				// Horizontal Wall(s)
				if (hasTopWall) {
					AddHorizontal (horizontal, x, blockWidth, wallWidth, y, verts, tris, uvs);
				}
				if (onTop) {
					AddHorizontal (horizontal, x, blockWidth, wallWidth, y + 1, verts, tris, uvs);
				}

				// Vertical Wall(s)
				if (hasLeftWall) {
					AddVertical (vertical, x, blockWidth, wallWidth, y, verts, tris, uvs);
				}
				if (onRight) {
					AddVertical (vertical, x + 1, blockWidth, wallWidth, y, verts, tris, uvs);
				}

				// Corners
				MazeCell leftCell = generator.GetCell (truX - 1, truY);
				MazeCell aboveCell = generator.GetCell (truX, truY - 1);
				bool [] corner = new bool [] { drawTop, false, !hasTopWall, (leftCell == null || !leftCell.HasWall (0)), (aboveCell == null || !aboveCell.HasWall (2)), !hasLeftWall };
				if (hasLeftWall || hasTopWall || (leftCell == null || leftCell.HasWall (0)) || (aboveCell == null || aboveCell.HasWall (2))) {
					AddCorner (corner, x, blockWidth, wallWidth, y - 1, verts, tris, uvs);
				}
				if (onRight && truY != 0) {
					AddCorner (new bool [] { drawTop, false, false, true, false, false }, x + 1, blockWidth, wallWidth, y - 1, verts, tris, uvs);
				}
				if (onTop && truX != 0) {
					AddCorner (new bool [] { drawTop, false, false, false, true, false }, x, blockWidth, wallWidth, y, verts, tris, uvs);
				}

				if (loadingHandler != null) {
					float prog = ((float)progress) / ((float)(generator.width * generator.height));
					float done = ((float)finished) / ((float)(generator.widthChunks * generator.heightChunks));
					prog *= 100.0f;
					done *= 100.0f;
					string progString = prog.ToString ("00.00");
					string doneString = done.ToString ("00.00");
					loadingHandler.displayText.text = "Materializing Chunks: " + progString + "%. Finished Chunks: " + doneString + "%.";
				}

				progress++;
				if (progress % 57 == 0) {
					yield return null;
				}
				buildingChunks = true;
			}
		}

		finished++;

		BuildMeshFromData (obj, verts, tris, uvs);
		yield break;
	}

	private void AddHorizontal (bool [] draws, int x, float bw, float ww, int uy, List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		Vector2 wallPos = new Vector2 (x * (bw + ww) + ww, (uy) * (bw + ww));
		Vector2 size = new Vector2 (bw, ww);
		DrawWallSegment (draws, wallPos, size, verts, tris, uvs);
	}

	private void AddVertical (bool [] draws, int x, float bw, float ww, int uy, List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		Vector2 wallPos = new Vector2 (x * (bw + ww), uy * (bw + ww) + ww);
		Vector2 size = new Vector2 (ww, bw);
		DrawWallSegment (draws, wallPos, size, verts, tris, uvs);
	}

	private void AddCorner (bool [] draws, int x, float bw, float ww, int uy, List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		Vector2 wallPos = new Vector2 (x * (bw + ww), (uy + 1) * (bw + ww));
		Vector2 size = new Vector2 (ww, ww);
		DrawWallSegment (draws, wallPos, size, verts, tris, uvs);
	}

	private void DrawWallSegment (bool [] sidesToDraw, Vector2 pos, Vector2 size, List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		if (sidesToDraw.Length != 6) {
			Debug.LogError ("Didn't receive right amount of side-drawing data.");
			return;
		}

		float zeroX = pos.x;
		float zeroY = 0.0f;
		float zeroZ = pos.y;
		float oneX = pos.x + size.x;
		float oneY = wallHeight;
		float oneZ = pos.y + size.y;

		// Top
		if (sidesToDraw [0]) {
			AddTrisAndUVs (verts.Count, tris, uvs, size.x, size.x);
			verts.Add (new Vector3 (oneX, oneY, zeroZ));
			verts.Add (new Vector3 (oneX, oneY, oneZ));
			verts.Add (new Vector3 (zeroX, oneY, oneZ));
			verts.Add (new Vector3 (zeroX, oneY, zeroZ));
		}

		// Bottom
		if (sidesToDraw [1]) {
			AddTrisAndUVs (verts.Count, tris, uvs, size.x, size.x);
			verts.Add (new Vector3 (oneX, zeroY, zeroZ));
			verts.Add (new Vector3 (zeroX, zeroY, zeroZ));
			verts.Add (new Vector3 (zeroX, zeroY, oneZ));
			verts.Add (new Vector3 (oneX, zeroY, oneZ));
		}

		// Right
		if (sidesToDraw [2]) {
			AddTrisAndUVs (verts.Count, tris, uvs, size.y, size.x);
			verts.Add (new Vector3 (oneX, zeroY, zeroZ));
			verts.Add (new Vector3 (oneX, zeroY, oneZ));
			verts.Add (new Vector3 (oneX, oneY, oneZ));
			verts.Add (new Vector3 (oneX, oneY, zeroZ));
		}

		// Left
		if (sidesToDraw [3]) {
			AddTrisAndUVs (verts.Count, tris, uvs, size.y, size.x);
			verts.Add (new Vector3 (zeroX, oneY, zeroZ));
			verts.Add (new Vector3 (zeroX, oneY, oneZ));
			verts.Add (new Vector3 (zeroX, zeroY, oneZ));
			verts.Add (new Vector3 (zeroX, zeroY, zeroZ));
		}

		// Front
		if (sidesToDraw [4]) {
			AddTrisAndUVs (verts.Count, tris, uvs, size.x, size.y);
			verts.Add (new Vector3 (oneX, zeroY, zeroZ));
			verts.Add (new Vector3 (oneX, oneY, zeroZ));
			verts.Add (new Vector3 (zeroX, oneY, zeroZ));
			verts.Add (new Vector3 (zeroX, zeroY, zeroZ));
		}

		// Back
		if (sidesToDraw [5]) {
			AddTrisAndUVs (verts.Count, tris, uvs, size.x, size.y);
			verts.Add (new Vector3 (zeroX, zeroY, oneZ));
			verts.Add (new Vector3 (zeroX, oneY, oneZ));
			verts.Add (new Vector3 (oneX, oneY, oneZ));
			verts.Add (new Vector3 (oneX, zeroY, oneZ));
		}
	}

	private void AddTrisAndUVs (int vertCountBefore, List<int> tris, List<Vector2> uvs, float sizeX, float sizeY) {
		tris.Add (vertCountBefore + 3);
		tris.Add (vertCountBefore + 1);
		tris.Add (vertCountBefore + 0);
		tris.Add (vertCountBefore + 1);
		tris.Add (vertCountBefore + 3);
		tris.Add (vertCountBefore + 2);
		uvs.Add (new Vector2 (1.0f, 1.0f));
		uvs.Add (new Vector2 (1.0f, 0.0f));
		uvs.Add (new Vector2 (0.0f, 0.0f));
		uvs.Add (new Vector2 (0.0f, 1.0f));
	}

	public float GetInWorldChunkSize () {
		return (generator.chunkSize * blockWidth) + (generator.chunkSize * wallWidth);
	}

	public Vector3 GetChunkPos (int x, int y) {
		return new Vector3 (x * GetInWorldChunkSize (), 0.0f, y * GetInWorldChunkSize ());
	}

	public Vector2 WorldPosOfCell (int x, int y) {
		return new Vector2 (TransformToWorld (x), TransformToWorld (y));
	}

	private float TransformToWorld (int val) {
		return ((blockWidth + wallWidth) * val) + ((blockWidth / 2) + wallWidth);
	}

}