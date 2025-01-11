using System;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public Tile TilePrefab;
    public TileState[] TileStates;
    
    private TileGrid _grid;
    private List<Tile> _tiles;


    private void Awake()
    {
        _grid = GetComponentInChildren<TileGrid>();
        _tiles = new List<Tile>(16);
    }

    private void Start()
    {
        CreateTile();
        CreateTile();
    }

    private void CreateTile()
    {
        Tile tile = Instantiate(TilePrefab, _grid.transform);
        tile.SetState(TileStates[0]);
        tile.Spawn(_grid.GetRandomEmptyCell());
        _tiles.Add(tile);
    }
}
