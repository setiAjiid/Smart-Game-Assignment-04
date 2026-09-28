using UnityEngine;

public class GridNode
{
    //main func: 1 kotak ni isinya perlu data apa saja? 
    public int x, y; //kotak nomor berapa?

    public Vector3 worldPosition; //posisi di dunia unity 3D world
    public bool walkable; //cek ada obstacle atau ga

    public int gCost; //biaya aseli dari start ke node yang mau dikunjungi (next node)
    public int hCost; //biaya perkiraan dari start ke node tujuan (goal node)

    public GridNode parent;
    public GameObject visual; //title scene untuk diwarnai

    public int FCost => gCost + hCost;

    public GridNode (int x, int y, Vector3 worldPosition, bool walkable)
    {
        this.x = x;
        this.y = y;
        this.worldPosition = worldPosition;
        this.walkable = walkable;

        gCost = int.MaxValue; //anggepannya infinity aka dia ga bisa kases ke goal
        hCost = 0;
        parent = null;
    }


}
