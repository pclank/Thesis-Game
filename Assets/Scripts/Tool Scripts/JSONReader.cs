using UnityEngine;
using System;
using System.Linq;
using System.Collections;

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

    public string json_location;                        // JSON File to Read
    public float lower_limit = 10.0f;                   // Lower Limit on Certainty

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private EmotionList deserialiazed_json;             // Deserialized JSON Objects

    private int[] emotion_occurrences = new int[4];     // Array to Store Occurrences of each Emotion, Initialized to O

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Function to Read Emotion from File
    public Tuple<string, float> readEmotion()
    {
        string text = System.IO.File.ReadAllText(@json_location);

        deserialiazed_json =  JsonUtility.FromJson<EmotionList>(text);                  // Get Deserialize JSON Objects

        string detected_emotion = "Unknown";                                            // Initialize Returned Emotion
        float detected_certainty = 0.0f;                                                // Initialize Returned Certainty

        if (deserialiazed_json.Emotion[0].face_detected == 1 && deserialiazed_json.Emotion[0].certainty >= lower_limit)
        {
            detected_certainty = deserialiazed_json.Emotion[0].certainty;   // Set Certainty
            detected_emotion = deserialiazed_json.Emotion[0].emotion;       // Set Emotion

            face_warning_ui.SetActive(false);
        }
        else if (deserialiazed_json.Emotion[0].face_detected == 0)
            face_warning_ui.SetActive(true);

        return new Tuple<string, float>(detected_emotion, detected_certainty);          // Return Information
    }

    // Function to Read Emotion Index from File
    public Tuple<int, float> readEmotionIndex()
    {
        string text = System.IO.File.ReadAllText(@json_location);

        deserialiazed_json = JsonUtility.FromJson<EmotionList>(text);                   // Get Deserialize JSON Objects

        int detected_index = 0;                                                         // Initialize Returned Index
        float detected_certainty = 0.0f;                                                // Initialize Returned Certainty

        if (deserialiazed_json.Emotion[0].certainty >= lower_limit)
        {
            detected_certainty = deserialiazed_json.Emotion[0].certainty;   // Set Certainty

            if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Happy"))
                detected_index = 0;
            else if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Sad"))
                detected_index = 1;
            else if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Angry"))
                detected_index = 2;
            else if (String.Equals(deserialiazed_json.Emotion[0].emotion, "Surprised"))
                detected_index = 3;

            face_warning_ui.SetActive(false);

            emotion_occurrences[detected_index]++;                                          // Increment Occurrence
        }
        else if (deserialiazed_json.Emotion[0].face_detected == 0)
            face_warning_ui.SetActive(true);

        return new Tuple<int, float>(detected_index, detected_certainty);               // Return Information
    }

    // Get Highest Occurring Emotion
    public int getHighestOccurrence()
    {
        return emotion_occurrences.ToList().IndexOf(emotion_occurrences.Max());
    }
}