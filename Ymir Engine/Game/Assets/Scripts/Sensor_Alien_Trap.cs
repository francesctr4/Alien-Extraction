using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Sensor_Alien_Trap : YmirComponent
{
    public float timerSensor = 5f;
    private bool active = false;
    private bool hitPlayer = false;
    private float time = 0f;
    private Vector3 originalPosition;

    public void Start()
    {
        originalPosition = gameObject.transform.globalPosition;
    }

    public void Update()
    {
        if (hitPlayer)
        {
            if (active)
            {
                gameObject.SetPosition(Vector3.negativeInfinity * Time.deltaTime * 1f);
                time += Time.deltaTime;

                if (time >= timerSensor)
                {
                    active = false;
                    hitPlayer = false;
                    gameObject.SetPosition(originalPosition);
                    time = 0f;
                }
            }
        }
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            active = true;
            hitPlayer = true;
            InternalCalls.GetGameObjectByName("Alien_Trap").SetActive(true);
        }
    }
}