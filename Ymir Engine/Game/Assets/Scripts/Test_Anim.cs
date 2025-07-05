using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Test_Anim : YmirComponent
{
	public void Start()
	{
		//Animation.SetLoop(gameObject, "Raisen_Idle", true);
  //      Animation.SetLoop(gameObject, "Raisen_Walk", true);
  //      Animation.SetLoop(gameObject, "Raisen_Run", true);
  //      Animation.SetLoop(gameObject, "Raisen_Dash", true);

  //      Animation.SetResetToZero(gameObject, "Raisen_Die", false);

  //      Animation.AddBlendOption(gameObject, "Raisen_Idle", "Raisen_Walk", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Idle", "Raisen_Run", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Idle", "Raisen_Die", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Idle", "Raisen_Dash", 5.0f);

  //      Animation.AddBlendOption(gameObject, "Raisen_Walk", "Raisen_Idle", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Walk", "Raisen_Run", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Walk", "Raisen_Die", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Walk", "Raisen_Dash", 5.0f);

  //      Animation.AddBlendOption(gameObject, "Raisen_Run", "Raisen_Idle", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Run", "Raisen_Walk", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Run", "Raisen_Die", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Run", "Raisen_Dash", 5.0f);

  //      Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Idle", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Walk", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Die", 5.0f);
  //      Animation.AddBlendOption(gameObject, "Raisen_Dash", "Raisen_Run", 5.0f);

  //      Animation.PlayAnimation(gameObject, "Raisen_Idle");



    }

	public void Update()
	{
        //if (Input.GetKey(YmirKeyCode.W) == KeyState.KEY_DOWN)
        //    Animation.PlayAnimation(gameObject, "Raisen_Idle");

        //if (Input.GetKey(YmirKeyCode.S) == KeyState.KEY_DOWN)
        //    Animation.PlayAnimation(gameObject, "Raisen_Walk");

        //if (Input.GetKey(YmirKeyCode.A) == KeyState.KEY_DOWN)
        //    Animation.PlayAnimation(gameObject, "Raisen_Die");

        //if (Input.GetKey(YmirKeyCode.D) == KeyState.KEY_DOWN)
        //    Animation.PlayAnimation(gameObject, "Raisen_Run");

        if (Input.GetKey(YmirKeyCode.M) == KeyState.KEY_DOWN)
            Animation.PlayAnimation(gameObject, "Raisen_Die");
    }
}