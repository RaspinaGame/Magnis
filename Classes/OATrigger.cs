using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(coli))]

public class OATrigger : MonoBehaviour {

    ArrayList obstacleAnimators = new ArrayList();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddObstacleAnimator(ObstacleAnimator inObstacleAnimator)
    {

        obstacleAnimators.Add(inObstacleAnimator);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach(ObstacleAnimator obstacleAnimator in obstacleAnimators)
        {
            obstacleAnimator.OnTriggerTouched();
        }
    }


}
