using UnityEngine;
using System.Collections;

public class AnimatedShader : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public float transition_speed = 0.2f;                       // Speed of Transition

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

        if (forwards && next_state < 1.0f)
        {
            next_state += transition_speed;

            if (next_state >= 1.0f)
            {
                next_state = 1.0f;

                forwards = false;
            }
        }

        else if (!forwards && next_state > 0.0f)
        {
            next_state -= transition_speed;

            if (next_state <= 0.0f)
            {
                next_state = 0.0f;

                forwards = true;
            }
        }

        gameObject.GetComponent<MeshRenderer>().material.SetFloat("BlendOpacity", next_state);
    }
}