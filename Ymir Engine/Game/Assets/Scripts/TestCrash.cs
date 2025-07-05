using System;
using YmirEngine;

public class TestCrash : YmirComponent
{
    public GameObject hola = null;
    public void Update()
    {
        hola.GetType();
    }

}