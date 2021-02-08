using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Bonus
{
    public override void Get()
    {
        var activeBalls = BallPool.Instance.ballsOnScene;

        for (int i = 0; i < activeBalls.Count; i++)
        {
            activeBalls[i].GetComponent<Ball>().SetEffector(BallPool.bonusEffectors[1]);
        }

        GameManager.Instance.currentPlayer.StartResetBonusCoroutine();
    }
}
