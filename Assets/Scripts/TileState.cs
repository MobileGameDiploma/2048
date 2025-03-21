
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/TileState", fileName = "New Tile Script")]
public class TileState : ScriptableObject
{
    public int Number;
    public Color BackgroundColor;
    public Color TextColor;
}
