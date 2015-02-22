using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(coli))]

public class OATrigger : MonoBehaviour {

    ArrayList obstacleAnimators = new ArrayList();
    bool bIsTriggerd;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
       // Space.World
        //if (transform.position.magnitude < 0.5f && !bIsTriggerd)
        //{
        //    foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
        //    {
        //        if (obstacleAnimator.triggerOnScreen)
        //            obstacleAnimator.OnTriggerTouched();
        //    }
        //    bIsTriggerd = true;
        //}
	
	}

    public void AddObstacleAnimator(ObstacleAnimator inObstacleAnimator)
    {
        obstacleAnimators.Add(inObstacleAnimator);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        foreach(ObstacleAnimator obstacleAnimator in obstacleAnimators)
        {
            if (!obstacleAnimator.triggerOnScreen)
                obstacleAnimator.OnTriggerTouched();
        }
        else if (other.gameObject.tag == "LevelSwicher")
        {
            foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
            {
                if (obstacleAnimator.triggerOnScreen)
                    obstacleAnimator.OnTriggerTouched();
            }
            bIsTriggerd = true;
        }
        
    }

    void TriggerOnRestartLevel(int levelIndex)
    {

        bIsTriggerd = false;
        //foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
        //{
        //    obstacleAnimator.OnStartLevel(levelIndex);
        //}
    }

    void TtiggerGameIsPused()
    {
        
        //foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
        //{
        //    obstacleAnimator.GameIsPused();
        //}
    }


}
