using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectPool
{
    private List<Tile> _pooledObjects = new List<Tile>();
    private int _amountToPool = 20;
    
    private Tile _tilePrefab;
    private GameObject _objectPoolParent;
    
    [Inject]
    public ObjectPool(Tile tilePrefab, GameObject objectPoolParent)
    {
        _tilePrefab = tilePrefab;
        _objectPoolParent = objectPoolParent;
    }
    
    public Tile GetPooledTile()
    {
        for(int i = 0; i < _pooledObjects.Count; i++)
        {
            if(!_pooledObjects[i].gameObject.activeSelf)
            {
                return _pooledObjects[i];
            }
        }
        
        if(_pooledObjects.Count < _amountToPool)
        {
            Tile newTile = Object.Instantiate(_tilePrefab, _objectPoolParent.transform);
            _pooledObjects.Add(newTile);
            return newTile;
        }
        
        return null;
    }

    public void Reset()
    {
        _pooledObjects.Clear();
    }
}
