using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoverComponent : MonoBehaviour {

	public Vector2 Direction;
	public bool IsBackground;
    public bool bIsChild;

	public float testee;

	public List<MoverComponent> MoverObstacles;
    Vector3 startingPosition;

	// Use this for initialization
	void Start () 
	{
        startingPosition = transform.position;
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

	public void ResetLevel(Vector3 StartPoint)
	{
		Vector3 tempVec;
		tempVec = StartPoint;
		tempVec.z = transform.position.z;
		transform.position = tempVec;

        BroadcastMessage("ResetChildren", SendMessageOptions.DontRequireReceiver);
        BroadcastMessage("OnStartLevel", SendMessageOptions.DontRequireReceiver);
	}

    public void ResetChildren()
    {
        if (bIsChild)
        {
            transform.position = startingPosition;
        }
    }
		
	public void LevelFinished(Vector3 EndPoint)
	{
		Vector3 tempVec;
		tempVec = EndPoint;
		tempVec.z = transform.position.z;
		transform.position = tempVec;
	}

	public void AddToMoverChildrenObstacle(MoverComponent MC)
	{
		if ( MoverObstacles == null )
			MoverObstacles = new List<MoverComponent>();
		MoverObstacles.Add (MC);
	}
}
