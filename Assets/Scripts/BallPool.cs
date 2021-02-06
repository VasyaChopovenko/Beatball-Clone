using System;
using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour, IDisposable
{
    [SerializeField]
    private GameObject ballPrefab;

    private Queue<GameObject> balls = new Queue<GameObject>();

    public List<GameObject> ballsOnScene { get; set; } = new List<GameObject>();

    public int BallsCount => ballsOnScene.Count;

    public static BallPool Instance { get; set; }

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        AddBalls(285);
    }

    public bool TryGet(out GameObject gameObject)
    {
        if (balls.Count == 0)
        {
            gameObject = null;
            return false;
        }

        gameObject = balls.Dequeue();
        ballsOnScene.Add(gameObject);
        return true;
    }

    private void AddBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newBall = Instantiate(ballPrefab, transform);
            newBall.gameObject.SetActive(false);
            balls.Enqueue(newBall);
        }
    }

    public void ReturnToPool(GameObject ball)
    {
        ball.gameObject.SetActive(false);
        balls.Enqueue(ball);
    }

    public GameObject GetByIndex(int index)
    {
        return ballsOnScene[index];
    }

    public void Dispose()
    {
        foreach (var ball in ballsOnScene)
        {
            ReturnToPool(ball);
            ball.SetActive(false);
        }

        ballsOnScene.Clear();
    }

    public void DestroyBall(GameObject ball)
    {
        ReturnToPool(ball);
        ballsOnScene.Remove(ball);
    }
}
