using UnityEngine;
using System;

public class Gun : Bonus
{
    public override void Get()
    {
        GameManager.Instance.currentPlayer.StartShooting();
    }
}
