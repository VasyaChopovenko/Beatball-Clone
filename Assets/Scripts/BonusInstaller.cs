using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonusInstaller : MonoBehaviour
{
    private Bonus bonus;

    private void OnEnable()
    {
        bonus = GameManager.Instance.bonuses[UnityEngine.Random.Range(0, 2)];
    }

    void OnBecameInvisible()
    {
        GameManager.Instance.BonusFactory.DeleteBonus(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Player player))
        {
            bonus.Get();
            Destroy(gameObject);
        }
    }
}

public class BonusFactory : IDisposable
{
    List<GameObject> bonuses = new List<GameObject>();

    public void CreateBonus(Vector3 position)
    {
        var newBonus = GameObject.Instantiate(GameManager.Instance.bonusPrefab, position, Quaternion.identity, GameManager.Instance.canvas.transform);
        bonuses.Add(newBonus);
    }

    public void DeleteBonus(GameObject bonus)
    {
        bonuses.Remove(bonus);
        GameObject.Destroy(bonus);
    }

    public void Dispose()
    {
        foreach (GameObject bonus in bonuses)
            GameObject.Destroy(bonus);

        bonuses.Clear();
    }
}

public abstract class Bonus
{
    public abstract void Get();
}

public class MultiBall : Bonus
{
    private int ballCounts;

    public MultiBall(int count)
    {
        ballCounts = count;
    }

    public override void Get()
    {
        BallFactory ballFactory = GameManager.Instance.BallFactory;
        List<GameObject> balls = new List<GameObject>();

        for (int i = 0; i < ballFactory.CountBalls; i++)
            balls.Add(ballFactory.GetByIndex(i));

        foreach (var ball in balls)
        {
            for(int i = 0; i < ballCounts; i++)
                ballFactory.CreateBall(ball.transform.position);
        }  
    }
}
