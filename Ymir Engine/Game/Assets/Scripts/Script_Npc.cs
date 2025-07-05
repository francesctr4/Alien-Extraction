using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Script_Npc : YmirComponent
{
    private GameObject canvas;

    public void Start()
    {
        canvas = InternalCalls.GetGameObjectByName("Npc_Canvas");
    }

    public void Update()
    {
       


    } 

    public void OnCollisionStay(GameObject other)
	{
        if (other.Tag == "Player")
		{
            canvas.SetActive(true);

        }
	}

    public void OnCollisionExit(GameObject other)
    {
        if (other.Tag == "Player")
        {
            canvas.SetActive(false);
        }
    }
}