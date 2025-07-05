using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class ExplosionDelete : YmirComponent
{
    public GameObject thisReference = null;

	private float deleteCounter;

    public void Start()
	{
		deleteCounter = 0f;
	}

	public void Update()
	{
        deleteCounter += Time.deltaTime;

        if (deleteCounter >= 1.5f)
		{
            InternalCalls.Destroy(gameObject);
        }
    }
}