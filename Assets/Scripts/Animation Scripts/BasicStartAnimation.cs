using UnityEngine;
using System.Collections;

public class BasicStartAnimation : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public string state_name = "Base Layer.Open";       // State to Start

    [Tooltip("Whether to Play Audio or Not.")]
    public bool play_audio = true;                      // SFX Flag

    [Tooltip("Wwise Event to Play.")]
    public AK.Wwise.Event event_to_play;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Animator anim;                              // Animator Variable

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Start Animation
    public void startAnimation()
    {
        anim.Play(state_name);                          // Play Animator

        if (play_audio)
        {
            event_to_play.Post(gameObject);                 // Play AudioFX
        }

        //Destroy(this);                                  // Destroy Script Component
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Animator>() == null)
            anim = GetComponentInChildren<Animator>();              // Get Animator Component
        else
            anim = GetComponent<Animator>();                        // Get Animator Component

        if (anim == null)                                       // Check Animator Isn't Null
        {
            Debug.Log("Animator Not Found!");
        }
    }
}