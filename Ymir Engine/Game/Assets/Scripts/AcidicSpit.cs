using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class AcidicSpit : YmirComponent
{
    private float maxDistance = 30.0f;
    private float speed = 130.0f;
    private float damage = 90.0f;
    private float time = 0.0f;
    public GameObject acidPuddle;
    private Vector3 vSpeed;

    public void Start()
    {
        time = 0;
        damage = 90;
        maxDistance = 60.0f;
        speed = 130.0f;
        vSpeed = GetDirection() * speed;
    }

    public void Update()
    {
        time += Time.deltaTime;

        ////Mover bala
        gameObject.SetVelocity(vSpeed);

        if (time > (maxDistance / speed))
        {
            KillBullet();
        }
    }

    public void OnCollisionStay(GameObject other)
    {
        //if (other.Tag == "Wall")
        //{
        //    Debug.Log("Collision with Wall!");
        //    KillBullet();
        //}
        if (other.Tag == "Enemy")
        {
            Debug.Log("Collision with Enemy!");
            //Hace daño al enemigo

            //other.GetComponent<Enemy>().life -= damage;
            FaceHuggerBaseScript aux = other.GetComponent<FaceHuggerBaseScript>();

            if (aux != null)
            {
                aux.life -= damage;
            }

            DroneXenomorphBaseScript aux2 = other.GetComponent<DroneXenomorphBaseScript>();
            if (aux2 != null)
            {
                aux2.life -= damage;
            }

            QueenXenomorphBaseScript aux3 = other.GetComponent<QueenXenomorphBaseScript>();
            if (aux3 != null)
            {
                aux3.life -= damage;
            }

            SpitterBaseScript aux4 = other.GetComponent<SpitterBaseScript>();
            if (aux4 != null)
            {
                aux4.life -= damage;
            }

            KillBullet();
        }
        else if (other.Tag == "Player")
        {
            //Evitar que el Player choque con el proyectil
        }
        else
        {
            KillBullet();
        }
    }

    void KillBullet()
    {
        //Crear charco de acido.
        InternalCalls.CreateAcidPuddle("AcidPudle", gameObject.transform.globalPosition);

        InternalCalls.Destroy(gameObject);
    }

    private Vector3 GetDirection()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");
        if (gameObject != null)
        {
            return gameObject.transform.GetForward();
        }
        else return new Vector3(0, 0, 0);
    }

}