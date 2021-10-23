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

        player_object.transform.position = start_position;

        EditorUtility.DisplayDialog("Player Position Tool", "Player Set at Start!", "OK", "");      // Display Result
    }
}