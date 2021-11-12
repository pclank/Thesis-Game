using UnityEngine;
using System.Collections;

public class AnimatedShader : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Header("Transition Settings")]
    public float transition_speed = 0.2f;                       // Speed of Transition
    public float lower_bound = 0.0f;
    public float upper_bound = 1.0f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool forwards = true;                               // Whether Blending is Increasing

    private Material mat;

    // Use this for initialization
    void Start()
    {
        mat =  gameObject.GetComponent<MeshRenderer>().material; 
    }

    // Update is called once per frame
    void Update()
    {
        float next_state = gameObject.GetComponent<MeshRenderer>().material.GetFloat("BlendOpacity");

        if (forwards && next_state < upper_bound)
        {
            next_state += transition_speed;

            if (next_state >= upper_bound)
            {
                next_state = upper_bound;

                forwards = false;
            }
        }

        else if (!forwards && next_state > lower_bound)
        {
            next_state -= transition_speed;

            if (next_state <= lower_bound)
            {
                next_state = lower_bound;

                forwards = true;
            }
        }

        gameObject.GetComponent<MeshRenderer>().material.SetFloat("BlendOpacity", next_state);
    }
}