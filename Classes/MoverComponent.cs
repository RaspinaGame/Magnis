using UnityEngine;
using System.Collections;

public class MoverComponent : MonoBehaviour {

	public Vector2 Direction;
	public bool IsBackground;


	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Move()
	{
		transform.Translate (Direction * Time.deltaTime);
	}

	public void ResetLevel(Vector3 StartPoint)
	{
		Vector3 tempVec;
		tempVec = StartPoint;
		tempVec.z = transform.position.z;
		transform.position = tempVec;
	}
		
	public void LevelFinished(Vector3 EndPoint)
	{
		Vector3 tempVec;
		tempVec = EndPoint;
		tempVec.z = transform.position.z;
		transform.position = tempVec;
	}
}
