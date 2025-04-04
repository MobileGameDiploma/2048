using System;
using System.Drawing;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = System.Random;

public class TileGrid : MonoBehaviour
{
    public TileRow[] Rows { get; private set; }
    public TileCell[] Cells { get; private set; }

    public int Size => Cells.Length;
    public int Height => Rows.Length;
    public int Width => Size/Height;

    [Inject]
    public void Construct(TileRow[] rows, TileCell[] cells)
    {
        Rows = rows;
        Cells = cells;
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

    public TileCell GetCell(int x, int y)
    {
        if(x>=0 && x<Width && y>=0 && y<Height)
            return Rows[y].Cells[x];
        else 
            return null;
    }
    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }
    
    public TileCell GetAdjacentCell(TileCell cell, Direction direction)
    {
        Vector2Int coordinates = cell.Coordinates;

        switch (direction)
        {
            case Direction.Up:
                coordinates.y--;
                break;
            case Direction.Down:
                coordinates.y++;
                break;
            case Direction.Left:
                coordinates.x--;
                break;
            case Direction.Right:
                coordinates.x++;
                break;
        }

        return GetCell(coordinates);
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
