using UnityEngine;
using System.Collections.Generic;

public class Tileset : MonoBehaviour {

	public int TILE_TEXW;
	public int TILE_TEXH;
	public float TILE_SIZE;

	public int[,] MAP = new int[,]{
		{1, 1, 1, 1, 3, 3, 3, 3},
		{1, 2, 2, 1, 3, 3, 3, 3},
		{1, 2, 2, 1, 3, 3, 3, 3},
		{1, 1, 1 ,1, 3, 3, 3, 3},
		{0, 0, 0, 0, 4, 4, 4, 4},
		{0, 0, 0, 0, 4, 0, 0, 4},
		{0, 0, 0, 0, 4, 0, 0, 4},
		{0, 0, 0, 0, 4, 4, 4, 4}};

	private int mapW, mapH;

	private PolygonCollider2D myPolygon;
	private EdgeCollider2D myEdge;

	private List<Vector3> vertices;
	private List<int> triangles;
	private List<Vector2> uv;

	private float ufrac, vfrac;

	// Use this for initialization
	void Start () {
		mapW = MAP.GetLength (0);
		mapH = MAP.GetLength (1);

		myPolygon = gameObject.AddComponent<PolygonCollider2D>();
		myPolygon.pathCount = 0;

		myEdge = gameObject.AddComponent<EdgeCollider2D>();
		myEdge.points = new Vector2[]{
			new Vector2(0, 0),
			new Vector2(mapW * TILE_SIZE, 0),
			new Vector2(mapW * TILE_SIZE, mapH * TILE_SIZE),
			new Vector2(0, mapH * TILE_SIZE),
			new Vector2(0, 0)
		};

		vertices = new List<Vector3> ();
		triangles = new List<int> ();
		uv = new List<Vector2> ();

		ufrac = 1.0f / TILE_TEXW;
		vfrac = 1.0f / TILE_TEXH;

		createTilemap ();
	}

	private void createTilemap() {
		// Clear lists
		vertices.Clear ();
		triangles.Clear ();
		uv.Clear ();

		// Create tiles
		for (int j = 0; j < mapW; j++)
			for (int i = 0; i < mapH; i++) {
				if (MAP [j, i] > 0) {
					createTile (i, j, MAP [j, i] - 1);
				} else
					createCollider(i, j);
					
			}

		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();
		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.uv = uv.ToArray ();
		mesh.Optimize ();
		mesh.RecalculateNormals ();
	}

	private void createTile(int X, int Y, int tile) {
		int vert_st = vertices.Count;

		vertices.AddRange (new Vector3[] {
			new Vector3(X * TILE_SIZE , Y * TILE_SIZE, 0.0f),
			new Vector3((X+1) * TILE_SIZE, Y * TILE_SIZE , 0.0f),
			new Vector3((X+1) * TILE_SIZE, (Y+1) * TILE_SIZE, 0.0f),
			new Vector3(X * TILE_SIZE, (Y+1) * TILE_SIZE, 0.0f)
		});

		triangles.AddRange (new int[] { vert_st + 2, vert_st + 1, vert_st, vert_st + 3, vert_st + 2, vert_st});

		float u = (float)(tile % TILE_TEXW) / TILE_TEXW;
		float v = Mathf.Floor ((float)tile / TILE_TEXW) / TILE_TEXH;

		uv.AddRange (new Vector2[] {
			new Vector2 (u, v),
			new Vector2 (u + ufrac, v),
			new Vector2(u + ufrac, v + vfrac),
			new Vector2 (u, v + vfrac)
		});
	}

	private void createCollider(int X, int Y) {
		int index = myPolygon.pathCount;
		myPolygon.pathCount++;

		Vector2[] points = new Vector2[]
		{
			new Vector2(X*TILE_SIZE, Y*TILE_SIZE),
			new Vector2((X+1)*TILE_SIZE, Y*TILE_SIZE),
			new Vector2((X+1)*TILE_SIZE, (Y+1)*TILE_SIZE),
			new Vector2(X*TILE_SIZE, (Y+1)*TILE_SIZE)
		};
		myPolygon.SetPath (index, points);


	}
}
