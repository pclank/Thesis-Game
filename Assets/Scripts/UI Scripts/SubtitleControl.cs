using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// ************************************************************************************
// Subtitle List JSON Object Class
// ************************************************************************************
[System.Serializable]
public class Jsubtitles_list
{
    public Jsubtitles[] subtitles_list;
}

// ************************************************************************************
// Subtitle JSON Object Class
// ************************************************************************************
[System.Serializable]
public class Jsubtitles
{
    private uint audio_id;                                              // ID of Audio Object

    private string audio_title;                                         // Title of Audio Object

    private Jline[] lines;                                              // List of Subtitle Line Objects

    public uint getAudioId()
    {
        return this.audio_id;
    }

    public Jline getLine(int index)
    {
        return this.lines[index];
    }
}

// ************************************************************************************
// Subtitle Line JSON Object Class
// ************************************************************************************
[System.Serializable]
public class Jline
{
    public float timestamp { get; }                                     // Timestamp for the Line to Appear
    public float duration { get; }                                      // Duration of which Line will Appear

    public string line { get; }                                         // Subtitle Line
    public string style { get; }                                        // Style of Line to be Parsed by Unity
}

// ************************************************************************************
// Subtitle Parse & Control Script
// ************************************************************************************

public class SubtitleControl : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("JSON File with Subtitles.")]
    public TextAsset json_file;

    [Tooltip("GameObject of Subtitle UI.")]
    public GameObject subtitle_ui;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Jsubtitles_list subtitle_list;                              // Deserialized JSON File

    private Jsubtitles selected_subtitles;                              // Selected Subtitles from List

    private int line_index = 0;                                         // Subtitle List Line Index

    private float line_duration;                                        // Duration of Current Line

    private float line_start_time;                                      // Start Time of Current Line

    private bool timer_on = false;                                      // Whether Timer is On

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Select and Start Subtitles
    public bool startSubtitles(uint audio_id)
    {
        // TODO: Add Functionality!

        return true;
    }

    // Set and Show Subtitle UI
    private void showSubtitles()
    {
        // Parse JSON Object
        if (selected_subtitles != null)
        {
            subtitle_ui.GetComponent<Text>().text = selected_subtitles.getLine(line_index).line;        // Set Line
        }


        line_start_time = Time.time;                                            // Set Start Time

        timer_on = true;                                                        // Enable Timer

        subtitle_ui.SetActive(true);                                            // Enable UI
    }

    // Clear and Hide Subtitle UI
    private void hideSubtitles()
    {
        subtitle_ui.SetActive(false);                                           // Disable UI

        subtitle_ui.GetComponent<Text>().text = "";                             // Clear Text
    }

    // Use this for initialization
    void Start()
    {
        subtitle_list = JsonUtility.FromJson<Jsubtitles_list>(json_file.text);  // Deserialize JSON File
    }

    // Update is called once per frame
    void Update()
    {
        // Timer Section
        if (timer_on && (Time.time - line_start_time) >= line_duration)
        {
            hideSubtitles();

            timer_on = false;
        }
    }
}