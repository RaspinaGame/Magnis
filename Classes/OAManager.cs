using UnityEngine;
using System.Collections;

public class OAManager : MonoBehaviour
{

    ArrayList obstacleAnimators = new ArrayList();
    ArrayList triggerdObstacleAnimators = new ArrayList();
    //bool bIsTriggerd;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    //void Update()
    //{
    //    // Space.World
    //    if (transform.position.magnitude < 0.5f && !bIsTriggerd)
    //    {
    //        foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
    //        {
    //            if (obstacleAnimator.triggerOnScreen)
    //                obstacleAnimator.OnTriggerTouched();
    //        }
    //        bIsTriggerd = true;
    //    }
    //}

    public void AddObstacleAnimator(ObstacleAnimator inObstacleAnimator)
    {
        obstacleAnimators.Add(inObstacleAnimator);
    }

    public void AddTriggerdObstacleAnimators(ObstacleAnimator inObstacleAnimator)
    {
        triggerdObstacleAnimators.Add(inObstacleAnimator);
    }

    public bool ShoudBlendToinitialAnimClip(ObstacleAnimator inObstacleAnimator)
    {
        if (triggerdObstacleAnimators.Count > 0)
        {
 
           return( (ObstacleAnimator)triggerdObstacleAnimators[0] == inObstacleAnimator);
        }
        return false;
    }
    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
    //    {
    //        if (!obstacleAnimator.triggerOnScreen)
    //            obstacleAnimator.OnTriggerTouched();
    //    }
    //}

    //void TtiggerOnStartLevel(int levelIndex)
    //{

    //    foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
    //    {
    //        obstacleAnimator.OnStartLevel(levelIndex);
    //    }
    //}

    //void TtiggerGameIsPused()
    //{
    //    bIsTriggerd = false;
    //    foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
    //    {
    //        obstacleAnimator.GameIsPused();
    //    }
    //}

    public void TtiggerOnStartLevel(int levelIndex)
    {

        foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
        {
            obstacleAnimator.OnStartLevel(levelIndex);
        }

        triggerdObstacleAnimators.Clear();
    }

    public void TtiggerGameIsPused(int levelIndex)
    {
        //bIsTriggerd = false;
        foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
        {
            obstacleAnimator.GameIsPused(levelIndex);
        }
    }

    public void TtiggerRollBack(int levelIndex)
    {
        //bIsTriggerd = false;
        foreach (ObstacleAnimator obstacleAnimator in obstacleAnimators)
        {
            obstacleAnimator.RoleBack(levelIndex);
        }
    }
}