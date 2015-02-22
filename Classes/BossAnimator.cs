using UnityEngine;
using System.Collections;


public class BossAnimator : ObstacleAnimator {

    public int levelNumber;

    public override void OnRestartLevel(int levelIndex)
    {

        // anim.Stop();
        if (levelNumber == levelIndex + 1)
        {
            resertToInitial();
        }

        //anim.Play(animClip.name, PlayMode.StopAll);
        if (trigger == null)
        {
            OnTriggerTouched();
        }
    }
}
