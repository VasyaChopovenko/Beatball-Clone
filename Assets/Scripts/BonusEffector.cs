using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BonusEffector
{
    public abstract void FixedUpdate(Rigidbody rigidbody);

    public abstract void Bounce(Vector3 normal, Rigidbody rigidbody, Vector3 velocity);

    public abstract void Hit(IHittable hittable);

    public virtual void EnableEffect(ParticleSystem particleSystem)
    {
        
    }

    public virtual void DisableEffect(ParticleSystem particleSystem)
    {

    }
}

public class DefaultEffector : BonusEffector
{
    protected float _speed = 20f;

    protected int _power = 1;

    public override void Bounce(Vector3 normal, Rigidbody rigidbody, Vector3 velocity)
    {
        rigidbody.velocity = Vector3.Reflect(velocity, normal);
    }

    public override void FixedUpdate(Rigidbody rigidbody)
    {
        rigidbody.velocity = rigidbody.velocity.normalized * _speed;
    }

    public override void Hit(IHittable hittable)
    {
        hittable.Hit(_power);
    }
}

public class FiredEffector : DefaultEffector
{
    public FiredEffector(float speed)
    {
        _speed = speed;
        _power = 9999;
    }

    public override void Bounce(Vector3 normal, Rigidbody rigidbody, Vector3 velocity)
    {
        rigidbody.velocity = velocity;
    }

    public override void DisableEffect(ParticleSystem particleSystem)
    {
        particleSystem.Stop();
    }

    public override void EnableEffect(ParticleSystem particleSystem)
    {
        particleSystem.Play();
    }
}
