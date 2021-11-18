using UnityEngine;
using System.Collections;

// ************************************************************************************
// Script to Queue Multiple Animations on Different GameObjects
// ************************************************************************************

public class AnimationQueueing : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Array of GameObjects Containing Animators.")]
    public GameObject[] animation_objects = new GameObject[2];

    [Tooltip("Intervals between Animations.")]
    public float[] animation_intervals = new float[1];

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private float timer_value = 0.0f;                                   // Starting Value of Timer

    private bool timer_on = false;                                      // Timer Flag

    private int animation_index;                                        // Animation Index

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Start Animation Queue
    public void startQueue()
    {
        animation_index = 0;

        playAnimation();
    }

    // Play Animation
    private void playAnimation()
    {
        animation_objects[animation_index].GetComponent<Animator>().Play("Base Layer.Main");


        if (animation_index < animation_objects.Length)
        {
            timer_value = Time.time;

            timer_on = true;
        }
        else
            Destroy(this);
    }

    // Use this for initialization
    void Start()
    {
        // Make Sure Array Lengths are Correct
        if (animation_objects.Length - 1 != animation_intervals.Length)
        {
            Debug.LogError("Incompatible Array Lengths!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Timer Section
        if (timer_on && (Time.time - timer_value >= animation_intervals[animation_index]))
        {
            animation_index++;

            playAnimation();
        }
    }
}