using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonusFactory : IDisposable
{
    List<GameObject> bonuses = new List<GameObject>();

    public void CreateBonus(Vector3 position)
    {
        var newBonus = GameObject.Instantiate(GameManager.Instance.bonuses[UnityEngine.Random.Range(0, GameManager.Instance.bonuses.Length)], position, Quaternion.identity, GameManager.Instance.canvas.transform);
        bonuses.Add(newBonus.gameObject);
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

public abstract class Bonus : MonoBehaviour
{
    public abstract void Get();
    private bool isColliding;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Player player))
        {
            Get();
            GameManager.Instance.BonusFactory.DeleteBonus(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        GameManager.Instance.BonusFactory.DeleteBonus(gameObject);
    }
}
