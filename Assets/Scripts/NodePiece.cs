﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public int value;
    public Point index;
    Match3 game;

    [HideInInspector]
    public Vector2 pos;

    [HideInInspector]
    public RectTransform rect;

    bool updating;
    Image img;

    public void Initialize(int v, Point p, Sprite piece)
    {        
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        game = FindObjectOfType<Match3>();

        value = v;
        SetIndex(p);
        img.sprite = piece;
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

    public void MovePosition(Vector2 move)
    {
        rect.anchoredPosition += move * Time.deltaTime * 16f;
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
            updating = true;
            return true;
        }
        else
        {
            rect.anchoredPosition = pos;
            updating = false;
            return false;
        }
    }

    void UpdateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (game.gameState != Match3.GAME_STATE.Started)
            return;

        if (updating)
            return;

        MovePieces.instance.MovePiece(this);   
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (game.gameState != Match3.GAME_STATE.Started)
            return;

        MovePieces.instance.DropPiece();
    }
}
