using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmirEngine;

public class SpawnSensor:YmirComponent
{
    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;
    public GameObject spawner4;
    public GameObject spawner5;
    public GameObject spawner6;
    public GameObject spawner7;
    public GameObject spawner8;

    private List<GameObject> spawnerList = new List<GameObject>();
    private Spawner spawnScript = null;

    private bool spawned;

    public void Start()
    {
        spawned = false;
        AddNonNullSpawners();
    }


    public void Update()
    {
        if (Input.GetKey(YmirKeyCode.X) == KeyState.KEY_DOWN)
        {
            spawnScript.spawn = true;
        }
    }

    private void AddNonNullSpawners()
    {
        if (spawner1 != null)
            spawnerList.Add(spawner1);
        if (spawner2 != null)
            spawnerList.Add(spawner2);
        if (spawner3 != null)
            spawnerList.Add(spawner3);
        if (spawner4 != null)
            spawnerList.Add(spawner4);
        if (spawner5 != null)
            spawnerList.Add(spawner5);
        if (spawner6 != null)
            spawnerList.Add(spawner6);
        if (spawner7 != null)
            spawnerList.Add(spawner7);
        if (spawner8 != null)
            spawnerList.Add(spawner8);


        
    }


    public void OnCollisionStay(GameObject other)
    {
        //Debug.Log("[ERROR] COLLISION");
        if (other.Tag == "Player" && !spawned)
        {
            foreach (GameObject spawn in spawnerList)
            {
                spawn.GetComponent<Spawner>().spawn = true;
            }
            spawned = true;
        }

    }

}

