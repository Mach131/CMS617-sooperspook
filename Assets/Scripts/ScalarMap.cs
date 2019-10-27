using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General-purpose spatial mapping, i.e. a scalar field.
/// </summary>
/// <remarks>
/// This is represented as a discretized scalar field in 3D space, projected
/// onto a 2D plane. In the future, this will probably change to a 3D grid
/// or multiple levels.
/// </remarks>
abstract public class ScalarMap : MonoBehaviour
{
	/// <summary>
	/// Size of each cell in the grid.
	/// </summary>
	public static float cellSize = 10.0f;

	/// <summary>
	/// Number of cells per direction in the grid.
	/// </summary>
	/// <remarks>
	/// Used for auto-generation only.
	/// </remarks>
	public static float gridFactor = 10.0f;

	/// <summary>
	/// Nominal region spanned by this map.
	/// </summary>
	/// <remarks>
	/// Will not work as expected if update from outside this class. This is not
	/// the intention, and should work. This is because bounds are only used 
	/// during awake, and ignored otherwise.
	/// </remarks>
	public static Rect bounds = new Rect(
		-gridFactor * cellSize,
		-gridFactor * cellSize,
		2 * gridFactor * cellSize,
		2 * gridFactor * cellSize);

	/// <summary>
	/// Number of cells along the x and y directions, respectively.
	/// </summary>
	private Vector2Int size;
	
	/// <summary>
	/// Store for grid values.
	/// </summary>
	private float[,] heatmap;

	/// <summary>
	/// Width and height of each grid cell.
	/// </summary>
	public static float CellSize {
		get => cellSize;
		set => cellSize = value;
	}

	/// <summary>
	/// Width and height of the grid as x and y, respectively.
	/// </summary>
	public Vector2Int GridSize {
		get { return size; }
	}

	void Awake() {
		int numXCells = Mathf.CeilToInt((bounds.xMax - bounds.xMin) / cellSize);
		int numYCells = Mathf.CeilToInt((bounds.yMax - bounds.yMin) / cellSize);
		size = new Vector2Int(numXCells, numYCells);
		heatmap = new float[numXCells, numYCells];
		foreach( Vector2Int cell in CellEnumerable() ) {
			Vector2 pos = CellCenter2(cell);
			SetAt(cell, 1);
		}
	}

	/// <summary>
	/// Iterate through all cells in the grid.
	/// </summary>
	/// <returns>
	/// Each cell in the grid, in a consistent sequence between invokations.
	/// </returns>
	public IEnumerable<Vector2Int> CellEnumerable() {
		for( int x = 0; x < size.x; x++ ) {
			for( int y = 0; y < size.y; y++ ) {
				yield return new Vector2Int(x, y);
			}
		}
	}

	/// <summary>
	/// Iterate through all values in the grid.
	/// </summary>
	/// <returns>
	/// Each value in the grid, in a consistent sequence between invokations.
	/// </returns>
	public IEnumerable<float> ValueEnumerable() {
		foreach( Vector2Int cell in CellEnumerable() ) {
			yield return ValueAt(cell);
		}
	}

	/// <summary>
	/// Get the grid cell cooresponding to a given position.
	/// </summary>
	/// <param name="position">Position to fetch the grid cell for.</param>
	/// <returns>
	/// The grid cell containing the input position, as an (x,y) coordinate pair.
	/// </returns>
	public Vector2Int Quantize( Vector2 position ) {
		float xf = Mathf.Floor((position.x - bounds.xMin) / cellSize);
		float yf = Mathf.Floor((position.y - bounds.yMin) / cellSize);

		int x = (int) Mathf.Clamp(xf, 0, GridSize.x-1);
		int y = (int) Mathf.Clamp(yf, 0, GridSize.y-1);

		return new Vector2Int(x, y);
	}

	/// <summary>
	/// Get the position of the input cell's geometric center.
	/// </summary>
	/// <param name="cell">Cell to fetch the position for.</param>
	/// <returns>
	/// Position of the input cell's centroid in 2D space.
	/// </returns>
	public Vector2 CellCenter2( Vector2Int cell ) {
		float x = (cell.x + .5f) * cellSize + bounds.xMin;
		float y = (cell.y + .5f) * cellSize + bounds.yMin;
		return new Vector2(x, y);
	}

	/// <summary>
	/// Get the position of the input cell's geometric center.
	/// </summary>
	/// <param name="cell">Cell to fetch the position for.</param>
	/// <returns>
	/// Position of the input cell's centroid in 3D space.
	/// </returns>
	public Vector3 CellCenter3( Vector2Int cell ) {
		float x = (cell.x + .5f) * cellSize + bounds.xMin;
		float y = (cell.y + .5f) * cellSize + bounds.yMin;
		return ProjectTo3(new Vector2(x, y));
	}

	/// <summary>
	/// The maximal value within the grid.
	/// </summary>
	/// <returns>
	/// The maximal value within the grid.
	/// </returns>
	public float Max() {
		float result = float.NegativeInfinity;
		foreach( float v in ValueEnumerable() ) {
			result = v > result ? v : result;
		}
		return result;
	}

	/// <summary>
	/// Compute the finite difference via the central difference form.
	/// </summary>
	/// <remarks>
	/// Otherwise known as a discrete/finite gradient.
	/// </remarks>
	/// <see cref="https://en.wikipedia.org/wiki/Finite_difference#Forward,_backward,_and_central_differences"/>
	/// <param name="position">Position to evaluate the finite difference at.</param>
	/// <param name="h">Finite width parameter.</param>
	/// <returns>The central difference of the input position, parameterized by 'h'.</returns>
	public Vector2 CentralDifference( Vector2 position, float h ) {
		float hOver2 = 0.5f * h;

		Vector2 xPos = position + hOver2 * Vector2.right;
		Vector2 xNeg = position - hOver2 * Vector2.right;
		Vector2 yPos = position + hOver2 * Vector2.up;
		Vector2 yNeg = position - hOver2 * Vector2.up;

		float x = ValueAt(xPos) - ValueAt(xNeg);
		float y = ValueAt(yPos) - ValueAt(yNeg);

		return new Vector2(x, y);
	}

	/// <summary>
	/// Project the input 3-vector into 2D space.
	/// </summary>
	/// <param name="vec">Vector in 3D space.</param>
	/// <returns>Vector in 2D space consistent with the input vector.</returns>
	private static Vector2 ProjectTo2( Vector3 vec ) {
		return new Vector2(vec.x, vec.z);
	}

	/// <summary>
	/// Project the input 2-vector into 3D space.
	/// </summary>
	/// <param name="vec">Vector in 2D space.</param>
	/// <returns>Vector in 3D space consistent with the input vector.</returns>
	private static Vector3 ProjectTo3( Vector2 vec ) {
		return new Vector3(vec.x, 0, vec.y);
	}

	/// <summary>
	/// Approximate gradient operator in 2D space.
	/// </summary>
	/// <param name="position">Position to take the gradient at.</param>
	/// <returns>The gradient vector in 2D space, evaluated at the input 
	/// position.</returns>
	public Vector2 Gradient2( Vector2 position ) {
		const float cellFactor = 0.5f;
		float h = cellSize * cellFactor;
		return CentralDifference(position, h) / h;
	}

	/// <summary>
	/// Approximate gradient operator in 3D space.
	/// </summary>
	/// <param name="position">Position to take the gradient at.</param>
	/// <returns>The gradient vector in 3D space, evaluated at the input 
	/// position.</returns>
	public Vector3 Gradient3( Vector3 position ) {
		return ProjectTo3(Gradient2(ProjectTo2(position)));
	}

	/// <summary>
	/// Get the value of the input position via bilinear interpolation.
	/// </summary>
	/// <see cref="https://en.wikipedia.org/wiki/Bilinear_interpolation"/>
	/// <param name="position">Position in 3D space.</param>
	/// <returns>
	/// The value of the input position via bilinear interpolation.
	/// </returns>
	public float ValueAt( Vector3 position )
		=> ValueAt(new Vector2(position.x, position.z));

	/// <summary>
	/// Get the value of the input position via bilinear interpolation.
	/// </summary>
	/// <see cref="https://en.wikipedia.org/wiki/Bilinear_interpolation"/>
	/// <param name="position">Position in 2D space.</param>
	/// <returns>
	/// The value of the input position via bilinear interpolation.
	/// </returns>
	public float ValueAt( Vector2 position ) {
		Vector2Int localCell = Quantize(position);

		float f00 = ValueAt(localCell);
		float f01 = ValueAt(localCell.x, localCell.y + 1);
		float f10 = ValueAt(localCell.x + 1, localCell.y);
		float f11 = ValueAt(localCell.x + 1, localCell.y + 1);

		float a00 = f00;
		float a10 = f10 - f00;
		float a01 = f01 - f00;
		float a11 = f11 + f00 - f10 - f01;

		Vector2 p_0 = CellCenter2(localCell);
		float alpha = (position.x - p_0.x) / cellSize;
		float beta  = (position.y - p_0.y) / cellSize;

		return a00 + a10 * alpha + a01 * beta + a11 * alpha * beta;
	}

	/// <summary>
	/// Set the grid value closest to input position.
	/// </summary>
	/// <remarks>
	/// NOTE!!!
	/// This does not perform anything resembling interpolation; only the
	/// cell containing the input position will be updated.
	/// </remarks>
	/// <param name="position">Position in 2D space.</param>
	/// <param name="newValue">New value.</param>
	public void SetAt( Vector2 position, float newValue ) => SetAt(Quantize(position), newValue);

	/// <summary>
	/// Get the value of the input grid cell.
	/// </summary>
	/// <param name="cell">Grid cell to fetch value of.</param>
	/// <returns>The value of the input grid cell.</returns>
	public float ValueAt( Vector2Int cell ) => ValueAt(cell.x, cell.y);

	/// <summary>
	/// Set the grid value of the input cell.
	/// </summary>
	/// <param name="cell">Grid cell to update.</param>
	/// <param name="newValue">New value.</param>
	public void SetAt( Vector2Int cell, float newValue ) => heatmap[cell.x, cell.y] = newValue;

	/// <summary>
	/// Get the value of the input grid cell.
	/// </summary>
	/// <param name="x">Grid cell x-coordinate</param>
	/// <param name="y">Grid cell y-coordinate</param>
	/// <returns>The value of the input grid cell.</returns>
	public float ValueAt( int x, int y ) {
		int clampedX = (int) Mathf.Clamp(x, 0, GridSize.x-1);
		int clampedY = (int) Mathf.Clamp(y, 0, GridSize.y-1);
		return heatmap[clampedX, clampedY];
	}
}
