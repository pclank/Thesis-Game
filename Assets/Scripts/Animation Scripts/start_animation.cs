using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_animation : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject ui_open;                          // UI Open Text GameObject
    public GameObject ui_missing;                       // UI Item Missing Text GameObject

    public string state_name = "Base Layer.Open";       // State to Start
    public int it_id = 0;                               // Item ID to Query

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Animator anim;                              // Animator Variable
    private AudioSource audio_source;                   // Audio Source Component
    private GameObject player_object;                   // Player GameObject

    private bool player_trig = false;                   // Player in Collider
    private bool attempted = false;                     // Failed Attempt Flag

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_trig = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_trig = false;
            attempted = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();              // Get Animator Component
        audio_source = GetComponentInChildren<AudioSource>();
        player_object = GameObject.FindWithTag("Player");       // Get Player GameObject

        if (it_id == 0)                                         // Check Item ID Has Being Set
        {
            Debug.Log("ID Hasn't Being Set!");
        }

        if (anim == null)                                       // Check Animator Isn't Null
        {
            Debug.Log("Animator Not Found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player_trig)                                        // Player in Collider
        {
            if (!attempted)
            {
                ui_open.SetActive(true);
            }

            bool query_result = player_object.GetComponent<main_inventory>().startQuery(it_id);     // Initialize Query Result

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (query_result)
                {
                    ui_open.SetActive(false);

                    anim.Play(state_name);                          // Play Animator
                    audio_source.Play();                            // Play AudioFX

                    Destroy(this);                                  // Destroy Script Component
                }
                
                else
                {
                    ui_open.SetActive(false);
                    ui_missing.SetActive(true);

                    attempted = true;
                }
            }
        }

        else
        {
            ui_missing.SetActive(false);
            ui_open.SetActive(false);
        }
    }
}
