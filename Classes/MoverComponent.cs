using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoverComponent : MonoBehaviour {

	public Vector2 Direction;
	public bool IsBackground;

	public List<MoverComponent> MoverObstacles;
    Vector3 startingPosition;

	// Use this for initialization
	void Start () 
	{
        startingPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void Move()
	{
		transform.Translate (Direction * Time.deltaTime);
		foreach (MoverComponent MC in MoverObstacles)
		{
			MC.Move();
		}
	}

    public void MoveBack(Vector3 StartPoint)
	{
        transform.Translate(-Direction * Time.deltaTime * Mathf.Max( Mathf.Abs(transform.position.y / 2f) , 0.8f));
        //foreach (MoverComponent MC in MoverObstacles)
        //{
        //    MC.MoveBack(StartPoint);
        //}
	}

	public void ResetLevel(Vector3 StartPoint, int levelIndex)
	{
		Vector3 tempVec;
		tempVec = StartPoint;
		tempVec.z = transform.position.z;
		transform.position = tempVec;

        foreach(MoverComponent MC in MoverObstacles )
		{
            MC.ResetChildren(StartPoint);
		}
       // BroadcastMessage("TtiggerOnStartLevel", levelIndex, SendMessageOptions.DontRequireReceiver);
	}

    public void ResetChildren(Vector3 StartPoint)
    {
        transform.position = startingPosition;
    }
		
	public void LevelFinished(Vector3 EndPoint)
	{
        transform.localPosition = startingPosition ;
	}

	public void AddToMoverChildrenObstacle(MoverComponent MC)
	{
		if ( MoverObstacles == null )
			MoverObstacles = new List<MoverComponent>();
		MoverObstacles.Add (MC);
	}
}
