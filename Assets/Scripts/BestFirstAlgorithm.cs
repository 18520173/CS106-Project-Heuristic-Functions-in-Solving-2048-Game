using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestFirstAlgorithm : MonoBehaviour
{
    private GameManager gm;

    private List<int[]> left = new List<int[]>();
    private List<int[]> right = new List<int[]>();
    private List<int[]> up = new List<int[]>();
    private List<int[]> down = new List<int[]>();

    private List<bool[]> mergeThisTurn = new List<bool[]>();

    private List<int[]> rowsPriority = new List<int[]>();
    private List<int[]> goal = new List<int[]>();
    private List<int[]> corner1Priority = new List<int[]>();
    private List<int[]> corner2Priority = new List<int[]>();
    private List<int[]> corner3Priority = new List<int[]>();
    private List<int[]> corner4Priority = new List<int[]>();

    int maxTileNumber;
    int maxTileRowIndex;
    int maxTileColumnIndex;

    public int weight;
    public int division;

    void Awake()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        left.Add(new int[] { 0, 0, 0, 0 });
        left.Add(new int[] { 0, 0, 0, 0 });
        left.Add(new int[] { 0, 0, 0, 0 });
        left.Add(new int[] { 0, 0, 0, 0 });

        right.Add(new int[] { 0, 0, 0, 0 });
        right.Add(new int[] { 0, 0, 0, 0 });
        right.Add(new int[] { 0, 0, 0, 0 });
        right.Add(new int[] { 0, 0, 0, 0 });

        up.Add(new int[] { 0, 0, 0, 0 });
        up.Add(new int[] { 0, 0, 0, 0 });
        up.Add(new int[] { 0, 0, 0, 0 });
        up.Add(new int[] { 0, 0, 0, 0 });

        down.Add(new int[] { 0, 0, 0, 0 });
        down.Add(new int[] { 0, 0, 0, 0 });
        down.Add(new int[] { 0, 0, 0, 0 });
        down.Add(new int[] { 0, 0, 0, 0 });

        goal.Add(new int[] { 0, 0, 0, 0 });
        goal.Add(new int[] { 0, 0, 0, 0 });
        goal.Add(new int[] { 0, 0, 0, 0 });
        goal.Add(new int[] { 0, 0, 0, 0 });

        mergeThisTurn.Add(new bool[] { false, false, false, false });
        mergeThisTurn.Add(new bool[] { false, false, false, false });
        mergeThisTurn.Add(new bool[] { false, false, false, false });
        mergeThisTurn.Add(new bool[] { false, false, false, false });

        corner1Priority.Add(new int[] { 16, 15, 14, 13 });
        corner1Priority.Add(new int[] { 9, 10, 11, 12 });
        corner1Priority.Add(new int[] { 8, 7, 6, 5 });
        corner1Priority.Add(new int[] { 1, 2, 3, 4 });

        corner2Priority.Add(new int[] { 13, 14, 15, 16 });
        corner2Priority.Add(new int[] { 12, 11, 10, 9 });
        corner2Priority.Add(new int[] { 5, 6, 7, 8 });
        corner2Priority.Add(new int[] { 4, 3, 2, 1 });

        corner3Priority.Add(new int[] { 1, 2, 3, 4 });
        corner3Priority.Add(new int[] { 8, 7, 6, 5 });
        corner3Priority.Add(new int[] { 9, 10, 11, 12 });
        corner3Priority.Add(new int[] { 16, 15, 14, 13 });

        corner4Priority.Add(new int[] { 4, 3, 2, 1 });
        corner4Priority.Add(new int[] { 5, 6, 7, 8 });
        corner4Priority.Add(new int[] { 12, 11, 10, 9 });
        corner4Priority.Add(new int[] { 13, 14, 15, 16 });

        rowsPriority.Add(new int[] { 4, 3, 2, 1 });
        rowsPriority.Add(new int[] { 5, 6, 7, 8 });
        rowsPriority.Add(new int[] { 12, 11, 10, 9 });
        rowsPriority.Add(new int[] { 13, 14, 15, 16 });
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    //Initialize mergeThisTurn = false for all tiles
    public void InitializeMergeThisTurn(List<bool[]> mergeThisTurn)
    {
        for(int i = 0; i < mergeThisTurn.Count; i++)
        {
            for(int j = 0; j < mergeThisTurn[i].Length; j++)
            {
                mergeThisTurn[i][j] = false;
            }
        }
    }

    //Tranpose matrix
    public void Tranpose(List<int[]> Grid)
    {
        for(int i = 0; i < Grid.Count; i++)
        {
            for(int j = i; j < Grid[i].Length; j++)
            {
                int temp = Grid[i][j];
                Grid[i][j] = Grid[j][i];
                Grid[j][i] = temp;
            }
        }
    }

    //Return maxTile value and its location in terms of row index and column index
    public void FindMaxTile(List<int[]> Grid, ref int maxTile, ref int rowIndex, ref int columnIndex)
    {
        maxTile = Grid[0][0];
        rowIndex = 0;
        columnIndex = 0;
        int minDistance = FindDistancetoClosestCorner(rowIndex, columnIndex);
        for(int i = 0; i < Grid.Count; i++)
        {
            for(int j = 0; j < Grid[i].Length; j++)
            {
                if (Grid[i][j] == maxTile)
                {
                    if (FindDistancetoClosestCorner(i, j) < minDistance)
                    {
                        maxTile = Grid[i][j];
                        rowIndex = i;
                        columnIndex = j;
                        minDistance = FindDistancetoClosestCorner(i, j);
                    }
                }
                if(Grid[i][j] > maxTile)
                {
                    maxTile = Grid[i][j];
                    rowIndex = i;
                    columnIndex = j;
                    minDistance = FindDistancetoClosestCorner(i, j);
                }
            }
        }
    }

    //Check if the maxTile is at one of four corner
    public bool CheckCorner(int x, int y)
    {
        if (x == 0 && y == 0) return true;
        if (x == 0 && y == 3) return true;
        if (x == 3 && y == 0) return true;
        if (x == 3 && y == 3) return true;
        return false;
    }

    //Return the distance from the maxTile to the closest corner
    public int FindDistancetoClosestCorner(int x, int y)
    {
        int corner1 = x + y;
        int corner2 = x + 3 - y;
        int corner3 = 3 - x + y;
        int corner4 = 3 - x + 3 - y;

        int min = corner1;
        int closest = 1;
        if (min > corner2)
        {
            min = corner2;
            closest = 2;
        }
        if (min > corner3)
        {
            min = corner3;
            closest = 3;
        }
        if (min > corner4)
        {
            min = corner4;
            closest = 4;
        }
        return min;
    }

    //Return the closest corner to the maxTile
    public int FindClosestCornerforMax(int maxTileRowIndex, int maxTileColumnIndex)
    {
        int corner1 = maxTileRowIndex + maxTileColumnIndex;
        int corner2 = maxTileRowIndex + 3 - maxTileColumnIndex;
        int corner3 = 3 - maxTileRowIndex + maxTileColumnIndex;
        int corner4 = 3 - maxTileRowIndex + 3 - maxTileColumnIndex;

        int min = corner1;
        int closest = 1;
        if (min > corner2)
        {
            min = corner2;
            closest = 2;
        }
        if (min > corner3)
        {
            min = corner3;
            closest = 3;
        }
        if (min > corner4)
        {
            min = corner4;
            closest = 4;
        }
        return closest;
    }

    //Construct score distribution according to the maxTile's location
    public void ConstructPriority(int x, int y)
    {
        int closest = FindClosestCornerforMax(x, y);
        switch(closest)
        {
            case 1:
                rowsPriority = corner1Priority;
                break;
            case 2:
                rowsPriority = corner2Priority;
                break;
            case 3:
                rowsPriority = corner3Priority;
                break;
            case 4:
                rowsPriority = corner4Priority;
                break;
        }
    }

    //Check if the line is decreasing (for monotonicity)
    public bool CheckDecreasing(int[] Line)
    {
        for(int i = 0; i < Line.Length - 1; i++)
        {
            if (Line[i] == 0) continue;
            for(int j = i + 1; j < Line.Length; j++)
            {
                if (Line[j] == 0) continue;
                if (Line[i] < Line[j]) return false;
            }
        }
        return true;
    }

    //Check if the line is increasing (for monotonicity)
    public bool CheckIncreasing(int[] Line)
    {
        for (int i = 0; i < Line.Length - 1; i++)
        {
            if (Line[i] == 0) continue;
            for (int j = i + 1; j < Line.Length; j++)
            {
                if (Line[j] == 0) continue;
                if (Line[i] > Line[j]) return false;
            }
        }
        return true;
    }

    //Return monotonic score evaluated by given bonus if the line is either decreasing or increasing (bonus = (smallest value in the line) * priority of the line)
    public int EvaluateMonotonicScore(List<int[]> Grid)
    {
        int score = 0;
        bool check = (rowsPriority[0][0] > 10) ? false : true;
        for (int i = 0; i < Grid.Count; i++)
        {
            if (rowsPriority[i][0] < rowsPriority[i][1])
            {
                if (check)
                {
                    if (CheckDecreasing(Grid[i])) score += Grid[i][3] * (i + 1);
                }
                else
                {
                    if (CheckDecreasing(Grid[i])) score += Grid[i][3] * (3 - i + 1);
                }
            }
            else
            {
                if (check)
                {
                    if (CheckIncreasing(Grid[i])) score += Grid[i][0] * (i + 1);
                }
                else
                {
                    if (CheckIncreasing(Grid[i])) score += Grid[i][0] * (3 - i + 1);
                }
            }
        }
        bool checkcolumn;
        if (check)
        {
            if (rowsPriority[0][0] == 13)
            {
                checkcolumn = true;
            }
            else checkcolumn = false;
        }
        else
        {
            if (rowsPriority[3][3] == 16)
            {
                checkcolumn = true;
            }
            else checkcolumn = false;
        }
        Tranpose(Grid);
        Tranpose(rowsPriority);
        for (int i = 0; i < Grid.Count; i++)
        {
            if (rowsPriority[0][i] < rowsPriority[1][i])
            {
                if (checkcolumn)
                {
                    if (CheckDecreasing(Grid[i])) score += Grid[i][3] * (i + 1);
                }
                else
                {
                    if (CheckDecreasing(Grid[i])) score += Grid[i][3] * (3 - i + 1);
                }
            }
            else
            {
                if (checkcolumn)
                {
                    if (CheckIncreasing(Grid[i])) score += Grid[i][0] * (i + 1);
                }
                else
                {
                    if (CheckIncreasing(Grid[i])) score += Grid[i][0] * (3 - i + 1);
                }
            }
        }
        Tranpose(Grid);
        Tranpose(rowsPriority);
        return score;
    }

    //Return weight for score evaluation
    public int CalScoreWeight()
    {
        return weight + maxTileNumber/division;
    }

    //Return score evaluation (scoreEvaluate = (tile number) * priority
    public int EvaluateScore(List<int[]> Grid)
    {
        int scoreEvaluate = 0;
        for(int i = 0; i < Grid.Count; i++)
        {
            for(int j = 0; j < Grid[i].Length; j++)
            {
                scoreEvaluate += Grid[i][j] * rowsPriority[i][j];
            }
        }
        return scoreEvaluate;
    }

    //Make a down move
    public bool MoveDown(int[] LineOfints, int j)
    {
        for (int i = 0; i < LineOfints.Length - 1; i++)
        {
            //MOVE BLOCK 
            if (LineOfints[i] == 0 && LineOfints[i + 1] != 0)
            {
                LineOfints[i] = LineOfints[i + 1];
                LineOfints[i + 1] = 0;
                return true;
            }
            // MERGE BLOCK
            if (LineOfints[i] != 0 && LineOfints[i] == LineOfints[i + 1] &&
                mergeThisTurn[j][i] == false && mergeThisTurn[j][i+1] == false)
            {
                LineOfints[i] *= 2;
                LineOfints[i + 1] = 0;
                mergeThisTurn[j][i] = true;
                return true;
            }
        }
        return false;
    }

    //Make a up move
    public bool MoveUp(int[] LineOfints, int j)
    {
        for (int i = LineOfints.Length - 1; i > 0; i--)
        {

            //MOVE BLOCK 
            if (LineOfints[i] == 0 && LineOfints[i - 1] != 0)
            {
                LineOfints[i] = LineOfints[i - 1];
                LineOfints[i - 1] = 0;
                return true;
            }
            // MERGE BLOCK
            if (LineOfints[i] != 0 && LineOfints[i] == LineOfints[i - 1] &&
                mergeThisTurn[j][i] == false && mergeThisTurn[j][i] == false)
            {
                LineOfints[i] *= 2;
                LineOfints[i - 1] = 0;
                mergeThisTurn[j][i] = true;
                return true;
            }
        }
        return false;
    }

    //Evaluate the board after moving left
    public int GetScoreLeft()
    {
        bool checkleft = false;
        left = gm.GetCurrentRows();
        InitializeMergeThisTurn(mergeThisTurn);
        for (int i = 0; i < 4; i++)
        {
            while (MoveDown(left[i], i))
            {
                checkleft = true;
            }
        }
        if (!checkleft) return 0;
        int corner = 0;
        int maxTile = 0;
        int rowIndex = 0;
        int columnIndex = 0;
        FindMaxTile(left, ref maxTile, ref rowIndex, ref columnIndex);
        ConstructPriority(rowIndex, columnIndex);
        if (CheckCorner(rowIndex, columnIndex)) corner = maxTile * 16;
        return EvaluateScore(left) * CalScoreWeight() + EvaluateMonotonicScore(left) + corner;
    }

    //Evaluate the board after moving right
    public int GetScoreRight()
    {
        bool checkright = false;
        right = gm.GetCurrentRows();
        InitializeMergeThisTurn(mergeThisTurn);
        for (int i = 0; i < 4; i++)
        {
            while (MoveUp(right[i], i))
            {
                checkright = true;
            }
        }
        if (!checkright) return 0;
        int corner = 0;
        int maxTile = 0;
        int rowIndex = 0;
        int columnIndex = 0;
        FindMaxTile(right, ref maxTile, ref rowIndex, ref columnIndex);
        ConstructPriority(rowIndex, columnIndex);
        if (CheckCorner(rowIndex, columnIndex)) corner = maxTile * 16;
        return EvaluateScore(right) * CalScoreWeight() + EvaluateMonotonicScore(right) + corner;
    }

    //Evalaate the board after moving up
    public int GetScoreUp()
    {
        bool checkup = false;
        up = gm.GetCurrentColumns();
        InitializeMergeThisTurn(mergeThisTurn);
        for (int i = 0; i < 4; i++)
        {
            while (MoveDown(up[i], i))
            {
                checkup = true;
            }
        }
        Tranpose(up);
        if (!checkup) return 0;
        int corner = 0;
        int maxTile = 0;
        int rowIndex = 0;
        int columnIndex = 0;
        FindMaxTile(up, ref maxTile, ref rowIndex, ref columnIndex);
        ConstructPriority(rowIndex, columnIndex);
        if (CheckCorner(rowIndex, columnIndex)) corner = maxTile * 16;
        return EvaluateScore(up) * CalScoreWeight() + EvaluateMonotonicScore(up) + corner;
    }

    //Evaluate the board after moving down
    public int GetScoreDown()
    {
        bool checkdown = false;
        down = gm.GetCurrentColumns();
        InitializeMergeThisTurn(mergeThisTurn);
        for (int i = 0; i < 4; i++)
        {
            while (MoveUp(down[i], i))
            {
                checkdown = true;
            }
        }
        Tranpose(down);
        if (!checkdown) return 0;
        int corner = 0;
        int maxTile = 0;
        int rowIndex = 0;
        int columnIndex = 0;
        FindMaxTile(down, ref maxTile, ref rowIndex, ref columnIndex);
        ConstructPriority(rowIndex, columnIndex);
        if (CheckCorner(rowIndex, columnIndex)) corner = maxTile * 16;
        return EvaluateScore(down) * CalScoreWeight() +  EvaluateMonotonicScore(down) + corner;
    }

    //Choose the best move according to the highest score
    public MoveDirection GetMove(GameManager gm)
    {
        int scoredown = GetScoreDown();
        int scoreup = GetScoreUp();
        int scoreleft = GetScoreLeft();
        int scoreright = GetScoreRight();

        int maxScore = scoredown;
        int best = 0;
        if (maxScore < scoreup)
        {
            maxScore = scoreup;
            best = 1;
        }
        if (maxScore < scoreleft)
        {
            maxScore = scoreleft;
            best = 2;
        }
        if (maxScore < scoreright)
        {
            maxScore = scoreright;
            best = 3;
        }

        if (best == 0) return MoveDirection.Down;
        if (best == 1) return MoveDirection.Up;
        if (best == 2) return MoveDirection.Left;
        return MoveDirection.Right;
    }
}