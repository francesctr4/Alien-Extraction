using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Start : YmirComponent
{
	public void Start()
	{
		UI.SetFirstFocused(gameObject); // Script to set first UI when loading scene
	}

	public void Update()
	{
		return;
	}
}