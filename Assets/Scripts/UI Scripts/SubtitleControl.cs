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
    public uint audio_id;                                               // ID of Audio Object
    public string audio_title;                                          // Title of Audio Object
    public Jline[] lines;                                               // List of Subtitle Line Objects

    public uint getAudioId()
    {
        return this.audio_id;
    }

    public Jline getLine(int index)
    {
        return this.lines[index];
    }

    public int getLineCount()
    {
        return this.lines.Length;
    }
}

// ************************************************************************************
// Subtitle Line JSON Object Class
// ************************************************************************************
[System.Serializable]
public class Jline
{
    public float timestamp;                                             // Timestamp for the Line to Appear
    public float duration;                                              // Duration of which Line will Appear
    public string line;                                                 // Subtitle Line
    public string style;                                                // Style of Line to be Parsed by Unity
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

    private float audio_start_time;                                     // Start Time of Audio
    private float line_duration;                                        // Duration of Current Line
    private float line_start_time;                                      // Start Time of Current Line

    private bool subtitles_on = false;                                  // Whether a Subtitle List is Running
    private bool timer_on = false;                                      // Whether Timer is On

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Select and Start Subtitles
    public bool startSubtitles(uint audio_id)
    {
        // Get Subtitles from List
        foreach (Jsubtitles j_sub in subtitle_list.subtitles_list)
        {
            // Compare Audio IDs
            if (j_sub.getAudioId() == audio_id)
            {
                selected_subtitles = j_sub;                 // Set Selected Subtitles

                line_index = 0;                             // Reset Line Index to Start

                audio_start_time = Time.time;

                subtitles_on = true;

                return true;
            }
        }

        return false;
    }

    // Set and Show Subtitle UI
    private void showSubtitles()
    {
        // Parse JSON Object
        if (selected_subtitles != null && line_index < selected_subtitles.getLineCount())
        {
            subtitle_ui.GetComponent<Text>().text = selected_subtitles.getLine(line_index).line;        // Set Line
            line_duration = selected_subtitles.getLine(line_index).duration;                            // Set Duration
        }

        line_start_time = Time.time;                                            // Set Start Time

        timer_on = true;                                                        // Enable Timer

        subtitle_ui.SetActive(true);                                            // Enable UI
    }

    // Clear and Hide Subtitle UI
    private void hideSubtitles()
    {
        timer_on = false;

        subtitle_ui.SetActive(false);                                           // Disable UI

        subtitle_ui.GetComponent<Text>().text = "";                             // Clear Text

        // Check Whether Line List is Over
        if (line_index + 1 == selected_subtitles.getLineCount())
            subtitles_on = false;
        else
            line_index++;
    }

    // Check Timestamp for New Subtitle
    private bool checkTimestamp()
    {
        if (Time.time - audio_start_time >= selected_subtitles.getLine(line_index).timestamp)
            return true;
        else
            return false;
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
        }
        // Timestamp Check
        else if (subtitles_on && !timer_on && checkTimestamp())
        {
            showSubtitles();
        }
    }
}