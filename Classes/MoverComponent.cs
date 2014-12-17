using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoverComponent : MonoBehaviour {

	public Vector2 Direction;
	public bool IsBackground;
    public bool isMovable = false;

	public List<MoverComponent> MoverObstacles;
    Vector3 startingPosition;

    void Awake()
    {
        startingPosition = transform.localPosition;
    }
    
    

	// Use this for initialization
	void Start () 
	{
        //nima khare
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void Move()
	{
        if (isMovable)
		    transform.Translate (Direction * Time.deltaTime);
		foreach (MoverComponent MC in MoverObstacles)
		{
			MC.Move();
		}

	}
    public void MoveBack(Vector3 StartPoint,float timeToMoveBack,float timeElapsed,Vector3 DiePosition)
	{
        //transform.Translate(-Direction * Time.deltaTime * Mathf.Max(Mathf.Abs(StartPoint.y / 2f), 0.8f));
       // transform.Translate((StartPoint - transform.position).normalized * Time.deltaTime * Mathf.Max(Mathf.Abs((StartPoint - transform.position).magnitude),10f) );
        StartPoint.z = 0;
      //  transform.Translate(((StartPoint - transform.position) / timeToMoveBack) * Time.deltaTime);

        Vector3 vAvg = ((StartPoint - DiePosition) / timeToMoveBack);
        transform.position = ((-vAvg / timeToMoveBack * (timeElapsed * timeElapsed)) + (2f * vAvg * timeElapsed) + DiePosition);
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
        transform.localPosition = startingPosition;
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
