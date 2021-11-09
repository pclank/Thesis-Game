using UnityEngine;
using System.Collections;

// ************************************************************************************
// Lantern FX Animation Controller
// ************************************************************************************

public class LanternFX : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Target of Force to be Applied.")]
    public GameObject force_tgt;

    [Tooltip("Initial Delay.")]
    public bool initial_delay = true;

    [Tooltip("Unfreeze On Start.")]
    public bool unfreeze = true;

    [Tooltip("Measure of Force.")]
    public float force_value = 1.0f;

    [Tooltip("Delay Between Force Applications.")]
    public float delay = 5.0f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Rigidbody rigid_body;                                       // GameObject Rigidbody

    private bool timer_on = false;                                      // Timer Flag

    private float timer_value = 0.0f;                                   // Timer Value

    // Use this for initialization
    void Start()
    {
        rigid_body = GetComponent<Rigidbody>();

        timer_on = initial_delay;
        timer_value = Time.time;
    }

    // Update is called once per fixed frame
    void FixedUpdate()
    {
        // Timer Section
        if (timer_on && (Time.time - timer_value >= delay))
        {
            timer_on = false;

            // Unfreeze Section
            if (unfreeze)
            {
                rigid_body.constraints = RigidbodyConstraints.None;
                rigid_body.velocity = Vector3.zero;

                unfreeze = false;
            }
        }
        // FX Section
        else if (!timer_on)
        {
            rigid_body.AddForce(0.0f, 0.0f, force_value);

            timer_value = Time.time;

            timer_on = true;
        }
    }
}