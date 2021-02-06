using UnityEngine;
using System.Collections.Generic;
using System;

public class Ball : MonoBehaviour
{
    float _speed = 20f;
    Rigidbody _rigidbody;
    Vector3 _velocity;
    Renderer _renderer;

    public static event Action OnBallDestroyed;

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
            BallPool.Instance.DestroyBall(gameObject);
            OnBallDestroyed?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
    }
}
