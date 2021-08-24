using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour
{
    public ArrayLayout boardLayout;

    [SerializeField]
    ScoreManager scoreManager;

    [SerializeField]
    TimeManager timeManager;

    [Header("UI Elements")]
    public Sprite[] pieces;
    public RectTransform gameBoard;
    public RectTransform killedBoard;    

    [Header("Prefabs")]
    public GameObject nodePiece;
    public GameObject killedPiece;

    [Header("Game Control")]
    [SerializeField]
    public float TimeLimit;
    [SerializeField]
    float TimeDelayBeforeHint;
    
    float TimeElapsed;
    float TimeStarted;

    float TimeWhenLastMatchHappened;
    float TimeElapsedFromLastMatch;

    int Combo;    

    public enum GAME_STATE
    {
        Ready = 0,
        Started,
        Closing,
        End
    }

    public GAME_STATE gameState = GAME_STATE.Ready;

    public int Moves = 15;
    int width = 9;
    int height = 12;
    int[] fills;
        
    Node[,] board;

    List<NodePiece> update;
    List<FlippedPieces> flipped;
    List<NodePiece> dead;
    List<KilledPiece> killed;

    System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }
    
    void StartGame()
    {
        fills = new int[width];
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        update = new List<NodePiece>();
        flipped = new List<FlippedPieces>();
        dead = new List<NodePiece>();
        killed = new List<KilledPiece>();

        InitalizeScore();
        InitalizeBoard();
        VerifyBoard();
        InstantiateBoard();
    }

    void InitalizeScore()
    {
        //TimeStarted = Time.time;
        //TimeElapsed = 0;
        TimeWhenLastMatchHappened = TimeStarted;
        TimeElapsedFromLastMatch = 0;

        gameState = GAME_STATE.Started;
        Combo = 0;
    }

    void InitalizeBoard()
    {
        board = new Node[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                board[x, y] = new Node((boardLayout.rows[y].row[x]) ? -1 : fillPiece(), new Point(x, y));
            }
        }
    }

    void VerifyBoard()
    {
        List<int> remove;
        for (int x = 0; x < width; x++ )
        {
            for (int y = 0; y < height; y++)
            {
                Point p = new Point(x, y);
                int val = getValueAtPoint(p);
                if (val <= 0) continue;

                remove = new List<int>();

                while (isConnected(p, true).Count > 0) // PROBLEM POINT
                {
                    val = getValueAtPoint(p);
                    if (!remove.Contains(val))
                        remove.Add(val);
                    setValueAtPoint(p, newValue(ref remove));
                }                
            }
        }
    }

    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = getNodeAtPoint(new Point(x, y));
                int val = board[x, y].value;
                if (val <= 0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece piece = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(32 + (64 * x), -32 - (64 * y));
                piece.Initialize(val, new Point(x, y), pieces[val - 1]);
                node.SetPiece(piece);
                
            }
        }
    }

    public void ResetPiece(NodePiece piece)
    {
        piece.ResetPosition();
        update.Add(piece);
    }

    public void FlipPieces(Point one, Point two, bool main) // main : true = actual flip, false = flip back because of no match found
    {
        if (getValueAtPoint(one) < 0) return;

        Node nodeOne = getNodeAtPoint(one);
        NodePiece pieceOne = nodeOne.getPiece();

        if (getValueAtPoint(two) > 0) // if there's actually sth to flip with
        {
            Node nodeTwo = getNodeAtPoint(two);
            NodePiece pieceTwo = nodeTwo.getPiece();
            nodeOne.SetPiece(pieceTwo);
            nodeTwo.SetPiece(pieceOne);

            if (main)
                // only when actual flip happens
                flipped.Add(new FlippedPieces(pieceOne, pieceTwo));                            

            update.Add(pieceOne);
            update.Add(pieceTwo);
        }
        else
        {
            ResetPiece(pieceOne);
        }      
        
    }

    void KillPiece(Point p)
    {
        List<KilledPiece> available = new List<KilledPiece>();
        for (int i = 0; i < killed.Count; i++)
        {
            if (!killed[i].falling)
                available.Add(killed[i]);

            KilledPiece set = null;
            if (available.Count > 0)
                set = available[0];
            else
            {
                GameObject kill = GameObject.Instantiate(killedPiece, killedBoard);
                KilledPiece kPiece = kill.GetComponent<KilledPiece>();
                set = kPiece;
                killed.Add(kPiece);
            }

            int val = getValueAtPoint(p) - 1;
            if (set != null && val >= 0 && val < pieces.Length)
                set.Initialize(pieces[val], getPoistionFromPoint(p));
        }
    }

    List<Point> isConnected(Point p, bool main)
    {

        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(p);
        Point[] directions =
        {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };

        foreach (Point dir in directions) //Checking if there is 2 or more same shapes in the directions
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for (int i = 1; i < 3; i++)
            {
                Point check = Point.add(p, Point.mult(dir, i));
                if (getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1) //If there are more than 1 of the same shape in the direction then we know it is a match
                AddPoints(ref connected, line); //Add these points to the overarching connected list
        }

        for (int i = 0; i < 2; i++) //Checking if we are in the middle of two of the same shapes
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[i + 2]) };
            foreach (Point next in check) //Check both sides of the piece, if they are the same value, add them to the list
            {
                if (getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
                AddPoints(ref connected, line);
        }

        for (int i = 0; i < 4; i++) //Check for a 2x2
        {
            List<Point> square = new List<Point>();

            int same = 0;
            int next = i + 1;
            if (next >= 4)
                next -= 4;

            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[next]), Point.add(p, Point.add(directions[i], directions[next])) };
            foreach (Point pnt in check) //Check all sides of the piece, if they are the same value, add them to the list
            {
                if (getValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }

            if (same > 2)
                AddPoints(ref connected, square);
        }

        if (main) //Checks for other matches along the current match
        {
            for (int i = 0; i < connected.Count; i++)
                AddPoints(ref connected, isConnected(connected[i], false));
        }        

        return connected;

        /*
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(p);
        Point[] directions =
        {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };

        foreach (Point dir in directions) // Check if there's 2 or more same shapes in the directions
        {
            List<Point> line = new List<Point>();

            int same = 0;

            for (int i = 0; i < 3; i++)
            {
                Point check = Point.add(p, Point.mult(dir, i));
                if (getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1) // if there're more than 1 of the same => Match!
                AddPoints(ref connected, line);
        }

        for (int i = 0; i < 2; i++) // What if it's in the middle?
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[i + 2]) };

            foreach (Point next in check) // check one side and the opposite side
            {
                if (getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
                AddPoints(ref connected, line);
        }

        for (int i = 0; i < 4; i++) // check if it's in 2x2 square case
        {
            List<Point> square = new List<Point>();

            int same = 0;
            int next = i + 1;
            if (next >= 4)
                next -= 4; // make it cycle

            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[next]), Point.add(p, Point.add(directions[i], directions[next])) }; // check forward, side, and diagonal side

            foreach (Point pnt in check) // check one side and the opposite side
            {
                if (getValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }

            if (same > 2)
                AddPoints(ref connected, square);
        }

        if (main)
        {
            for (int i = 0; i < connected.Count; i++)
            {
                AddPoints(ref connected, isConnected(connected[i], false)); // call again like recursive but it's not called more than once since its "main" mark is set to be false.
            }
        }

        if (connected.Count > 0)
            connected.Add(p);

        return connected;
        */
    }
    
    void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach(Point p in add)
        {
            bool doAdd = true;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }

            if (doAdd)
                points.Add(p);
        }
    }

    int getValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height)
            return -1;
        return board[p.x, p.y].value;
    }

    void setValueAtPoint(Point p, int v)
    {
        board[p.x, p.y].value = v;
    }

    Node getNodeAtPoint(Point p)
    {
        return board[p.x, p.y];
    }

    int newValue(ref List<int> remove)
    {
        List<int> available = new List<int>();
        for (int i = 0; i < pieces.Length; i++)
        {
            available.Add(i + 1);
        }

        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0)
            return 0;

        return available[random.Next(0, available.Count)];
    }

    int fillPiece()
    {
        int val = 1;
        val = (random.Next(0, pieces.Length)) + 1;
        return val;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GAME_STATE.Started) {
            //TimeElapsed = Time.time - TimeStarted;                        
            //timeManager.SetTime(TimeLimit - TimeElapsed, TimeLimit);

            TimeElapsedFromLastMatch = Time.time - TimeWhenLastMatchHappened; // Time check before giving hint

            if (TimeElapsedFromLastMatch >= TimeDelayBeforeHint)
            {
                ShowHint();
            }

        }

        List<NodePiece> finishedUpdating = new List<NodePiece>();


        for (int i = 0; i < update.Count; i++)
        {
            NodePiece piece = update[i];
            if (!piece.UpdatePiece()) finishedUpdating.Add(piece);            
        }

        for (int i = 0; i < finishedUpdating.Count; i++)
        {
            NodePiece piece = finishedUpdating[i];
            FlippedPieces flip = getFlipped(piece);
            NodePiece flippedPiece = null;

            List<Point> connected = isConnected(piece.index, true);
            bool wasFlipped = (flip != null);

            int x = (int)piece.index.x;
            fills[x] = Mathf.Clamp(fills[x] - 1, 0, width);

            if (wasFlipped) { 
                flippedPiece = flip.getOtherPiece(piece);
                AddPoints(ref connected, isConnected(flippedPiece.index, true)); // to check if flipped pieces also matches
            }

            if (connected.Count == 0) // case : no match at all
            {
                if (wasFlipped) // if we "flipped" to make this update
                {
                    FlipPieces(piece.index, flippedPiece.index, false); // flip it back(main == false)
                    Combo = 0; // Reset Combo count                    
                    scoreManager.UpdateCombo(Combo, Moves);
                    
                }
            }
            else // case : match happens
            {
                foreach(Point pnt in connected) // remove the node pieces
                {
                    KillPiece(pnt);
                    Node node = getNodeAtPoint(pnt);
                    NodePiece nodePiece = node.getPiece();
                    if (nodePiece != null)
                    {
                        nodePiece.gameObject.SetActive(false);
                        dead.Add(nodePiece);
                    }

                    node.SetPiece(null);                    
                }

                scoreManager.AddPoint(CalcScore(Combo)); // Add Points
                Debug.Log(Combo);
                Combo++;                

                scoreManager.UpdateCombo(Combo, Moves);

                TimeWhenLastMatchHappened = Time.time;
                TimeElapsedFromLastMatch = 0;                

                ApplyGravityToBoard();
            }
            
            flipped.Remove(flip); // remove the flip after update
            update.Remove(piece);

            if (gameState == GAME_STATE.Closing)
            {
                Debug.Log("Game Closing");
                timeManager.GameEnd();
                gameState = GAME_STATE.End;
            }
            //if (TimeElapsed >= TimeLimit) // Time over, game set
        }
    }

    int CalcScore(int combo)
    {
        return (Mathf.Max(8, combo) + 1) * 5;
    }

    void ApplyGravityToBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = (height - 1); y >= 0;y--) // from bottom to top
            {
                Point p = new Point(x, y);
                Node node = getNodeAtPoint(p);
                int val = getValueAtPoint(p);

                if (val != 0) // if it is filled with sth, then just continue
                    continue;

                for (int ny = (y - 1); ny >= -1; ny--)
                {
                    Point next = new Point(x, ny);
                    int nextVal = getValueAtPoint(next);
                    if (nextVal == 0)
                        continue;

                    if (nextVal != -1) // hit mid-redcell
                    {
                        Node got = getNodeAtPoint(next);
                        NodePiece piece = got.getPiece();

                        node.SetPiece(piece);
                        update.Add(piece);

                        got.SetPiece(null);
                    }
                    else // hit end
                    {
                        NodePiece piece;
                        Point fallPoint = new Point(x, (-1 - fills[x]));

                        int newVal = fillPiece();
                        if (dead.Count > 0)
                        {
                            NodePiece revived = dead[0];
                            revived.gameObject.SetActive(true);
                            revived.rect.anchoredPosition = getPoistionFromPoint(fallPoint);
                            piece = revived;

                            
                            dead.RemoveAt(0);
                        }
                        else // just in case
                        {
                            GameObject obj = Instantiate(nodePiece, gameBoard);
                            NodePiece n = obj.GetComponent<NodePiece>();
                            RectTransform rect = obj.GetComponent<RectTransform>();
                            rect.anchoredPosition = getPoistionFromPoint(fallPoint);
                            piece = n;
                        }

                        piece.Initialize(newVal, p, pieces[newVal - 1]);

                        Node hole = getNodeAtPoint(p);
                        hole.SetPiece(piece);
                        ResetPiece(piece);
                        fills[x]++;
                    }
                    
                    break;
                }
            }

        }
    }

    FlippedPieces getFlipped(NodePiece p)
    {
        FlippedPieces flip = null;
        for (int i = 0; i < flipped.Count; i++)
        {
            if (flipped[i].getOtherPiece(p) != null)
            {
                flip = flipped[i];
                break;
            }                
        }
        return flip;
    }

    string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdeghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
        for (int i = 0; i < 20; i++)
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        return seed;
    }

    public Vector2 getPoistionFromPoint(Point p)
    {
        return new Vector2(32 + (64 * p.x), -32 - (64 * p.y));
    }

    void ShowHint()
    {
        Debug.Log("Hint!");
    }

}

[System.Serializable]
public class Node
{
    public int value; // 0 == Blank, 1 == Cube, 2 == Sphere, 3 == Cylinder, 4 == Pyramid, 5 == Diamond, -1 == Hole;
    public Point index;
    NodePiece piece;

    public Node(int v, Point i)
    {
        value = v;
        index = i;
    }

    public void SetPiece(NodePiece p)
    {
        piece = p;
        value = (piece == null) ? 0 : piece.value;
        if (piece == null) return;
        piece.SetIndex(index);
    }

    public NodePiece getPiece()
    {
        return piece;
    }
}

[System.Serializable]
public class FlippedPieces
{
    public NodePiece one;
    public NodePiece two;

    public FlippedPieces(NodePiece o, NodePiece t)
    {
        one = o;
        two = t;
    }

    public NodePiece getOtherPiece(NodePiece p)
    {
        if (p == one)
            return two;
        else if (p == two)
            return one;
        else
            return null;
    }
}