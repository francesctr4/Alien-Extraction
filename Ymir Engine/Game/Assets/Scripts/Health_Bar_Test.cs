using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;


public class Health_Bar_Test : YmirComponent
{
    public float HP = 500;
    public float initialHP = 500;
    public Vector3 initialScale;

    private GameObject enemy = null;
    public GameObject plane = null;

    private FaceHuggerBaseScript aux = null;
    private DroneXenomorphBaseScript aux2 = null;
    private SpitterBaseScript aux3 = null;
    private QueenXenomorphBaseScript aux4 = null;

    public void Start()
    {
        enemy = InternalCalls.GetEnemyGameObject(gameObject);
        plane = InternalCalls.CS_GetChild(gameObject, 0);
        initialScale = plane.transform.localScale;

        aux = enemy.GetComponent<FaceHuggerBaseScript>();
        aux2 = enemy.GetComponent<DroneXenomorphBaseScript>();
        aux3 = enemy.GetComponent<SpitterBaseScript>();
        aux4 = enemy.GetComponent<QueenXenomorphBaseScript>();

        SetInitialHP();
        initialHP = HP;
    }

    public void Update()
    {
        gameObject.transform.localPosition = new Vector3(enemy.transform.localPosition.x, gameObject.transform.localPosition.y, enemy.transform.localPosition.z);


        gameObject.SetAsBillboard();

        float scaleX = Mathf.Max(0, initialScale.x * (HP / initialHP));
        Vector3 newScale = new Vector3(scaleX, initialScale.y, initialScale.z);
        plane.SetScale(newScale);

        //---------------Xiao: Gurrada Pendiente de Cambiar----------------------------
        if (aux != null)
        {
            HP = aux.life;
            if(aux.GetState() == WanderState.DEATH)
            {
                Destroy();
            }
        }

        if (aux2 != null)
        {
            HP = aux2.life;
            if (aux2.GetState() == DroneState.DEAD)
            {
                Destroy();
            }
        }

        if (aux3 != null)
        {
            HP = aux3.life;
            if (aux3.GetState() == XenoState.DEAD)
            {
                Destroy();
            }
        }


        if (aux4 != null)
        {
            HP = aux4.life;
            if (aux4.GetState() == QueenState.DEAD)
            {
                Destroy();
            }
        }
    }

    public void SetInitialHP()
    {
        if (aux != null)
        {
            HP = aux.life;

        }

        if (aux2 != null)
        {
            HP = aux2.life;

        }

        if (aux3 != null)
        {
            HP = aux3.life;

        }

        if (aux4 != null)
        {
            HP = aux4.life;

        }
    }
    public void Destroy()
    {
        InternalCalls.Destroy(gameObject);
    }

}
