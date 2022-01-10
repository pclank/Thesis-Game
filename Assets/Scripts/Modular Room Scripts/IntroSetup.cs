using UnityEngine;
using UnityEngine.UI;
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

    public GameObject terms_ui;

    [Tooltip("Intro Background UI GameObject.")]
    public GameObject intro_background;

    public GameObject crosshair_ui;

    public Button accept_button;
    public Button refuse_button;

    public VideoPlayer video_player;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool intro_over = false;

    // End of Clip Reached
    private void endReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;

        video_player.Stop();

        intro_over = true;

        Destroy(video_player.gameObject);

        displayAgreement();
    }

    // Display Agreement UI
    private void displayAgreement()
    {
        terms_ui.SetActive(true);

        Cursor.lockState = CursorLockMode.None;         // Unlock Cursor
        Cursor.visible = true;                          // Make Cursor Visible
    }

    // Close Agreement UI
    private void acceptAgreement()
    {
        terms_ui.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;       // Lock Cursor to Center
        Cursor.visible = false;                         // Hide Cursor

        intro_ui.SetActive(false);

        crosshair_ui.SetActive(true);

        Destroy(intro_ui);

        Time.timeScale = 1;                             // Unpause Game

        Destroy(this);
    }


    // Use this for initialization
    void Start()
    {
        Time.timeScale = 0;

        crosshair_ui.SetActive(false);

        video_player.loopPointReached += endReached;

        accept_button.onClick.AddListener(() => buttonCallback(accept_button));
        refuse_button.onClick.AddListener(() => buttonCallback(refuse_button));
    }

    void Update()
    {
        if (!intro_over && video_player.isPlaying && video_player.clockTime >= 1.0f)
            intro_background.SetActive(false);
    }

    // Button Callback
    private void buttonCallback(Button pressed_button)
    {
        if (pressed_button == accept_button)
            acceptAgreement();
        else if (pressed_button == refuse_button)
            Application.Quit();
    }
}