using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private BlasterShot blasterShotPrefab;
    Rigidbody _rigidbody;

    private int shotsCount = 10;
    private Coroutine coroutine;

    public Blaster blaster { get; private set; }

    public Transform[] transforms = new Transform[2];

    private IEnumerator Shooting()
    {
        var time = new WaitForSeconds(0.5f);

        for (int i = 0; i < shotsCount; i++)
        {
            blaster.Fire();
            yield return time;
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        blaster = new Blaster(transforms, blasterShotPrefab);
    }

    private void FixedUpdate()
    {
        var position = Mathf.Clamp(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x, -32, 32);
        _rigidbody.MovePosition(new Vector3(position, -17, 0));
    }

    public void StartShooting()
    {
        StopShooting();
        coroutine = StartCoroutine(nameof(Shooting));
    }

    public void StopShooting()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    private IEnumerator ResetBonus()
    {
        var time = new WaitForSeconds(3.5f);

        yield return time;
        var activeBalls = BallPool.Instance.ballsOnScene;

        for (int i = 0; i < activeBalls.Count; i++)
            activeBalls[i].GetComponent<Ball>().SetEffector(BallPool.bonusEffectors[0]);
    }

    public void StartResetBonusCoroutine()
    {
        StartCoroutine(nameof(ResetBonus));
    }
}
