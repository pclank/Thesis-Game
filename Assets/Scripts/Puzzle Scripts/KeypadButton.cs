using UnityEngine;
using System.Collections;

// ************************************************************************************
// Keypad Button Class
// ************************************************************************************

public class KeypadButton : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public int key_id = -1;

    void Start()
    {
        if (key_id == -1)
        {
            Debug.Log("Key ID Not Correctly Set!");
        }
    }
}