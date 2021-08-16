﻿using UnityEngine;
using System.Collections;

public class BasicStartAnimation : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public string state_name = "Base Layer.Open";       // State to Start

    [Tooltip("Whether to Play Audio or Not.")]
    public bool play_audio = true;                      // SFX Flag

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Animator anim;                              // Animator Variable
    private AudioSource audio_source;                   // Audio Source Component

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Start Animation
    public void startAnimation()
    {
        anim.Play(state_name);                          // Play Animator

        if (play_audio)
        {
            audio_source.Play();                            // Play AudioFX
        }

        //Destroy(this);                                  // Destroy Script Component
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();              // Get Animator Component

        if (play_audio)
        {
            audio_source = GetComponentInChildren<AudioSource>();
        }

        if (anim == null)                                       // Check Animator Isn't Null
        {
            Debug.Log("Animator Not Found!");
        }
    }
}