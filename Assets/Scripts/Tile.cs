using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileState State { get; private set; }
    public TileCell Cell { get; private set; }
    public bool locked { get; set; }

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
        this.gameObject.SetActive(true);
    }

    public void MoveTo(TileCell cell)
    {
        if (this.Cell != null)
        {
            this.Cell.Tile = null;
        }
        
        this.Cell = cell;
        this.Cell.Tile = this;
        
        StartCoroutine(Animate(cell.transform.position, false));
    }

    public void Merge(TileCell cell)
    {
        if (this.Cell != null)
        {
            this.Cell.Tile = null;
        }
        
        this.Cell = null;
        cell.Tile.locked = true;
        
        StartCoroutine(Animate(cell.transform.position, true));
    }

    private IEnumerator Animate(Vector3 to, bool merging)
    {
        float elapsed = 0f;
        float duration = 0.1f;
        
        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = to;

        if (merging) gameObject.SetActive(false);
    }
}
