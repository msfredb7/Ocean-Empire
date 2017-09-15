﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour, Interfaces.IClickInputs {

        //Proportionnal to the acceleration rate
    public float accelerationRate;
        //Maximium attainable speed
    public float maximumSpeed;
        //Between 0 and 
    public float brakeRatio = 0.5F;
    private float moveAccuracy = 1.0F;

    private Vector2 currentTarget;

    public Rigidbody2D rb;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = new Vector2(transform.position.x, transform.position.y);
    }


    void FixedUpdate()
    {
       
        Vector2 distance = currentTarget - new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = distance.normalized;


        if (distance.magnitude < 4)
        {
            
            Debug.Log(rb.velocity.magnitude);
            /*
                        Vector2 deceleration = -rb.velocity;

                        if (rb.velocity.sqrMagnitude > 1)
                            deceleration = (-rb.velocity).normalized;

                        rb.AddForce(deceleration * accelerationRate);
                        */
        }

        else if (Vector2.Dot(rb.velocity, direction) < maximumSpeed)
        {


            Vector2 perpSpeed = rb.velocity - Vector2.Dot(rb.velocity, direction) * direction;
            float perpSpeedPourcent = (perpSpeed.magnitude / maximumSpeed) * brakeRatio;

            Vector2 thrustersForce = -perpSpeed.normalized * perpSpeedPourcent + direction * (1 - perpSpeedPourcent.Capped(1)) * accelerationRate;
            rb.AddForce(thrustersForce);

            rb.AddForce(direction * accelerationRate);


        }
    }
    // Update is called once per frame
    void Update() {

       
    }


    public void OnClick(Vector2 position)
    {
        currentTarget = position;
    }
}