﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour, Interfaces.IClickInputs, Interfaces.ITouchInputs
{
    public float deadZoneRadius = 0.75f;

    //Proportionnal to the acceleration rate
    public float accelerationRate;
    //Maximium attainable speed
    public float maximumSpeed;

    public float distanceFromBound;
    public float leftBound;
    public float rightBound;

    private float upBound;
    private float downBound;
    private float deadZoneRadiusSQR;

    public float brakeDistance = 1.5f;

    public Vector2 currentTarget;

    private Rigidbody2D rb;
    private float realBrakeDistance = -1;

    private Thruster thruster;
    private FishContainer fishContainer;

    private SlingshotControl slingshotControl;

    public bool inputEnable = true;

    public bool movementEnable = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        realBrakeDistance = brakeDistance;
    }

    void Start()
    {
        deadZoneRadiusSQR = deadZoneRadius * deadZoneRadius;
        currentTarget = new Vector2(transform.position.x, transform.position.y);

        Game.OnGameStart += Init;

        slingshotControl = GetComponent<SlingshotControl>();

        SubmarinParts parts = gameObject.GetComponent<SubmarinParts>();
        thruster = parts.GetThruster();
        if(thruster != null)
        {
            maximumSpeed = thruster.GetSpeed();
            accelerationRate = thruster.GetAcceleration();
        }

        fishContainer = parts.GetFishContainer();
    }

    void Init()
    {
        MapInfo m = Game.Instance.map;
        upBound = m.mapTop;
        downBound = m.mapBottom;
        return;
    }


    void FixedUpdate()
    {
        GetDraggingPosition();

        if (movementEnable == false)
        { 
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 distance = currentTarget - (Vector2)transform.position;
        Vector2 direction = distance.normalized;

        Vector2 targetSpeed;

        if (distance.magnitude < realBrakeDistance)
        {
            targetSpeed = distance.Capped(Vector2.one * maximumSpeed);
        }
        else
        {
            targetSpeed = direction * maximumSpeed;
        }

        Vector2 Speed = Vector2.Lerp(
            rb.velocity,
            targetSpeed,
            FixedLerp.Fix(0.1f * accelerationRate));
        rb.velocity = Speed;

    }

    public void GetDraggingPosition()
    {
        if (inputEnable)
        {
            Vector2 position;
            if (DragDetection.GetTouchPosition(out position) && slingshotControl.isDragging == false)
            {
                position = Game.GameCamera.cam.ScreenToWorldPoint(position);

                float sqrMag = (position - rb.position).sqrMagnitude;
                if (sqrMag > deadZoneRadiusSQR)
                {
                    float d = distanceFromBound;

                    currentTarget.x = position.x.Clamped(leftBound + d, rightBound - d);
                    currentTarget.y = position.y.Clamped(downBound + d, upBound - d);

                    realBrakeDistance = (0.2f + sqrMag * 0.5f).Capped(brakeDistance);
                }
            }
        }
    }


    public void OnClick(Vector2 position)
    {/*
        float sqrMag = (position - rb.position).sqrMagnitude;
        if (sqrMag > deadZoneRadiusSQR)
        {
            float d = distanceFromBound;

            currentTarget.x = position.x.Clamped(leftBound + d, rightBound - d);
            currentTarget.y = position.y.Clamped(downBound + d, upBound - d);

            realBrakeDistance = (0.2f + sqrMag * 0.5f).Capped(brakeDistance);
        }*/
    }

    public void OnTouch(Vector2 position)
    {
        OnClick(position);
    }

    FishContainer GetFishContainer()
    {
        return fishContainer;
    }

    public void PushAwayFromTarget(float duration, float speedBoost, float force)
    {
        inputEnable = false;
        Vector2 displacement = currentTarget - (Vector2)transform.position;

        float minDistance = 1;
        if (displacement.sqrMagnitude < (minDistance * minDistance))
            displacement = (displacement.normalized) * minDistance;

        currentTarget = (Vector2)transform.position + (displacement * -1 * force);

        maximumSpeed += speedBoost;

        TransformFlipper flipper = GetComponentInChildren<TransformFlipper>();
        if (flipper != null)
            flipper.enabled = false;

        TransformTilter tilter = GetComponentInChildren<TransformTilter>();
        if (tilter != null)
            tilter.enabled = false;

        this.DelayedCall(delegate ()
        {
            maximumSpeed -= speedBoost;
            inputEnable = true;
            flipper.enabled = true;
            tilter.enabled = true;
        },duration);
    }
}
