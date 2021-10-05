using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public int value;
    public Point index;
    private Match3 _game;

    [HideInInspector]
    public Vector2 pos;

    [HideInInspector]
    public RectTransform rect;

    private bool _updating;
    private Image _img;

    public void Initialize(int v, Point p, Sprite piece)
    {        
        _img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        _game = FindObjectOfType<Match3>();

        value = v;
        SetIndex(p);
        _img.sprite = piece;
    }


    public void SetIndex(Point p)
    {
        index = p;
        ResetPosition();
        UpdateName();
    }

    public void ResetPosition()
    {
        pos = new Vector2(32 + (64 * index.x), -32 - (64 * index.y));
    }

    public void MovePositionTo(Vector2 move)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 16f);
    }

    public bool UpdatePiece()
    {
        if (Vector2.Distance(rect.anchoredPosition, pos) > 1)
        {
            MovePositionTo(pos);
            _updating = true;
            return true;
        }
        else
        {
            rect.anchoredPosition = pos;
            _updating = false;
            return false;
        }
    }

    private void UpdateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_game.gameState != Match3.GameState.Started)
            return;

        if (_updating)
            return;

        MovePieces.instance.MovePiece(this);   
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_game.gameState != Match3.GameState.Started)
            return;

        MovePieces.instance.DropPiece();
    }
}
