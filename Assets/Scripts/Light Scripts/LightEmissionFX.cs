using UnityEngine;
using System.Collections;

public class LightEmissionFX : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public Material tgt_material;               // Target Material

    public float delay = 1.0f;                  // Delay Between Material Change

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Material default_material;          // Default Material

    private float timer_start = 0.0f;           // Timer Start Time

    private bool timer_on = false;              // Timer On Flag

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Start Indicator FX
    public void startIndicator()
    {
        increaseEmission();     // Turn On

        timer_on = true;        // Start Timer
    }

    // Increase Emission
    private void increaseEmission()
    {
        gameObject.GetComponent<MeshRenderer>().material = tgt_material;        // Set New Material
    }

    // Decrease Emission
    private void decreaseEmission()
    {
        gameObject.GetComponent<MeshRenderer>().material = default_material;    // Set Default Material
    }

    // Use this for initialization
    void Start()
    {
        default_material = gameObject.GetComponent<MeshRenderer>().material;    // Get Default Material
    }

    // Update is called once per frame
    void Update()
    {
        // Timer Section
        if (timer_on && Time.time - timer_start >= delay)
        {
            decreaseEmission();         // Turn Off

            timer_on = false;           // Turn Timer Off
        }
    }
}