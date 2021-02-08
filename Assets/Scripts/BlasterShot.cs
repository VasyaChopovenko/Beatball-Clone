using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BlasterShot : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody;

    public float moveSpeed = 15f;

    void FixedUpdate()
    {
        rigidBody.velocity = Vector3.up * moveSpeed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IHittable hittable))
            hittable.Hit(1);

        Destroy(gameObject);
    }
}
