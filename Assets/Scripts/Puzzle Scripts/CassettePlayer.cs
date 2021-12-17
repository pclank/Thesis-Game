using UnityEngine;
using System.Collections;

// ************************************************************************************
// Cassette Player Class
// ************************************************************************************

public class CassettePlayer : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Wwise Event to Trigger.")]
    public AK.Wwise.Event play_event;

    [Tooltip("Button SFX Event.")]
    public AK.Wwise.Event button_event;

    [Tooltip("Target Puzzle GameObject to Send \"Tape Played\" Message to.")]
    public GameObject target_puzzle;

    [Tooltip("Timer Delay for UI Messages.")]
    public float delay = 2.0f;

    [Tooltip("Cassette Player ID.")]
    public int player_id = 0;

    [Tooltip("Item ID of Expected Tape.")]
    public int item_id = -1;

    [Tooltip("Audio ID for Subtitles System.")]
    public uint audio_id = 0;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool tape_in = false;                                                   // Whether a Tape is Placed in Player
    private bool tape_playing = false;                                              // Whether a Tape is Playing
    private bool was_played = false;                                                // Whether the Tape was Played
    private bool ray_trig = false;                                                  // Raycast Trigger Flag
    private bool timer_on = false;                                                  // Timer On Flag

    private float timer_value = 0.0f;                                               // Timer Value

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Call Externally, and Place Tape in Player
    public bool placeInPlayer(Item item)
    {
        if (!tape_in && item.getID() == item_id)
        {
            tape_in = true;

            ray_trig = false;

            GameObject.FindWithTag("Player").GetComponent<AuxiliaryUI>().controlUI(1, false);

            return true;
        }
        else
        {
            return false;
        }
    }

    // Function to Externally Set the Raycast Trigger Flag
    public void setRaycast(bool flag)
    {
        if (!GameObject.FindWithTag("Player").GetComponent<main_inventory>().isInventoryOpen())
        ray_trig = flag;

        GameObject.FindWithTag("Player").GetComponent<AuxiliaryUI>().controlUI(1, flag);
    }

    // Get Raycast Trigger Flag
    public bool getRaycast()
    {
        return ray_trig;
    }

    // Play Tape Audio Clip
    private void playTape()
    {
        button_event.Post(gameObject);

        // Check Whether a Tape is Placed
        if (tape_in)
        {
            play_event.Post(gameObject);

            target_puzzle.GetComponent<MirrorPuzzle>().setTapeIsPlayed();                               // Send Message to Puzzle

            GameObject.FindWithTag("Player").GetComponent<SubtitleControl>().startSubtitles(audio_id);  // Start Subtitles

            tape_playing = true;

            if (!was_played)
            {
                was_played = true;

                // Add Information to Journal
                if (GetComponent<AddToJournalOnInteraction>() != null)
                    GetComponent<AddToJournalOnInteraction>().addToJournal();
            }
        }
        else
        {
            GameObject.FindWithTag("Player").GetComponent<AuxiliaryUI>().controlUI(2, true);

            timer_value = Time.time;
            timer_on = true;
        }
    }

    // Stop Tape Audio Clip
    private void stopTape()
    {
        play_event.Stop(gameObject);

        GameObject.FindWithTag("Player").GetComponent<SubtitleControl>().stopSubtitles();  // Stop Subtitles

        tape_playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Timer Section
        if (timer_on && (Time.time - timer_value >= delay))
        {
            GameObject.FindWithTag("Player").GetComponent<AuxiliaryUI>().controlUI(2, false);

            timer_on = false;
        }

        // Click Section
        if (ray_trig && Input.GetKeyUp(KeyCode.Mouse0) && !GameObject.FindWithTag("Player").GetComponent<main_inventory>().isInventoryOpen())
        {
            if (!tape_playing)
                playTape();
            else
                stopTape();
        }
    }
}