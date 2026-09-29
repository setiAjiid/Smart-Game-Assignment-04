using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //main func: for the sake of
    // 1. Pembuat grid
    // 2. Mengelola grid aka mengisi datanya, entah x, y, worldPosition, sama walkable atau ga
    // 3. Memberi data tetangga (GetNeighbors) + mereset nilai algoritma (ResetSearchData)
    // 4. Visualizer


    [Header("Grid")]
    public Vector2Int boardSize = new(10, 10);
    public float cellSize = 1f;

    [Header("Obstacle Detection")]
    public LayerMask obstacleMask;

    [Header("Visualization")]
    public bool ShowGrid = true;
    public float visualHeight = 0.05f;
    public Transform visualParent;
    

    public GridNode[,] grid { get; private set; }

    private void Awake()
    {
        CreateGrid();
    }

    public void CreateGrid()
    { 
        grid = new GridNode[boardSize.x, boardSize.y];

        GameObject holder = new("GridVisuals");
        holder.transform.SetParent(transform);
        visualParent = holder.transform;

        int cntWalkable = 0;
        for (int x = 0; x < boardSize.x; x++)
        {
            for (int y = 0; y < boardSize.y; y++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y);

                bool blocked = Physics.CheckBox( //pake physic box alih2 collider soalnya cuman mo dipanggil 1 kali frame pertama kali doang so ga berat
                    worldPosition + Vector3.up * 0.5f, //center box position ==> naikin posisi titik tengah asli shadow box ke atas 0.5f biar ga sejajar dengan lantainya that's why ditambah (0, 1, 0) * 0.5f   
                    new Vector3(cellSize * 0.4f, 0.45f, cellSize * 0.4f), //ukuran box
                    Quaternion.identity,  //rotasi == 0,0,0 aka tegak lurus
                    obstacleMask // layer yg mau di cek
                );
                bool walkable = !blocked;
                if (walkable) cntWalkable++;

                GridNode node = new(x, y, worldPosition, walkable);
                grid[x, y] = node;

                if (ShowGrid) CreateVisual(node);
            }
        }
        Debug.Log($"Jumlah walkable ada {cntWalkable} kotak.");
    }

    private void CreateVisual(GridNode node)
    {
        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tile.name = $"Node_{node.x}_{node.y}";
        tile.transform.SetParent(visualParent);
        tile.transform.position = node.worldPosition + Vector3.down * (visualHeight * 0.5f); //menurunkan center dari visual box agak ke bawah biar ga nonjol ke atas box nya, so it will be linear with the floor
        tile.transform.localScale = new Vector3(cellSize * 0.9f, visualHeight, cellSize * 0.9f);

        Collider col = tile.GetComponent<Collider>();
        if (col != null) Destroy(col);

        node.visual = tile; //simpan ref tile ke node biar bisa diwarnain
        SetNodeColor(node, node.walkable ? Color.white : Color.black);
    }

    public void SetNodeColor(GridNode node, Color color)
    {
        if (node.visual == null) return;
        Renderer renderer = node.visual.GetComponent<Renderer>();
        renderer.material.color = color;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return transform.position + new Vector3(cellSize * x, 0f, cellSize * y);
    }
}
