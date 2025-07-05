using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmirEngine;

public class Spawner: YmirComponent
{
    public bool spawn = false;

    //private int maxEnemies = 2;

    public float porcentajeFaceHugger;
    public float porcentajeDroneXenomorph;
    public float porcentajeSpitter; 

    static Random random = new Random();

    //private int enemiesCounter;

    public List<GameObject> currentEnemies = null;

    public int typeOfEnemy = 0;

    public void Start()
    {
        spawn = false;
        currentEnemies = new List<GameObject>();
        
        //enemiesCounter = 0;
    }

    public void Update()
    {

        if(spawn)
        {
            Spawn(typeOfEnemy);
            spawn = false;
        }

    }


    public void Spawn(int enemyType)
    {

        double randomValue = random.NextDouble();
        Debug.Log("RandomValue" + randomValue);

        if(randomValue < porcentajeFaceHugger)
        {
            InternalCalls.CreateGOFromPrefab("Assets/Prefabs", "Enemy-FaceHugger-DEF", gameObject.transform.globalPosition);
        }
        else if (randomValue < (porcentajeSpitter + porcentajeFaceHugger))
        {
            InternalCalls.CreateGOFromPrefab("Assets/Prefabs", "Enemy-Spitter-DEF", gameObject.transform.globalPosition);
        }
        else
        {
            InternalCalls.CreateGOFromPrefab("Assets/Prefabs", "Enemy-DroneXenomorph-DEF", gameObject.transform.globalPosition);
        }



        //switch (enemyType)
        //{
        //    case 0:
        //    break;
        //    case 1:
        //        InternalCalls.CreateGOFromPrefab("Assets/Prefabs", "Enemy-FaceHugger", gameObject.transform.globalPosition);
        //        break;

        //    case 2:

        //        InternalCalls.CreateGOFromPrefab("Assets/Prefabs", "Enemy-DroneXenomorph", gameObject.transform.globalPosition);
        //        break;
        //}
       


    }





}
