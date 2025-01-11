using System;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class TileGrid : MonoBehaviour
{
    public TileRow[] Rows { get; private set; }
    public TileCell[] Cells { get; private set; }

    public int Size => Cells.Length;
    public int Height => Rows.Length;
    public int Width => Size/Height;
    private void Awake()
    {
        Rows = GetComponentsInChildren<TileRow>();
        Cells = GetComponentsInChildren<TileCell>();
    }
    
    private void Start()
    {
        for (int y = 0; y < Rows.Length; y++)
        {
            for (int x = 0; x < Rows[y].Cells.Length; x++)
            {
                Rows[y].Cells[x].Coordinates = new Vector2Int(x, y);
            }
        }
    }

    public TileCell GetRandomEmptyCell()
    {
        int index = UnityEngine.Random.Range(0, Cells.Length);
        int startingIndex = index;
        
        
        while (Cells[index].Occupied)
        {
            index++;

            if(index >= Cells.Length)
            {
                index = 0;
            }
            
            if(index == startingIndex)
                return null;
        }
        
        return Cells[index];
    }
}
