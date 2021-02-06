using System.Collections.Generic;
using UnityEngine;

public class MultiBall : Bonus
{
    [SerializeField]
    private int ballCounts;

    public override void Get()
    {
        BallPool ballPool = BallPool.Instance;
        List<GameObject> balls = new List<GameObject>(ballPool.ballsOnScene);

        Debug.Log($"{balls.Count}");

        foreach (var ball in balls)
        {
            for (int i = 0; i < ballCounts; i++)
            {
                if (BallPool.Instance.TryGet(out GameObject pooledBall))
                {
                    pooledBall.transform.position = ball.transform.position;
                    pooledBall.SetActive(true);
                }
            }
        }
    }
}
