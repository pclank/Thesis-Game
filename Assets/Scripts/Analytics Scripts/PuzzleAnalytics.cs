using UnityEngine;
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
        // TODO: Write Code to Store Information From Analytics Item List to JSON File
    }
}