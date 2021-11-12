using UnityEngine;
using System.Collections;

// ************************************************************************************
// Mirror Puzzle Control Script
// ************************************************************************************
public class MirrorPuzzle : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Touch UI GameObject.")]
    public GameObject touch_ui;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool ray_trig = false;
    private bool tape_is_played = false;                                                // Whether Tape Has Been Played

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Set Raycast
    public void setRaycast(bool flag)
    {
        ray_trig = flag;

        touch_ui.SetActive(flag);
    }

    // Set Tape Has Been Played
    public void setTapeIsPlayed()
    {
        tape_is_played = true;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}