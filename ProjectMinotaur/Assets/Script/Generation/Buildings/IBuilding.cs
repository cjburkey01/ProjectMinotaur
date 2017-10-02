using UnityEngine;

public class IBuilding : MonoBehaviour {

	public float generateChance = 0.005f;

	// Acceptable place for one? (Even if, in the end, it is not used).
	public virtual bool ForCell(MazeGenerate generator, int mazeX, int mazeY) {
		return true;
	}

	// Returns whether successful or not.
	public virtual bool OnGenerate(Vector2 position, MazeGenerate generator, int mazeX, int mazeY, int seed) {
		return false;
	}

}