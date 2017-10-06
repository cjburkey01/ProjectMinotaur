﻿using System;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerate : MonoBehaviour {

	public int chunkSize = 16;
	public int widthChunks = 2;
	public int heightChunks = 1;
	public bool generated { private set; get; }
	public int width { private set; get; }
	public int height { private set; get; }
	public IBuilding[] structures;
	public float chanceRemoveWall = 0.05f;

	private MazeCell sideWall;
	private MazeCell[] cells;
	private MazeMesher mesher;
	private LoadingHandler loadingHandler;
	private int seed;

	void Start() {
		loadingHandler = GetComponent<LoadingHandler>();
		mesher = GetComponent<MazeMesher>();
		if (mesher == null) {
			Debug.LogError("Mesher not found.");
			gameObject.SetActive(false);
		}

		width = chunkSize * widthChunks;
		height = chunkSize * heightChunks;

		GenerateMaze();
	}

	public void GenerateMaze() {
		i = 0;
		seed = UnityEngine.Random.Range(0, 999999999);
		GenerateMaze(seed);
	}

	public void GenerateMaze(int seed) {
		Generate(seed);
	}

	private void Generate(int seed) {
		generated = false;
		print("Generating maze...; seed is " + seed);

		if (loadingHandler != null) {
			loadingHandler.Set(true);
		}

		InitVariables();
		Stack<CellPosition> stack = new Stack<CellPosition>();
		int[] keys = new int[] { 0, 1, 2, 3 }; // Up, down, left, right
		StartCoroutine(CarveTo(0, 0, stack, keys));
	}

	private void FinishedBuilding() {
		DoneGenerating();
	}

	private void DoneGenerating() {
		generated = true;
		print("Generated maze.");
	}

	IEnumerator GenerateStructures() {
		int max = 999999999;
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				foreach (IBuilding structure in structures) {
					if (loadingHandler != null) {
						float perc = (((float) x * width + (float) y) / (width * height)) * 100.0f;
						loadingHandler.displayText.text = "Generating Structures: " + perc.ToString("00.00") + "%";
					}
					if (structure.ForCell(this, x, y)) {
						seed ++;
						if (((float) BetterRandom.Between(0, max, seed) / (float) max) <= structure.generateChance) {
							structure.OnGenerate(mesher.WorldPosOfCell(x, y), this, x, y, seed);
						}
					}
					if ((y * width + x) % 663 == 0) {
						yield return null;
					}
				}
			}
		}
		DoneGenerating();
		yield break;
	}

	int i = 0;
	IEnumerator CarveTo(int x, int y, Stack<CellPosition> stack, int[] keys) {
		MazeCell cell = GetCell(x, y);

		if (cell.init) {
			stack.Pop();
			CellPosition next = stack.Pop();
			GetCell(next.x, next.y).init = false;
			if (stack.Count > 0) {
				yield return CarveTo(next.x, next.y, stack, keys);
				yield break;
			}
			FinishedBuilding();
			yield break;
		}

		cell.init = true;
		stack.Push(new CellPosition(x, y));

		MazeCell[] neighbors = GetNeighbors(x, y);
		Shuffle(ref keys);
		int check = 0;
		int rand = 0;

		while (check ++ < keys.Length) {
			rand = keys[check - 1];

			switch (rand) {
				case 0:
					if (!neighbors[0].init) {
						ClearWalls(x, y, MazeCell.UP);
						ClearWalls(x, y - 1, MazeCell.DOWN);
						y --;
						check = keys.Length;
					}
					break;
				case 1:
					if (!neighbors[1].init) {
						ClearWalls(x, y, MazeCell.DOWN);
						ClearWalls(x, y + 1, MazeCell.UP);
						y ++;
						check = keys.Length;
					}
					break;
				case 2:
					if (!neighbors[2].init) {
						ClearWalls(x, y, MazeCell.LEFT);
						ClearWalls(x - 1, y, MazeCell.RIGHT);
						x --;
						check = keys.Length;
					}
					break;
				case 3:
					if (!neighbors[3].init) {
						ClearWalls(x, y, MazeCell.RIGHT);
						ClearWalls(x + 1, y, MazeCell.LEFT);
						x ++;
						check = keys.Length;
					}
					break;
			}

			if (loadingHandler != null) {
				loadingHandler.displayText.text = "Carving Maze: " + i;
			}

			i ++;
			if (i % 2438 == 0) {
				yield return null;
			}
		}

		yield return CarveTo(x, y, stack, keys);
		yield break;
	}

	private void Shuffle(ref int[] array) {
		//BetterRandom r = new BetterRandom();
		for (int i = array.Length; i > 0; i --) {
			int j = BetterRandom.Between(0, i - 1, seed);
			seed ++;
			//int j = r.Next(i - 1);
			int k = array[j];
			array[j] = array[i - 1];
			array[i - 1] = k;
		}
	}

	private void InitVariables() {
		sideWall = new MazeCell();
		sideWall.init = true;
		sideWall.walls = 0x1111;
		cells = new MazeCell[width * height];
		for (int i = 0; i < cells.Length; i ++) {
			cells[i] = new MazeCell();
		}
	}

	public MazeCell[] GetNeighbors(int x, int y) {
		MazeCell[] neighbors = new MazeCell[4];
		neighbors[0] = ((y > 0) ? (GetCell(x, y - 1)) : (sideWall));
		neighbors[1] = ((y < height - 1) ? (GetCell(x, y + 1)) : (sideWall));
		neighbors[2] = ((x > 0) ? (GetCell(x - 1, y)) : (sideWall));
		neighbors[3] = ((x < width - 1) ? (GetCell(x + 1, y)) : (sideWall));
		return neighbors;
	}

	public MazeCell GetCell(int x, int y) {
		int i = y * width + x;
		if (x < 0 || x > width - 1 || y < 0 || y > height - 1 || i < 0 || i >= cells.Length) {
			return null;
		}
		return cells[i];
	}

	public void SetWalls(int x, int y, int walls) {
		MazeCell cell = GetCell(x, y);
		if (cell != null) {
			cell.walls = walls;
		}
	}

	public void ClearWalls(int x, int y, int walls) {
		MazeCell cell = GetCell(x, y);
		if (cell != null) {
			cell.walls ^= walls;
		}
	}

	/*
	public void CarveTo(int x, int y, Stack<CellPosition> stack, int[] keys) {
		MazeCell cell = GetCell(x, y);

		if (cell.init) {
			stack.Pop();
			CellPosition next = stack.Pop();
			GetCell(next.x, next.y).init = false;
			if (stack.Count > 0) {
				CarveTo(next.x, next.y, stack, keys);
				return;
			}
			IsBuilt = true;
			FinishedBuilding();
			return;
		}

		cell.init = true;
		stack.Push(new CellPosition(x, y));

		MazeCell[] neighbors = GetNeighbors(x, y);
		Shuffle(ref keys);
		int check = 0;
		int rand = 0;

		while (check ++ < keys.Length) {
			i ++;
			rand = keys[check - 1];

			switch (rand) {
				case 0:
					if (!neighbors[0].init) {
						ClearWalls(x, y, MazeCell.UP);
						ClearWalls(x, y - 1, MazeCell.DOWN);
						y --;
						check = keys.Length;
					}
					break;
				case 1:
					if (!neighbors[1].init) {
						ClearWalls(x, y, MazeCell.DOWN);
						ClearWalls(x, y + 1, MazeCell.UP);
						y ++;
						check = keys.Length;
					}
					break;
				case 2:
					if (!neighbors[2].init) {
						ClearWalls(x, y, MazeCell.LEFT);
						ClearWalls(x - 1, y, MazeCell.RIGHT);
						x --;
						check = keys.Length;
					}
					break;
				case 3:
					if (!neighbors[3].init) {
						ClearWalls(x, y, MazeCell.RIGHT);
						ClearWalls(x + 1, y, MazeCell.LEFT);
						x ++;
						check = keys.Length;
					}
					break;
			}

			//yield return null;
		}

		CarveTo(x, y, stack, keys);
	}
	*/

}