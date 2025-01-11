using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileState State { get; private set; }
    public TileCell Cell { get; private set; }
    public int Number { get; private set; }
}
