using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

// ************************************************************************************
// Analytics Emotion Class
// ************************************************************************************

public class AnalyticsEmotion
{
    public string emotion;

    public float detection_time;

    public AnalyticsEmotion(string emotion)
    {
        this.emotion = emotion;

        this.detection_time = Time.time;
    }
}

// ************************************************************************************
// JSON Object Emotion List Class
// ************************************************************************************

[System.Serializable]
public class EmotionList
{
    public Emotion[] Emotion;
}

// ************************************************************************************
// JSON Object Emotion Class
// ************************************************************************************

[System.Serializable]
public class Emotion
{
    public string emotion;
    public float certainty;
    public int face_detected;
}

public class JSONReader : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public GameObject face_warning_ui;                  // No Face Detected Warning UI GameObject

    [Tooltip("Flag to Allow Emotion Detection to Proceed.")]
    public bool start_trigger;

    public string json_location;                        // JSON File to Read
    public float lower_limit = 10.0f;                   // Lower Limit on Certainty

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private EmotionList deserialiazed_json;             // Deserialized JSON Objects

    private int previously_detected_emotion_index = -1; // Emotion Previously Detected

    private int[] emotion_occurrences = new int[4];     // Array to Store Occurrences of each Emotion, Initialized to O

    private float last_update_time = 0.0f;              // Time JSON File was Last Updated
    private float previously_detected_emotion_certainty = 0.0f;     // Certainty Previously Detected

    private List<AnalyticsEmotion> analytics_list = new List<AnalyticsEmotion>();     // List of Analytics Events

    private string release_path;                        // Release Version Path of JSON File
    private string previously_detected_emotion = "Unknown";     // Emotion Previously Detected

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Function to Read Emotion from File
    public Tuple<string, float> readEmotion()
    {
        // For Before Exiting Bathroom
        if (!start_trigger)
            return new Tuple<string, float>("Happy", 0.0f);

        //string text = System.IO.File.ReadAllText(@json_location);
        string text = System.IO.File.ReadAllText("json_results.json");

        deserialiazed_json =  JsonUtility.FromJson<EmotionList>(text);                  // Get Deserialize JSON Objects

        string detected_emotion = "Unknown";                                            // Initialize Returned Emotion
        float detected_certainty = 0.0f;                                                // Initialize Returned Certainty

        if (deserialiazed_json.Emotion[0].face_detected == 1 && deserialiazed_json.Emotion[0].certainty >= lower_limit)
        {
            addAnalytics(deserialiazed_json.Emotion[0].emotion);            // Save Detection to Analytics

            detected_certainty = deserialiazed_json.Emotion[0].certainty;   // Set Certainty
            detected_emotion = deserialiazed_json.Emotion[0].emotion;       // Set Emotion

            // Check Whether File was Updated
            if (Time.time - last_update_time >= 15.0f)
            {
                // Check Whether File hasn't been Updated
                if (String.Equals(detected_emotion, previously_detected_emotion) && detected_certainty == previously_detected_emotion_certainty)
                {
                    face_warning_ui.SetActive(true);
                }
                else
                {
                    face_warning_ui.SetActive(false);

                    last_update_time = Time.time;
                }
            }
            else
            {
                face_warning_ui.SetActive(false);
            }
        }
        else if (deserialiazed_json.Emotion[0].face_detected == 0)
            face_warning_ui.SetActive(true);

        Debug.Log("Detected " + detected_emotion);

        previously_detected_emotion = detected_emotion;
        previously_detected_emotion_certainty = detected_certainty;

        return new Tuple<string, float>(detected_emotion, detected_certainty);          // Return Information
    }

    // Function to Read Emotion Index from File
    public Tuple<int, float> readEmotionIndex()
    {
        // For Before Exiting Bathroom
        if (!start_trigger)
            return new Tuple<int, float>(0, 0.0f);

        //string text = System.IO.File.ReadAllText(@json_location);
        string text = System.IO.File.ReadAllText("json_results.json");

        deserialiazed_json = JsonUtility.FromJson<EmotionList>(text);                   // Get Deserialize JSON Objects

        int detected_index = previously_detected_emotion_index;                         // Initialize Returned Index
        float detected_certainty = 0.0f;                                                // Initialize Returned Certainty

        if (deserialiazed_json.Emotion[0].face_detected == 1 && deserialiazed_json.Emotion[0].certainty >= lower_limit)
        {
            detected_certainty = deserialiazed_json.Emotion[0].certainty;   // Set Certainty

            addAnalytics(deserialiazed_json.Emotion[0].emotion);            // Save Detection to Analytics

            if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Happy"))
                detected_index = 0;
            else if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Sad"))
                detected_index = 1;
            else if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Angry"))
                detected_index = 2;
            else if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Surprised"))
                detected_index = 3;

            // Check Whether File was Updated
            if (Time.time - last_update_time >= 15.0f)
            {
                // Check Whether File hasn't been Updated
                if (detected_index == previously_detected_emotion_index && detected_certainty == previously_detected_emotion_certainty)
                {
                    face_warning_ui.SetActive(true);
                }
                else
                {
                    face_warning_ui.SetActive(false);

                    last_update_time = Time.time;
                }
            }
            else
            {
                face_warning_ui.SetActive(false);
            }

            emotion_occurrences[detected_index]++;                                          // Increment Occurrence
        }
        else if (deserialiazed_json.Emotion[0].face_detected == 0)
            face_warning_ui.SetActive(true);

        Debug.Log("Detected " + detected_index);

        previously_detected_emotion_index = detected_index;
        previously_detected_emotion_certainty = detected_certainty;

        return new Tuple<int, float>(detected_index, detected_certainty);               // Return Information
    }

    // Get Highest Occurring Emotion
    public int getHighestOccurrence()
    {
        return emotion_occurrences.ToList().IndexOf(emotion_occurrences.Max());
    }

    // Add Analytics to List
    public void addAnalytics(string emotion)
    {
        analytics_list.Add(new AnalyticsEmotion(emotion));

        Debug.Log(emotion + " Added to Analytics");
    }

    // Record Analytics List to JSON File
    public void recordAnalytics()
    {
        string c_string = "{\"emotion_analytics\": [" + JsonUtility.ToJson(analytics_list[0]) + ", ";

        for (int i = 1; i < analytics_list.Count; i++)
        {
            if (i == analytics_list.Count - 1)
                c_string += JsonUtility.ToJson(analytics_list[i]) + "]}";
            else
                c_string += JsonUtility.ToJson(analytics_list[i]) + ", ";
        }

        File.WriteAllText("emotion_analytics.json", c_string);
    }

    void Start()
    {
        release_path = Path.Combine(Directory.GetCurrentDirectory(), "\\json_results.json");
    }
}