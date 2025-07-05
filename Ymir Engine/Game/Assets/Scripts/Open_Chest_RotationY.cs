using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Open_Chest_RotationY : YmirComponent
{
    private float time = 0f;
    private float velocity = 30f;

    public void Start()
    {
    }

    public void Update()
    {
        if (time < 1)
        {
            //gameObject.transform.localRotation = Quaternion.RotateAroundAxis(new Vector3(0, 1, 0), velocity * time); en caso de fallar cambiar a este
            gameObject.transform.localRotation = Quaternion.Euler(180f, velocity * time, 0f);
            time += Time.deltaTime;
        }

        return;
    }
}