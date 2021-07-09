using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class PythonScriptConnector : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public int n_frames = 60;                           // Number of Frames to Use
    public string file_name = "";                       // Executable Filename

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private string arguments = "";                     // Console Arguments

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Run Python Executable

    public string runExecutable(string cmd, string args, string filename)
    {
        ProcessStartInfo python_info = new ProcessStartInfo();
        Process python_p;

        // Set Executable

        python_info.FileName = filename;
        python_info.Arguments = string.Format("\"{0}\" \"{1}\"", cmd, args);
        python_info.CreateNoWindow = true;
        python_info.UseShellExecute = false;
        python_info.RedirectStandardOutput = true;
        python_info.RedirectStandardError = true;

        // Start Executable

        using (python_p = Process.Start(python_info))
        {
            // Read From Executable Output

            using (StreamReader reader = python_p.StandardOutput)
            {
                string stderr = python_p.StandardError.ReadToEnd();
                string result = reader.ReadToEnd();

                return result;
            }
        }
    }
}