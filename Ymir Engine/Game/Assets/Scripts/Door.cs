using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Door : YmirComponent
{
	bool opened = false;
    float openTime = 0;
    float crono_openTime = 0;
    private Vector3 originalPosition;

    private float autoCloseDelay = 5f;

    public void Start()
	{
        originalPosition = gameObject.transform.localPosition;
		opened = false;
	}

	public void Update()
	{
        //Debug.Log("openTime: " + openTime);

		if (opened)
        {
            openTime = Time.time - openTime;

            if (openTime >= autoCloseDelay + crono_openTime)
            {
                if (gameObject.transform.localPosition.y < originalPosition.y)
                {
                    gameObject.SetPosition(gameObject.transform.globalPosition + new Vector3(0, 5, 0));
                }
                else
                {
                    opened = false;
                    openTime = 0f;
                }
            }
        }
	}

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
			gameObject.SetPosition(gameObject.transform.globalPosition + new Vector3(0, -5, 0));
			opened = true;
            openTime = Time.time;
            crono_openTime = Time.time;
        }
    }
}