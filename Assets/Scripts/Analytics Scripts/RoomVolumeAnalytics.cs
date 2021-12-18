﻿using UnityEngine;
using System.Collections.Generic;

// ************************************************************************************
// Room - Volume Time - On Analytics System
// ************************************************************************************

// ************************************************************************************
// Analytics Room Class
// ************************************************************************************

public class AnalyticsRoom
{
    public string room_name;

    public float time_entered;
    public float time_exited;

    public AnalyticsRoom(string name, float t_entered)
    {
        this.room_name = name;

        this.time_entered = t_entered;
        this.time_exited = Time.time;
    }
}

public class RoomVolumeAnalytics : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Length of List that Triggers Data Export.")]
    public int max_length = 6;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private List<AnalyticsRoom> analytics_list = new List<AnalyticsRoom>();         // List of Analytics Events

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Add Analytics to List
    public void addAnalytics(string name, float t_entered)
    {
        analytics_list.Add(new AnalyticsRoom(name, t_entered));

        Debug.Log(name + " Added to Analytics, with Time Entered: " + t_entered);

        if (analytics_list.Count == max_length)
            recordAnalytics();
    }

    // Record Analytics List to JSON File
    private void recordAnalytics()
    {
        // TODO: Write Code to Store Information From Analytics Item List to JSON File
    }
}