using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster
{
    private BlasterShot blasterShotPrefab;

    private Transform[] transforms;

    public Blaster(Transform[] transform, BlasterShot blasterShotPrefab)
    {
        this.transforms = transform;
        this.blasterShotPrefab = blasterShotPrefab;
    }

    public void Fire()
    {
        for (int i = 0; i < transforms.Length; i++)
            GameObject.Instantiate(blasterShotPrefab, transforms[i].position, transforms[i].rotation);
    }
}
