using UnityEngine;
using UnityEngine.Video;
using System.Collections;

// ************************************************************************************
// Intro Setup Controller
// ************************************************************************************

public class IntroSetup : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Intro UI GameObject.")]
    public GameObject intro_ui;

    [Tooltip("Intro Background UI GameObject.")]
    public GameObject intro_background;

    public GameObject crosshair_ui;

    public VideoPlayer video_player;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    // End of Clip Reached
    private void endReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;

        Time.timeScale = 1;

        video_player.Stop();

        Destroy(video_player.gameObject);

        intro_ui.SetActive(false);

        crosshair_ui.SetActive(true);

        Destroy(intro_ui);

        Destroy(this);
    }


    // Use this for initialization
    void Start()
    {
        Time.timeScale = 0;

        crosshair_ui.SetActive(false);

        video_player.loopPointReached += endReached;
    }

    void Update()
    {
        if (video_player.isPlaying && video_player.clockTime >= 1.0f)
            intro_ui.SetActive(false);
    }    
}