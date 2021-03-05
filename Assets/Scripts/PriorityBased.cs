using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityBased : MonoBehaviour
{
    private GameManager gm;

    private List<int[]> left = new List<int[]>();
    private List<int[]> right = new List<int[]>();
    private List<int[]> up = new List<int[]>();
    private List<int[]> down = new List<int[]>();

    private List<bool[]> mergeThisTurn = new List<bool[]>();

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

        mergeThisTurn.Add(new bool[] { false, false, false, false });
        mergeThisTurn.Add(new bool[] { false, false, false, false });
        mergeThisTurn.Add(new bool[] { false, false, false, false });
        mergeThisTurn.Add(new bool[] { false, false, false, false });
    }

    //Initialize mergeThisTurn = false for all tiles
    public void InitializeMergeThisTurn(List<bool[]> mergeThisTurn)
    {
        for (int i = 0; i < mergeThisTurn.Count; i++)
        {
            for (int j = 0; j < mergeThisTurn[i].Length; j++)
            {
                mergeThisTurn[i][j] = false;
            }
        }
    }

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
                mergeThisTurn[j][i] == false && mergeThisTurn[j][i + 1] == false)
            {
                LineOfints[i] *= 2;
                LineOfints[i + 1] = 0;
                mergeThisTurn[j][i] = true;
                return true;
            }
        }
        return false;
    }

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

    //Choose move based on the priority: down, right, left, up. If down direction is invalid, choose the next direction and continue.
    public MoveDirection GetMove(GameManager gm)
    {
        bool checkdown = false;
        down = gm.GetCurrentColumns();
        InitializeMergeThisTurn(mergeThisTurn);
        for (int i = 0; i < 4; i++)
        {
            while (MoveUp(down[i], i))
            {
                checkdown = true;
                if (checkdown) return MoveDirection.Down;
            }
        }

        bool checkright = false;
        right = gm.GetCurrentRows();
        InitializeMergeThisTurn(mergeThisTurn);
        for (int i = 0; i < 4; i++)
        {
            while (MoveUp(right[i], i))
            {
                checkright = true;
                if (checkright) return MoveDirection.Right;
            }
        }

        bool checkleft = false;
        left = gm.GetCurrentRows();
        InitializeMergeThisTurn(mergeThisTurn);
        for (int i = 0; i < 4; i++)
        {
            while (MoveDown(left[i], i))
            {
                checkleft = true;
                if (checkleft) return MoveDirection.Left;
            }
        }

        return MoveDirection.Up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
