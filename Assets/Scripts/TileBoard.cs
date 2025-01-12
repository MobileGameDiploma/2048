using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class TileBoard : MonoBehaviour
{
    private GameManager _gameManager;
    private Tile _tilePrefab;
    private TileState[] _tileStates;
    
    private TileGrid _grid;
    private List<Tile> _tiles = new List<Tile>(16);
    private bool _waiting = false;
    private ObjectPool _objectPool;
    private MainControls _mainControls;

    [Inject]
    public void Construct(TileGrid grid, GameManager gameManager, Tile tilePrefab, TileState[] states, ObjectPool objectPool)
    {
        _gameManager = gameManager;
        _tilePrefab = tilePrefab;
        _grid = grid;
        _tileStates = states;
        _objectPool = objectPool;
        _mainControls = new MainControls();
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
        Tile tile = _objectPool.GetPooledTile();
        tile.SetState(_tileStates[0]);
        tile.Spawn(_grid.GetRandomEmptyCell());
        _tiles.Add(tile);
    }

    #region Controls

    private void OnMovingUp()
    {
        MoveTiles(Direction.Up, 0, 1, 1, 1);
    }
    
    private void OnMovingDown()
    {
        MoveTiles(Direction.Down, 0, 1, _grid.Height - 2, -1);
    }
    
    private void OnMovingLeft()
    {
        MoveTiles(Direction.Left, 1, 1, 0, 1);
    }
    
    private void OnMovingRight()
    {
        MoveTiles(Direction.Right, _grid.Width - 2, -1, 0, 1);
    }

    private void OnExit()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    #endregion

    private void MoveTiles(Direction direction, int startX, int incrementX, int startY, int incrementY)
    {
        if(!_waiting)
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
        int index = Mathf.Clamp(IndexOf(b.State) + 1, 0, _tileStates.Length - 1);
        
        b.SetState(_tileStates[index]);
        
        _gameManager.IncreaseScore(b.Cell.Tile.State.Number);
    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < _tileStates.Length; i++)
        {
            if(state == _tileStates[i]) return i;
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
            _gameManager.GameOver();
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
