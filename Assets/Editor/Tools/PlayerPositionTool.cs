using UnityEngine;
using UnityEditor;

// ************************************************************************************
// Tool to Change Player Initial Position in WorldSpace
// ************************************************************************************

public class PlayerPositionTool : ScriptableObject
{
    // Add First Menu Option

    [MenuItem("Tools/Player Position Tool/Set Player at Start")]
    static void setAtStart()
    {
        Vector3 start_position = new Vector3(3.79f, 0.0f, -3.11f);                                  // Game Start Position

        GameObject player_object = GameObject.FindWithTag("Player");                                // Get Player GameObject

        player_object.transform.position = start_position;                                          // Transform Player

        EditorUtility.DisplayDialog("Player Position Tool", "Player Set at Start!", "OK", "");      // Display Result
    }

    // Add Second Menu Option

    [MenuItem("Tools/Player Position Tool/Set Player at Coin Puzzle")]
    static void setAtCoinPuzzle()
    {
        Vector3 target_position = new Vector3(-61.38f, 9.302f, 14.675f);                            // Game Target Position

        GameObject player_object = GameObject.FindWithTag("Player");                                // Get Player GameObject

        player_object.transform.position = target_position;                                         // Transform Player

        EditorUtility.DisplayDialog("Player Position Tool", "Player Set at Coin Puzzle!", "OK", "");    // Display Result
    }    
}