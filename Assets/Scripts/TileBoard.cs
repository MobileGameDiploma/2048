using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public GameManager GameManager;
    public Tile TilePrefab;
    public TileState[] TileStates;
    
    private TileGrid _grid;
    private List<Tile> _tiles;
    private bool _waiting = false;

    private void Awake()
    {
        _grid = GetComponentInChildren<TileGrid>();
        _tiles = new List<Tile>(16);
    }

    public void ClearBoard()
    {
        foreach (TileCell cell in _grid.Cells)
        {
            cell.Tile = null;
        }
        
        foreach (Tile tile in _tiles)
        {
            Destroy(tile.gameObject);
        }
        _tiles.Clear();
    }
    
    public void CreateTile()
    {
        Tile tile = Instantiate(TilePrefab, _grid.transform);
        tile.SetState(TileStates[0]);
        tile.Spawn(_grid.GetRandomEmptyCell());
        _tiles.Add(tile);
    }

    private void Update()
    {
        if (!_waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(Direction.Up, 0, 1, 1, 1);
            }
            else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Direction.Down, 0, 1, _grid.Height - 2, -1);
            }
            else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Direction.Left, 1, 1, 0, 1);
            }
            else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Direction.Right, _grid.Width - 2, -1, 0, 1);
            }
        }
    }

    private void MoveTiles(Direction direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        for (int x = startX; x >= 0 && x < _grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < _grid.Height; y += incrementY)
            {
                TileCell cell = _grid.GetCell(x, y);

                if (cell.Occupied)
                {
                    changed |= MoveTile(cell.Tile, direction);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile,Direction direction)
    {
        TileCell newCell = null;
        TileCell adjacent = _grid.GetAdjacentCell(tile.Cell, direction);

        while (adjacent != null)
        {
            if (adjacent.Occupied)
            {
                if (CanMerge(tile, adjacent.Tile))
                {
                    Merge(tile, adjacent.Tile);
                    return true;
                }
                break;
            }
            
            newCell = adjacent;
            adjacent = _grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }
        return false;
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.State == b.State && !b.locked;
    }

    private void Merge(Tile a, Tile b)
    {
        _tiles.Remove(a);
        a.Merge(b.Cell);
        int index = Mathf.Clamp(IndexOf(b.State) + 1, 0, TileStates.Length - 1);
        
        b.SetState(TileStates[index]);
        
        GameManager.IncreaseScore(b.Cell.Tile.State.Number);
    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < TileStates.Length; i++)
        {
            if(state == TileStates[i]) return i;
        }
        
        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        _waiting = true;
        
        yield return new WaitForSeconds(0.1f);
        
        _waiting = false;

        foreach (Tile tile in _tiles)
        {
            tile.locked = false;
        }
        
        if(_tiles.Count != _grid.Size)
            CreateTile();
        if (CheckForGameOver())
        {
            GameManager.GameOver();
        }
    }

    private bool CheckForGameOver()
    {
        if(_tiles.Count != _grid.Size)
            return false;

        foreach (Tile tile in _tiles)
        {
            TileCell up = _grid.GetAdjacentCell(tile.Cell, Direction.Up);
            TileCell down = _grid.GetAdjacentCell(tile.Cell, Direction.Down);
            TileCell left = _grid.GetAdjacentCell(tile.Cell, Direction.Left);
            TileCell right = _grid.GetAdjacentCell(tile.Cell, Direction.Right);
            
            if(up != null && CanMerge(tile, up.Tile)) return false;
            if(down != null && CanMerge(tile, down.Tile)) return false;
            if(left != null && CanMerge(tile, left.Tile)) return false;
            if(right != null && CanMerge(tile, right.Tile)) return false;
        }
        
        return true;
    }
}
