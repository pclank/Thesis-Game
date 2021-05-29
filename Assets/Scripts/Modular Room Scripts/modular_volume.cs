using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modular_volume : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************



    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool player_trigger = false;    // Player In/Out of Volume

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_trigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_trigger = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
