using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YmirEngine;


public class CameraRot : YmirComponent
{
    //--------------------- Controller var ---------------------\\

    //position difference between camera and player
    public Vector3 difPos = new Vector3(-58, 111, 63);

    private GameObject target;
    private Player player;

    // Timers
    private float idleTimer;
    private float delayTimer;

    private float idleTimerMax = 2.0f;
    private float delayTimerMax = 0.6f;

    private bool delay;

    public const float constDelay = 0.3f;

    public void Start()
    {
        idleTimer = 0.0f;
        delayTimer = 0.0f;

        delay = false;

        target = InternalCalls.GetGameObjectByName("Player");
        if (target != null)
        {
            player = target.GetComponent<Player>();
        }

        Audio.PlayEmbedAudio(gameObject);
    }

    public void Update()
    {
        // Follow Player (If unrelated stuff gets added, write above this comment)

        Vector3 newpos = target.transform.localPosition + difPos;

        float distance = Vector3.Distance(gameObject.transform.localPosition, newpos);

        if (player.currentState == Player.STATE.IDLE)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer > idleTimerMax)
            {

                delay = true;
            }
        }
        else
        {
            idleTimer = 0.0f;

        }

        if (delay)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer < delayTimerMax)
            {
                return;
            }
            else
            {
                delay = false;
                delayTimer = 0.0f;
            }
        }

        gameObject.transform.localPosition = (Vector3.Lerp(gameObject.transform.localPosition, newpos, Time.deltaTime * distance * constDelay));
        //gameObject.SetPosition(Vector3.Lerp(gameObject.transform.globalPosition, newpos, Time.deltaTime * distance * constDelay));
    }
}

