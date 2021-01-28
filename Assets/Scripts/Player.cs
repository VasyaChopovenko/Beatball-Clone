using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody _rigidbody;

    void Start()
    { 
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var position = Mathf.Clamp(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x, -32, 32);
        _rigidbody.MovePosition(new Vector3(position, -17, 0));
    }
    
}
