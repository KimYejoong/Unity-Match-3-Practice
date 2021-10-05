using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
    public static MovePieces instance;

    private NodePiece _moving;
    private Point _newIndex;
    private Vector2 _mouseStart;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_moving == null)
            return;
        
        Vector2 dir = ((Vector2)Input.mousePosition - _mouseStart);
        Vector2 nDir = dir.normalized;
        Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

        _newIndex = Point.Clone(_moving.index);
        Point add = Point.Zero;
        if (dir.magnitude > 32) // if mouse goes more than 32pxs from starting point
        {
            // drag direction
            if (aDir.x > aDir.y)
                add = (new Point((nDir.x > 0) ? 1 : -1, 0));
            else if (aDir.y > aDir.x)
                add = (new Point(0, (nDir.y > 0) ? -1 : 1));
        }

        _newIndex.Add(add);

        Vector2 pos = Match3.Instance.GetPositionFromPoint(_moving.index);
        if (!_newIndex.Equals(_moving.index))
            pos += Point.Mult(new Point(add.x, -add.y), 16).ToVector();

        _moving.MovePositionTo(pos);
    }

    public void MovePiece(NodePiece piece)
    {
        if (_moving != null) return;
        _moving = piece;
        _mouseStart = Input.mousePosition;
    }

    public void DropPiece()
    {
        if (_moving == null) return;
        Debug.Log("Dropped");

        if (!_newIndex.Equals(_moving.index))
        {
            Match3.Instance.FlipPieces(_moving.index, _newIndex, true);
            Match3.Instance.moves--;

            if (Match3.Instance.moves == 0)
            {
                Debug.Log("Last Move, Game over");
                Match3.Instance.gameState = Match3.GameState.Closing;
                           
            }
        }
        else
            Match3.Instance.ResetPiece(_moving);
        
        _moving = null;
    }
}
