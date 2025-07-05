using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public enum XenoState
{
    IDLE,
    IDLE_AGGRO,
    MOVE,
    MOVE_AGGRO,
    MOVE_BACKWARDS,
    CRY,
    ACID_SPIT,
    ACID_REBOUND,
    KNOCKBACK,
    DEAD,
    PAUSED
}

public class SpitterBaseScript : Enemy
{
    public GameObject thisReference = null;

    protected Vector3 targetPosition = null;

    private XenoState xenoState;
    private XenoState pausedState;

    //Attacks variables

    //If aggressive or not
    private bool aggro;
    //private float backwardsCooldownTime;

    //Acid spit
    //private float clawDamage;
    private float acidSpitCooldown;
    private float acidSpitCooldownTime;
    private float acidSpitRange;

    private float tooCloseRange;

    private float acidDamage = 350f;
    private float explosionDamage = 350f;

    private bool explosionEnabled = false;
    private float explosionCounter = 0f;

    //Acid rebound
    //private float tailDamage;
    private float acidExplosiveCooldown;
    private float acidExplosiveCooldownTime;

    //FOR GENERAL TIME MANAGEMENT
    public float timeCounter;
    private float timeLimit;

    private float outOfRangeTimer;

    private bool walkAni = false;
    private bool acidDone = false;
    private bool explosionDone = false;

    public GameObject particlesGO = null;

    public void Start()
    {
        //Base stuff
        xenoState = XenoState.IDLE;
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();
        boss = InternalCalls.GetGameObjectByName("Boss");
        if (boss != null) { bossScrit = boss.GetComponent<QueenXenomorphBaseScript>(); }
        agent = gameObject.GetComponent<PathFinding>();
        healthBar = InternalCalls.GetHealtBarObject(gameObject,8);

        //Agent
        agent.stoppingDistance = 2f;
        agent.speed = 900f; //Debería ser más lento que el facehugger (Antes estaba en 1600f)
        agent.angularSpeed = 10f;

        //ATTACKS

        aggro = false;
        detectionRadius = 120f;
        wanderRange = 100f;

        //Acid spit
        acidSpitCooldown = 4f;
        acidSpitCooldownTime = 3.5f;
        acidSpitRange = 100f;

        tooCloseRange = 60f;

        //Explosive spit
        acidExplosiveCooldown = 10f;
        acidExplosiveCooldownTime = 5f;

        //Time
        timeCounter = 0f;
        timeLimit = 1f;

        //Out of range timer
        outOfRangeTimer = 0f;

        paused = false;

        //Drop items
        keys = "Nombre:,Probabilidad:";
        path = "Assets/Loot Tables/spitter_loot.csv";
        numFields = 2;
        level = InternalCalls.GetCurrentMap();
        switch (level)
        {
            case 1:
                commonProb = 95.0f;
                rareProb = 5.0f;
                epicProb = 0.0f;
                break;
            case int i when (i == 2 || i == 3):
                commonProb = 93.0f;
                rareProb = 5.0f;
                epicProb = 2.0f;
                break;
            case int i when (i == 4 || i == 5):
                commonProb = 85.0f;
                rareProb = 10.0f;
                epicProb = 5.0f;
                break;
            default:
                commonProb = 93.0f;
                rareProb = 5.0f;
                epicProb = 2.0f;
                break;
        }

        life = 425f;
        armor = 0.0f;

        rarity = random.Next(101);

        Debug.Log("[ERROR]: " + rarity);

        if (rarity >= (101.0f - epicProb))
        {
            rarity = 2;
        }
        else if (rarity >= (101.0f - rareProb))
        {
            rarity = 1;
        }
        else
        {
            rarity = 0;
        }

        //Enemy rarity stats
        if (rarity == 1)
        {
            life = 687.5f; //763,88
            armor = 0.1f; //0.1f
            agent.speed = 990f; //Antes estaba en 1700f
            acidDamage = 317f;
            explosionDamage = 375f;
        }
        else if (rarity == 2)
        {
            life = 950; //1187,5
            armor = 0.2f; // 0.2f
            agent.speed = 1080f; //Antes estaba en 1800f
            acidDamage = 375f;
            explosionDamage = 440f;
        }

        SetColor();

        //Animations
        Animation.SetLoop(gameObject, "Idle_Spiter", true);
        Animation.SetLoop(gameObject, "Move_Spiter", true);
        Animation.SetLoop(gameObject, "Combat Idle_Spiter", true);

        Animation.SetResetToZero(gameObject, "Death_Spiter", false);

        Animation.SetSpeed(gameObject, "Move_Spitter", 0.1f);

        Animation.AddBlendOption(gameObject, "", "Idle_Spiter", 10f);
        Animation.AddBlendOption(gameObject, "", "Combat Idle_Spiter", 10f);
        Animation.AddBlendOption(gameObject, "", "Death_Spiter", 10f);
        Animation.AddBlendOption(gameObject, "", "Move_Spiter", 10f);
        Animation.AddBlendOption(gameObject, "", "Cry_Spiter", 10f);
        Animation.AddBlendOption(gameObject, "", "Atack_1_Spiter", 10f);
        Animation.AddBlendOption(gameObject, "", "Atack_2_Spiter", 10f);

        Animation.PlayAnimation(gameObject, "Idle_Spiter");
    }

    public void Update()
    {

        if (explosionEnabled)
        {
            explosionCounter += Time.deltaTime;
            if (explosionCounter >= 1.3f)
            {
                explosionCounter = 0f;
                explosionEnabled = false;
                particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesAcidSplit");
                Particles.PlayParticlesTrigger(particlesGO);
            }
        }

        if (CheckPause())
        {
            SetPause(true);
            paused = true;
            return;
        }
        else if (paused)
        {
            SetPause(false);
            paused = false;
        }
        //backwardsCooldownTime += Time.deltaTime;

        if (xenoState != XenoState.DEAD) { isDeath(); }

        switch (xenoState)
        {
            case XenoState.PAUSED:
                //Do nothing
                break;
            case XenoState.IDLE:

                timeCounter += Time.deltaTime;

                //Do wander if enough time has passed
                if (timeCounter >= timeLimit)
                {
                    //Animation.PlayAnimation(gameObject, "Drone_Walk");
                    timeCounter = 0f;
                    agent.CalculateRandomPath(gameObject.transform.globalPosition, wanderRange);
                    if (agent.GetPathSize() > 0)
                    {
                        targetPosition = agent.GetPointAt(agent.GetPathSize() - 1);
                        if (walkAni == false)
                        {
                            Animation.PlayAnimation(gameObject, "Move_Spiter");
                            walkAni = true;
                        }
                        xenoState = XenoState.MOVE;
                    }
                }

                //Check if player in radius and if so go to cry state
                if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, detectionRadius))
                {
                    player.GetComponent<Player>().SetExplorationAudioState();
                    timeLimit = 0.5f;
                    aggro = true;
                    Animation.PlayAnimation(gameObject, "Cry_Spiter");
                    walkAni = false;
                    Audio.PlayAudio(gameObject, "XS_Cry");
                    xenoState = XenoState.CRY;
                }

                break;
            case XenoState.MOVE:

                LookAt(agent.GetDestination());

                MoveToCalculatedPos(agent.speed);

                IsReached1(gameObject.transform.globalPosition, targetPosition);

                //Check if player in radius and if so go to cry state
                if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, detectionRadius))
                {
                    player.GetComponent<Player>().SetExplorationAudioState();
                    timeLimit = 0.5f;
                    aggro = true;
                    Animation.PlayAnimation(gameObject, "Cry_Spiter");
                    walkAni = false;
                    Audio.PlayAudio(gameObject, "XS_Cry");
                    xenoState = XenoState.CRY;
                }

                break;
            case XenoState.CRY:

                timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);

                //If done with animation, go to move aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    if (walkAni == false)
                    {
                        Animation.PlayAnimation(gameObject, "Move_Spiter");
                        walkAni = true;
                    }
                    xenoState = XenoState.MOVE_AGGRO;
                }

                break;
            case XenoState.IDLE_AGGRO:

                agent.CalculatePath(gameObject.transform.globalPosition, player.transform.globalPosition);
                //Rather look at player? May cause to do weird rotations
                //LookAt(agent.GetDestination());
                if (explosionEnabled == false)
                {
                    LookAt(player.transform.globalPosition);
                }

                if (!CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, acidSpitRange))
                {
                    if (walkAni == false)
                    {
                        Animation.PlayAnimation(gameObject, "Move_Spiter");
                        walkAni = true;
                    }
                    xenoState = XenoState.MOVE_AGGRO;
                }

                break;
            case XenoState.MOVE_AGGRO:

                agent.CalculatePath(gameObject.transform.globalPosition, player.transform.globalPosition);
                if (agent.GetPathSize() == 0)
                {
                    xenoState = XenoState.IDLE;
                }
                if (explosionEnabled == false)
                {
                    LookAt(player.transform.globalPosition);
                }

                MoveToCalculatedPos(agent.speed);

                break;
            case XenoState.MOVE_BACKWARDS:

                timeCounter += Time.deltaTime;

                agent.CalculatePath(gameObject.transform.globalPosition, player.transform.globalPosition);
                if (explosionEnabled == false)
                {
                    LookAt(player.transform.globalPosition);
                }

                MoveToCalculatedPos(-agent.speed);

                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    timeLimit = 1f;
                    Animation.SetBackward(gameObject, "Move_Spiter", false);
                    Animation.PlayAnimation(gameObject, "Idle_Spiter");
                    walkAni = false;
                    xenoState = XenoState.IDLE_AGGRO;
                }

                if (acidSpitCooldownTime >= acidSpitCooldown && xenoState != XenoState.DEAD)
                {
                    acidSpitCooldownTime = 0f;
                    timeCounter = 0f;
                    //ANIMATION DURATION HERE!!!
                    timeLimit = 0.8f;
                    Animation.PlayAnimation(gameObject, "Atack_1_Spiter");

                    ////PARTICLES
                    //particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesAttack1_Spitter");
                    //Particles.PlayParticlesTrigger(particlesGO);

                    walkAni = false;
                    Audio.PlayAudio(gameObject, "XS_Spit");
                    acidDone = false;
                    xenoState = XenoState.ACID_SPIT;
                    acidExplosiveCooldownTime -= 1.5f;
                    if (explosionEnabled == false)
                    {
                        LookAt(player.transform.globalPosition);
                    }
                }
                else if (acidExplosiveCooldownTime >= acidExplosiveCooldown && xenoState != XenoState.DEAD)
                {
                    acidExplosiveCooldownTime = 0f;
                    timeCounter = 0f;
                    //ANIMATION DURATION HERE!!!
                    timeLimit = 0.8f;
                    Animation.PlayAnimation(gameObject, "Atack_2_Spiter");
                    walkAni = false;

                    ////PARTICLES
                    //particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesAttack2_Spitter");
                    //Particles.PlayParticlesTrigger(particlesGO);

                    Audio.PlayAudio(gameObject, "XS_Rebound");
                    explosionDone = false;
                    xenoState = XenoState.ACID_REBOUND;
                    acidSpitCooldownTime -= 1.5f;
                    if (explosionEnabled == false)
                    {
                        LookAt(player.transform.globalPosition);
                    }
                }

                break;
            case XenoState.KNOCKBACK:

                KnockBack(knockBackSpeed);

                timePassed += Time.deltaTime;

                if (timePassed >= knockBackTimer)
                {
                    Animation.PlayAnimation(gameObject, "Idle_Spiter");
                    walkAni = false;
                    xenoState = XenoState.IDLE;
                    timePassed = 0f;
                }

                return;
            case XenoState.ACID_SPIT:

                timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);
                if (explosionEnabled == false)
                {
                    LookAt(player.transform.globalPosition);
                }

                //If done with animation, go to idle aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    Animation.PlayAnimation(gameObject, "Idle_Spiter");
                    walkAni = false;
                    xenoState = XenoState.IDLE_AGGRO;
                    acidExplosiveCooldownTime -= 1f;

                }
                else if (timeCounter >= 0.6f && acidDone == false)
                {
                    Vector3 pos = gameObject.transform.globalPosition;
                    pos.y += 10;
                    InternalCalls.CreateSpitterAcidSpit(pos, gameObject.transform.globalRotation, acidDamage);
                    //InternalCalls.CreateGOFromPrefab("Assets/Prefabs", "Projectile-SpitterAcidSpit", pos);

                    //PARTICLES
                    particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesAttack1_Spitter");
                    Particles.ParticlesForward(particlesGO, gameObject.transform.GetForward().normalized, 1, 80.0f);
                    Particles.ParticlesSetDirection(particlesGO, gameObject.transform.GetForward().normalized, 0);
                    Particles.ParticlesForward(particlesGO, gameObject.transform.GetForward().normalized, 2, 10.0f);
                    Particles.PlayParticlesTrigger(particlesGO);
                    acidDone = true;
                }

                break;
            case XenoState.ACID_REBOUND:

                timeCounter += Time.deltaTime;

                gameObject.SetVelocity(gameObject.transform.GetForward() * 0);
                if (explosionEnabled == false)
                {
                    LookAt(player.transform.globalPosition);
                }

                //If done with animation, go to idle aggro
                if (timeCounter >= timeLimit)
                {
                    timeCounter = 0f;
                    Animation.PlayAnimation(gameObject, "Idle_Spiter");
                    walkAni = false;
                    xenoState = XenoState.IDLE_AGGRO;
                    acidSpitCooldownTime -= 2f;
                }
                else if (timeCounter >= 0.6f && explosionDone == false)
                {
                    Vector3 pos = gameObject.transform.globalPosition;
                    pos.y += 10;
                    pos.z -= 10;
                    InternalCalls.CreateSpitterAcidExplosive(pos, gameObject.transform.globalRotation, explosionDamage);
                    //InternalCalls.CreateGOFromPrefab("Assets/Prefabs", "Projectile-SpitterExplosive", pos);

                    //PARTICLES
                    particlesGO = InternalCalls.GetChildrenByName(gameObject, "ParticlesAttack1_Spitter");
                    Particles.ParticlesForward(particlesGO, gameObject.transform.GetForward().normalized, 1, 80.0f);
                    Particles.ParticlesSetDirection(particlesGO, gameObject.transform.GetForward().normalized, 0);
                    Particles.ParticlesForward(particlesGO, gameObject.transform.GetForward().normalized, 2, 10.0f);
                    Particles.PlayParticlesTrigger(particlesGO);

                    explosionDone = true;
                    //GameObject particles = GetParticles(gameObject, "ParticlesAcidicEnemy");
                    //Particles.PlayParticlesTrigger(particles);
                    explosionEnabled = true;
                }

                break;
            case XenoState.DEAD:

                timePassed += Time.deltaTime;

                if (timePassed >= 1.4f)
                {
                    DropItem();
                    InternalCalls.Destroy(gameObject);
                }

                break;
        }
        //If the enemy isn't paused
        if (xenoState != XenoState.PAUSED)
        {
            //Walk backwards
            if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, tooCloseRange) && aggro == true)
            {
                if (xenoState != XenoState.ACID_SPIT && xenoState != XenoState.ACID_REBOUND && xenoState != XenoState.DEAD)
                {
                    timeCounter = 0f;
                    timeLimit = 0.8f;
                    Animation.SetBackward(gameObject, "Move_Spiter", true);
                    if (walkAni == false)
                    {
                        Animation.PlayAnimation(gameObject, "Move_Spiter");
                        walkAni = true;
                    }
                    xenoState = XenoState.MOVE_BACKWARDS;
                }
            }

            //Check attacks
            CheckAttacks();

            //If player too far away, go back to wander
            if (!CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, detectionRadius) && aggro == true)
            {
                outOfRangeTimer += Time.deltaTime;

                if (outOfRangeTimer >= 3f && xenoState != XenoState.DEAD)
                {
                    outOfRangeTimer = 0f;
                    timeCounter = 0f;
                    timeLimit = 5f;
                    aggro = false;
                    gameObject.SetVelocity(new Vector3(0, 0, 0));
                    Animation.PlayAnimation(gameObject, "Idle_Spiter");
                    walkAni = false;
                    xenoState = XenoState.IDLE;
                    player.GetComponent<Player>().SetExplorationAudioState();

                }
            }
            else
            {
                //So that it resets if it is again in range
                outOfRangeTimer = 0f;
            }
        }
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Tail" && xenoState != XenoState.KNOCKBACK && xenoState != XenoState.DEAD)
        {
            life -= 80;
            Animation.PlayAnimation(gameObject, "Idle_Spiter");
            walkAni = false;
            xenoState = XenoState.KNOCKBACK;
        }
    }

    public void IsReached1(Vector3 position, Vector3 destintion)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x),
                                      0,
                                      Mathf.Round(position.z));
        Vector3 roundedDestination = new Vector3(Mathf.Round(destintion.x),
                                                 0,
                                                 Mathf.Round(destintion.z));
        if ((roundedPosition.x == roundedDestination.x) && (roundedPosition.y == roundedDestination.y) && (roundedPosition.z == roundedDestination.z))
        {
            gameObject.SetVelocity(new Vector3(0, 0, 0));
            Animation.PlayAnimation(gameObject, "Idle_Spiter");
            walkAni = false;
            if (xenoState != XenoState.DEAD)
            {
                xenoState = XenoState.IDLE;
            }
        }
    }

    private void isDeath()
    {
        if (life <= 0 && xenoState != XenoState.DEAD)
        {
            gameObject.SetVelocity(new Vector3(0, 0, 0));
            itemPos = gameObject.transform.globalPosition;
            Animation.PlayAnimation(gameObject, "Death_Spiter");
            Audio.PlayAudio(gameObject, "XS_Death");
            xenoState = XenoState.DEAD;
            timePassed = 0;
        }
        if (bossScrit != null)
        {
            if (bossScrit.GetState() == QueenState.DEAD)
            {
                gameObject.SetVelocity(new Vector3(0, 0, 0));
                itemPos = gameObject.transform.globalPosition;
                Animation.PlayAnimation(gameObject, "Death_Spiter");
                Audio.PlayAudio(gameObject, "XS_Death");
                xenoState = XenoState.DEAD;
                timePassed = 0;
            }
        }
    }

    private void CheckAttacks()
    {
        acidSpitCooldownTime += Time.deltaTime;
        acidExplosiveCooldownTime += Time.deltaTime;

        if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, acidSpitRange) && acidSpitCooldownTime >= acidSpitCooldown)
        {
            if (xenoState != XenoState.ACID_REBOUND && xenoState != XenoState.DEAD)
            {
                acidSpitCooldownTime = 0f;
                timeCounter = 0f;
                //ANIMATION DURATION HERE!!!
                timeLimit = 0.8f;
                Animation.PlayAnimation(gameObject, "Atack_1_Spiter");
                walkAni = false;
                Audio.PlayAudio(gameObject, "XS_Spit");
                acidDone = false;
                xenoState = XenoState.ACID_SPIT;
                acidExplosiveCooldownTime -= 1.5f;
                LookAt(player.transform.globalPosition);
            }
        }
        else if (CheckDistance(player.transform.globalPosition, gameObject.transform.globalPosition, acidSpitRange) && acidExplosiveCooldownTime >= acidExplosiveCooldown)
        {
            if (xenoState != XenoState.ACID_SPIT && xenoState != XenoState.DEAD)
            {
                acidExplosiveCooldownTime = 0f;
                timeCounter = 0f;
                //ANIMATION DURATION HERE!!!
                timeLimit = 0.8f;
                Animation.PlayAnimation(gameObject, "Atack_2_Spiter");
                walkAni = false;
                Audio.PlayAudio(gameObject, "XS_Rebound");
                explosionDone = false;
                xenoState = XenoState.ACID_REBOUND;
                acidSpitCooldownTime -= 1.5f;
                LookAt(player.transform.globalPosition);
            }
        }
    }

    public XenoState GetState()
    {
        return xenoState;
    }

    private void SetPause(bool pause)
    {
        if (pause && !paused)
        {
            pausedState = xenoState;
            xenoState = XenoState.PAUSED;
            Animation.PauseAnimation(gameObject);
            gameObject.SetVelocity(gameObject.transform.GetForward() * 0);
        }
        else if (xenoState == XenoState.PAUSED)
        {
            //If bool set to false when it was never paused, it will do nothing
            xenoState = pausedState;
            Animation.ResumeAnimation(gameObject);
        }
    }

   

    private GameObject GetParticles(GameObject go, string pName)
    {
        return InternalCalls.GetChildrenByName(go, pName);
    }
}
