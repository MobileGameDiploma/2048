using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileState State { get; private set; }
    public TileCell Cell { get; private set; }

    private Image _background;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _background = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetState(TileState state)
    {
        this.State = state;
        _background.color = state.BackgroundColor;
        _text.color = state.TextColor;
        _text.text = state.Number.ToString();
    }

    public void Spawn(TileCell cell)
    {
        if (this.Cell != null)
        {
            this.Cell.Tile = null;
        }
        
        this.Cell = cell;
        this.Cell.Tile = this;
        transform.position = cell.transform.position;
    }
}
