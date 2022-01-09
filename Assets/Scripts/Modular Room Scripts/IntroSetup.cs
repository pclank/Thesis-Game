using UnityEngine;
using System.Collections;

// ************************************************************************************
// Intro Setup Controller
// ************************************************************************************

public class IntroSetup : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Intro UI GameObject.")]
    public GameObject intro_ui;

    [Tooltip("Intro Background UI GameObject.")]
    public GameObject intro_background;

    [Tooltip("Intro Duration.")]
    public float intro_duration = 0.0f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool intro_started = false;

    private float timer_value = 0.0f;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Timer Section
        if (intro_started && Time.time - timer_value >= intro_duration)
        {
            intro_ui.SetActive(false);

            Destroy(intro_ui);
        }
    }
}