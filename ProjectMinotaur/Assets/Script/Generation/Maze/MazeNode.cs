﻿using System.Collections.Generic;
using UnityEngine;

public class MazeNode {

	public readonly static int TOP = 1 << 0;        // The top wall
	public readonly static int BOTTOM = 1 << 1;     // The bottom wall
	public readonly static int RIGHT = 1 << 2;      // The right wall
	public readonly static int LEFT = 1 << 3;       // The left wall

	// The walls enabled or disabled on this node.
	private int walls;
	private readonly MazePos pos;
	private readonly MazePos globalP;
	private Vector3 worldOffset;
	public bool Visited;

	// Defaults the walls to none
	public MazeNode(int x, int y, int gx, int gy, Vector3 worldOffset) {
		walls = TOP | BOTTOM | RIGHT | LEFT;
		pos = new MazePos(x, y);
		globalP = new MazePos(gx, gy);
		this.worldOffset = worldOffset;
	}

	public MazeNode(int chunkX, int chunkY, int chunkSize, int x, int y, int walls, Vector2 worldOffset) {
		this.walls = walls;
		pos = new MazePos(x, y);
		globalP = new MazePos(chunkX * chunkSize, chunkY * chunkSize);
		this.worldOffset = new Vector3(worldOffset.x, 0.0f, worldOffset.y);
	}

	public void SetWalls(int walls) {
		this.walls = walls;
	}

	public int GetWallData() {
		return walls;
	}

	// Adds the specified wall to the node, providing it is not already enabled.
	public void AddWall(int wallCode) {
		if (HasWall(wallCode)) {
			return;
		}
		walls |= wallCode;
	}

	// Removes the specific wall from the node, providing it is enabled.
	public void RemoveWall(int wallCode) {
		if (!HasWall(wallCode)) {
			return;
		}
		walls ^= wallCode;
	}

	// Returns whether or not the provided wall is enabled or disabled.
	public bool HasWall(int wallCode) {
		return (walls & wallCode) != 0;
	}

	// Returns the state of all walls for this node in the following order:
	// TOP, BOTTOM, RIGHT, LEFT
	public bool[] GetWalls() {
		return new bool[] { HasWall(TOP), HasWall(BOTTOM), HasWall(RIGHT), HasWall(LEFT) };
	}

	// (Local in-chunk position)
	public MazePos GetPosition() {
		return pos;
	}

	// Global position
	public MazePos GetGlobalPos() {
		return globalP;
	}

	public Vector3 GetWorldOffset() {
		return worldOffset;
	}

	public void SetWorldOffset(Vector3 worldOffset) {
		this.worldOffset = worldOffset;
	}

	public override int GetHashCode() {
		return pos.GetHashCode();
	}

}