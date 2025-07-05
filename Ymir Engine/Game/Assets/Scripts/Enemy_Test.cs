using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Enemy_Test : YmirComponent
{
    public float life;
	public void Start()
	{
        life = 100;
	}

	public void Update()
	{
        if(life <= 0) 
        {
            InternalCalls.Destroy(gameObject);
        }
	}

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Tail")
        {
            Debug.Log("Enemy HP: " + life);
            gameObject.SetImpulse(gameObject.transform.GetForward() * -4);
            //InternalCalls.Destroy(gameObject);
            //Le hace daño
        }
    }

    public void OnCollisionExit(GameObject other)
    {
        if (other.Tag == "Tail")
        {
            Debug.Log("Manel");
            gameObject.SetVelocity(new Vector3(0, 0, 0));
            gameObject.ClearForces();
        }
    }
}