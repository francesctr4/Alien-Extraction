using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Flesh_Door : YmirComponent
{
    public enum DoorState
    {
        OPENING,
        WAITING,
        CLOSING,
        CLOSED
    }

    private DoorState currentState;

    public float fraction = 0f;

    float timer = 0;
    float animDuration = 0.75f;

    private GameObject lDoor;
    private Vector3 initialPos_lDoor;
    private Vector3 initialScale_lDoor;
    private GameObject end_lDoor;

    private GameObject rDoor;
    private Vector3 initialPos_rDoor;
    private Vector3 initialScale_rDoor;
    private GameObject end_rDoor;

    private GameObject upDoor;
    private Vector3 initialPos_upDoor;
    private GameObject end_upDoor;

    public void Start()
    {
        lDoor = InternalCalls.CS_GetChild(gameObject, 1);
        end_lDoor = InternalCalls.CS_GetChild(gameObject, 3);
        rDoor = InternalCalls.CS_GetChild(gameObject, 2);
        end_rDoor = InternalCalls.CS_GetChild(gameObject, 4);
        upDoor = InternalCalls.CS_GetChild(gameObject, 0);
        end_upDoor = InternalCalls.CS_GetChild(gameObject, 5);

        initialPos_lDoor = lDoor.transform.localPosition;
        initialScale_lDoor = lDoor.transform.localScale;
        initialPos_rDoor = rDoor.transform.localPosition;
        initialScale_rDoor = rDoor.transform.localScale;
        initialPos_upDoor = upDoor.transform.localPosition;

        currentState = DoorState.CLOSED;
    }

    public void Update()
    {
        switch (currentState)
        {
            case DoorState.OPENING:
                timer += Time.deltaTime;
                float fraction = Mathf.Clamp01(timer / animDuration);

                lDoor.transform.localPosition = Vector3.Lerp(initialPos_lDoor, end_lDoor.transform.localPosition, fraction);
                lDoor.transform.localScale = Vector3.Lerp(initialScale_lDoor, end_lDoor.transform.localScale, fraction);

                rDoor.transform.localPosition = Vector3.Lerp(initialPos_rDoor, end_rDoor.transform.localPosition, fraction);
                rDoor.transform.localScale = Vector3.Lerp(initialScale_lDoor, end_rDoor.transform.localScale, fraction);

                upDoor.transform.localPosition = Vector3.Lerp(initialPos_upDoor, end_upDoor.transform.localPosition, fraction);
                if (timer >= animDuration)
                {
                    currentState = DoorState.WAITING;
                    timer = 0;
                }
                break;
            case DoorState.WAITING:
                timer += Time.deltaTime;
                lDoor.transform.localPosition = end_lDoor.transform.localPosition;
                rDoor.transform.localPosition = end_rDoor.transform.localPosition;
                upDoor.transform.localPosition = end_upDoor.transform.localPosition;
                if (timer >= animDuration)
                {
                    currentState = DoorState.CLOSING;
                    timer = 0;
                }
                break;
            case DoorState.CLOSING:
                timer += Time.deltaTime;
                fraction = Mathf.Clamp01(timer / animDuration);
                lDoor.transform.localPosition = Vector3.Lerp(end_lDoor.transform.localPosition, initialPos_lDoor, fraction);
                lDoor.transform.localScale = Vector3.Lerp(end_lDoor.transform.localScale, initialScale_lDoor, fraction);

                rDoor.transform.localPosition = Vector3.Lerp(end_rDoor.transform.localPosition, initialPos_rDoor, fraction);
                rDoor.transform.localScale = Vector3.Lerp(end_rDoor.transform.localScale, initialScale_rDoor, fraction);

                upDoor.transform.localPosition = Vector3.Lerp(end_upDoor.transform.localPosition, initialPos_upDoor, fraction);
                if (timer >= animDuration)
                {
                    currentState = DoorState.CLOSED;
                }
                break;
            case DoorState.CLOSED:
                // Puerta cerrada, no hay acción necesaria
                break;
        }
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player" && currentState == DoorState.CLOSED)
        {
            currentState = DoorState.OPENING;
            timer = 0;
        }
        else if (other.Tag == "Player" && currentState == DoorState.CLOSING)
        {
            currentState = DoorState.OPENING;
            timer = animDuration - timer;
        }
    }
}