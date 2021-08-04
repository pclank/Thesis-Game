using UnityEngine;
using System;
using System.Collections;

public class RotatingRooms : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public float rotation = -0.2f;              // Degrees of Rotation per Frame
    public float delay = 5.0f;                  // Delay Between 90 Degree Rotations

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private float timer_start = 0.0f;           // Timer Start Time

    private int limiter;                        // Rotation Limiter
    private int counter = 0;                    // Rotation Counter

    private bool timer_on = false;              // Whether Timer is On

    // Use this for initialization
    void Start()
    {
        limiter = (int)(90.0f / Math.Abs(rotation)) + 1;     // Calculate Number of Frames Required for a 90 Degree Rotation
    }

    // Update is called once per frame
    void Update()
    {
        if (!timer_on)
        {
            transform.Rotate(new Vector3(0, 0, rotation), Space.Self);      // Rotate

            counter++;                                                      // Increment Counter
        }

        // If GameObject is in Position, Start Timer
        if (!timer_on && counter == limiter)
        {
            counter = 0;

            timer_on = true;

            timer_start = Time.time;
        }

        // Timer Section
        if (timer_on && Time.time - timer_start >= delay)
        {
            timer_on = false;
        }
    }
}