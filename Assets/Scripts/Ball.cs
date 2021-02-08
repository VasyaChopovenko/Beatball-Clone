using UnityEngine;
using System.Collections.Generic;
using System;

public class Ball : MonoBehaviour
{
    Rigidbody _rigidbody;
    Renderer _renderer;
    [SerializeField]
    private ParticleSystem _particleSystem;

    Vector3 velocity;

    public BallData ballData;

    public static event Action OnBallDestroyed;

    private void Awake()
    {
        ballData = new BallData() { BonusEffector = BallPool.bonusEffectors[0] };
    }

    void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _rigidbody.velocity = Vector3.up;
    }

    void FixedUpdate()
    {
        ballData.BonusEffector.FixedUpdate(_rigidbody);
        velocity = _rigidbody.velocity;

        if (!_renderer.isVisible)
        {
            BallPool.Instance.DestroyBall(gameObject);
            OnBallDestroyed?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IHittable hittable))
        {
            ballData.BonusEffector.Bounce(collision.contacts[0].normal, _rigidbody, velocity);
            ballData.BonusEffector.Hit(hittable);
        }   
        else
            _rigidbody.velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);
    }

    public void SetEffector(BonusEffector bonusEffector)
    { 
        ballData.BonusEffector.DisableEffect(_particleSystem);
        bonusEffector.EnableEffect(_particleSystem);
        ballData.BonusEffector = bonusEffector;
    }
}

public class BallData : IClonable<BallData>
{
    public BonusEffector BonusEffector { get; set; }

    public BallData Clone()
    {
        return new BallData() { BonusEffector = BonusEffector };
    }
}

public interface IClonable<T>
{
    T Clone();
}