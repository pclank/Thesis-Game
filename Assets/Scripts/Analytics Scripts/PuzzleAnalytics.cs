using UnityEngine;
using System.IO;
using System.Collections.Generic;

// ************************************************************************************
// Puzzle Analytics System
// ************************************************************************************

// ************************************************************************************
// Analytics Puzzle Class
// ************************************************************************************

public class AnalyticsPuzzle
{
    public string puzzle_name;

    public float solution_time;

    public AnalyticsPuzzle(string name)
    {
        this.puzzle_name = name;

        this.solution_time = Time.time;
    }
}

public class PuzzleAnalytics : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Length of List that Triggers Data Export.")]
    public int max_length = 5;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private List<AnalyticsPuzzle> analytics_list = new List<AnalyticsPuzzle>();     // List of Analytics Events

    // ************************************************************************************
    // Member Variables
    // ************************************************************************************

    // Add Analytics to List
    public void addAnalytics(string name)
    {
        analytics_list.Add(new AnalyticsPuzzle(name));

        Debug.Log(name + " Added to Analytics");

        if (analytics_list.Count == max_length)
            recordAnalytics();
    }

    // Record Analytics List to JSON File
    private void recordAnalytics()
    {
        string c_string = "{\"puzzle_analytics\": [" + JsonUtility.ToJson(analytics_list[0]) + ", ";

        for (int i = 1; i < analytics_list.Count; i++)
        {
            if (i == analytics_list.Count - 1)
                c_string += JsonUtility.ToJson(analytics_list[i]) + "]}";
            else
                c_string += JsonUtility.ToJson(analytics_list[i]) + ", ";
        }

        File.WriteAllText("puzzle_analytics.json", c_string);
    }
}