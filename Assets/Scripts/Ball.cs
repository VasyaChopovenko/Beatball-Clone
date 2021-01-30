using UnityEngine;
using System.Collections.Generic;
using System;

public class Ball : MonoBehaviour
{
    float _speed = 20f;
    Rigidbody _rigidbody;
    Vector3 _velocity;
    Renderer _renderer;

    void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _rigidbody.velocity = Vector3.up * _speed;
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = _rigidbody.velocity.normalized * _speed;
        _velocity = _rigidbody.velocity;

        if (!_renderer.isVisible)
        {
            GameManager.Instance.BallFactory.DeleteBall(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
    }
}

public class BallFactory : IDisposable
{
    private List<GameObject> balls = new List<GameObject>();
    private GameObject ballPrefab;
    public int CountBalls => balls.Count;

    public event Action OnBallDestroyed;

    public BallFactory(GameObject ballPref)
    {
        ballPrefab = ballPref;
    }

    public void CreateBall(Vector3 position)
    {
        var newBall = GameObject.Instantiate(ballPrefab, position, Quaternion.identity);
        balls.Add(newBall);
    }

    public void DeleteBall(GameObject ball)
    {
        balls.Remove(ball);
        GameObject.Destroy(ball);

        OnBallDestroyed?.Invoke();
    }

    public void Dispose()
    {
        foreach(var ball in balls)
        {
            GameObject.Destroy(ball);
        }

        balls.Clear();
    }

    public GameObject GetByIndex(int index)
    {
        return balls[index];
    }
}
