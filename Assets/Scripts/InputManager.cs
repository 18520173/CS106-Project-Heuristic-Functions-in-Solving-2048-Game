using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum MoveDirection
{
	Left, Right, Up, Down
}


public class InputManager : MonoBehaviour {
	
	private GameManager gm;
    private BestFirstAlgorithm BF_AI;
    private PriorityBased pb;

    void Awake()
	{
		gm = GameObject.FindObjectOfType<GameManager> ();
        BF_AI = GameObject.FindObjectOfType<BestFirstAlgorithm>();
        pb = GameObject.FindObjectOfType<PriorityBased>();
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (gm.State == GameState.Playing)
		{
			if (Input.GetKeyDown (KeyCode.RightArrow)) 
			{
				//right move
				gm.Move(MoveDirection.Right);
			} 
			else if (Input.GetKeyDown (KeyCode.LeftArrow)) 
			{
				//left move
				gm.Move(MoveDirection.Left);
			}
			else if (Input.GetKeyDown (KeyCode.UpArrow)) 
			{
				//up move
				gm.Move(MoveDirection.Up);
			}
			else if (Input.GetKeyDown (KeyCode.DownArrow)) 
			{
				//down move
				gm.Move(MoveDirection.Down);
			}
		}
	    if(gm.State == GameState.BestFirst)
        {
            MoveDirection md = BF_AI.GetMove(gm);
            gm.Move(md);
        }
        if(gm.State == GameState.Random)
        {
            MoveDirection md = Randomize();
            gm.Move(md);
        }
        if(gm.State == GameState.PriorityBased)
        {
            MoveDirection md = pb.GetMove(gm);
            gm.Move(md);
        }
	}

    private MoveDirection Randomize()
    {
        int best = Random.Range(0, 4);
        if (best == 0) return MoveDirection.Down;
        if (best == 1) return MoveDirection.Up;
        if (best == 2) return MoveDirection.Left;
        return MoveDirection.Right;
    }

   
}
