using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

// ************************************************************************************
// Ending Quotes List JSON Class
// ************************************************************************************
[System.Serializable]
public class EndingQuotes
{
    public Quotes[] ending_quotes;
}

// ************************************************************************************
// Ending Quote JSON Class
// ************************************************************************************
[System.Serializable]
public class Quotes
{
    public Quote[] quotes;
}

// ************************************************************************************
// Ending Quote String Class
// ************************************************************************************
[System.Serializable]
public class Quote
{
    public string quote;
}

// ************************************************************************************
// Ending Setup Code
// ************************************************************************************

public class EndingSetup : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Header("Ending Music Setup")]
    [Tooltip("Happy Music Wwise Event.")]
    public AK.Wwise.Event happy_event;

    [Tooltip("Sad Music Wwise Event.")]
    public AK.Wwise.Event sad_event;

    [Tooltip("Angry Music Wwise Event.")]
    public AK.Wwise.Event angry_event;

    [Header("Ending Page Setup")]
    [Tooltip("JSON File Containing Entries.")]
    public TextAsset json_file;

    [Tooltip("Main UI Page.")]
    public GameObject main_ui_page;

    [Tooltip("Main UI Page Text.")]
    public GameObject main_ui_page_text;

    [Tooltip("Main UI Page Secondary Text.")]
    public GameObject main_ui_page_text_sec;

    [Tooltip("End It UI GameObject.")]
    public GameObject end_it_ui;

    [Tooltip("Press ESC UI GameObject.")]
    public GameObject press_esc_ui;

    [Tooltip("Delay for FX.")]
    public float delay = 0.1f;

    [Header("Environment Setup")]
    [Tooltip("Happy Columns.")]
    public GameObject happy_columns;
    
    [Tooltip("Sad Columns.")]
    public GameObject sad_columns;

    [Tooltip("Angry Columns.")]
    public GameObject angry_columns;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;
    private GameObject camera_object;

    private EndingQuotes quotes_list;

    private bool ray_trig = false;
    private bool ran = false;
    private bool fx_done = false;
    private bool sec_fx_done = false;
    private bool hint_displayed = false;

    private float timer_value;
    private float press_esc_timer_value;
    private float ending_opacity = 0.0f;

    private int emotion_index;

    private string page_text;

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Set Raycast Flag
    public void setRayTrig(bool flag)
    {
        ray_trig = flag;

        end_it_ui.SetActive(flag);
    }

    // Get Random Quote from Index
    private string getRandomQuote(int index)
    {
        System.Random rnd = new System.Random();

        int r_int = rnd.Next(0, 2);

        return quotes_list.ending_quotes[index].quotes[r_int].quote;
    }

    // Display Ending
    private void displayEnding()
    {
        player_object.GetComponent<FirstPersonMovement>().enabled = false;
        camera_object.GetComponent<FirstPersonLook>().enabled = false;

        main_ui_page.SetActive(true);

        player_object.GetComponent<RoomVolumeAnalytics>().recordAnalytics();
        player_object.GetComponent<JSONReader>().recordAnalytics();
    }

    // Use this for initialization
    void OnEnable()
    {
        player_object = GameObject.FindWithTag("Player");
        camera_object = GameObject.FindWithTag("MainCamera");

        quotes_list = JsonUtility.FromJson<EndingQuotes>(json_file.text);                                       // Deserialize JSON File

        emotion_index = player_object.GetComponent<JSONReader>().getHighestOccurrence();                        // Get Highest Occurring Emotion

        page_text = getRandomQuote(emotion_index);                                                              // Get Random Quote from Index

        main_ui_page_text.GetComponent<Text>().text = page_text;                                                // Set Page Text

        main_ui_page_text.GetComponent<Text>().color = new Color(main_ui_page_text.GetComponent<Text>().color.r, main_ui_page_text.GetComponent<Text>().color.g, main_ui_page_text.GetComponent<Text>().color.b, 0.0f);
        main_ui_page_text_sec.GetComponent<Text>().color = new Color(main_ui_page_text_sec.GetComponent<Text>().color.r, main_ui_page_text_sec.GetComponent<Text>().color.g, main_ui_page_text_sec.GetComponent<Text>().color.b, 0.0f);

        player_object.GetComponent<WwiseDynamicMusic>().stopAllPlayback();                                      // Stop All Dynamic Music Playback
        player_object.GetComponent<WwiseDynamicMusic>().enabled = false;                                        // Disable Dynamic Music System

        // Start Ending Music and Setup Environment

        if (emotion_index == 0)
        {
            happy_event.Post(gameObject);

            Destroy(sad_columns);
            Destroy(angry_columns);

            happy_columns.SetActive(true);
        }
        else if (emotion_index == 1)
        {
            sad_event.Post(gameObject);

            Destroy(happy_columns);
            Destroy(angry_columns);

            sad_columns.SetActive(true);
        }
        else if (emotion_index == 2 || emotion_index == 3)
        {
            angry_event.Post(gameObject);

            Destroy(happy_columns);
            Destroy(sad_columns);

            angry_columns.SetActive(true);
        }
    }

    void Update()
    {
        if (!ran && ray_trig && Input.GetKeyUp(KeyCode.Mouse0))
        {
            displayEnding();

            end_it_ui.SetActive(false);

            ran = true;
        }
        // Ending Effect
        if (ran && !fx_done && Time.time - timer_value >= delay && ending_opacity < 1.0f)
        {
            ending_opacity += 0.08f;

            // Edge Case
            if (ending_opacity >= 1.0f)
            {
                ending_opacity = 1.0f;

                fx_done = true;
            }

            else
                timer_value = Time.time;

            main_ui_page_text.GetComponent<Text>().color = new Color(main_ui_page_text.GetComponent<Text>().color.r, main_ui_page_text.GetComponent<Text>().color.g, main_ui_page_text.GetComponent<Text>().color.b, ending_opacity);

            if (ending_opacity == 1.0f)
            {
                ending_opacity = 0.0f;

                timer_value = Time.time;
            }
        }
        // Secondary Ending Effect
        else if (fx_done && !sec_fx_done && Time.time - timer_value >= delay && ending_opacity < 1.0f)
        {
            ending_opacity += 0.08f;

            // Edge Case
            if (ending_opacity >= 1.0f)
            {
                ending_opacity = 1.0f;

                press_esc_timer_value = Time.time;

                sec_fx_done = true;
            }

            else
                timer_value = Time.time;

            main_ui_page_text_sec.GetComponent<Text>().color = new Color(main_ui_page_text_sec.GetComponent<Text>().color.r, main_ui_page_text_sec.GetComponent<Text>().color.g, main_ui_page_text_sec.GetComponent<Text>().color.b, ending_opacity);
        }

        // Timer for Press ESC Hint
        if (sec_fx_done && !hint_displayed && Time.time - press_esc_timer_value >= 5.0f)
        {
            press_esc_ui.SetActive(true);

            hint_displayed = true;
        }
    }
}