using UnityEngine;
using System.Collections;

public class FirstPersonAudioWwise : MonoBehaviour
{

    public FirstPersonMovement character;
    public GroundCheck groundCheck;

    [Header("Step")]
    public AK.Wwise.Event stepAudio;
    private bool step_running = false;
    private bool sprint_running = false;
    private bool crouched_running = false;
    public AK.Wwise.Event runningAudio;
    public AK.Wwise.Event resetAudio;
    [Tooltip("Minimum velocity for the step audio to play")]
    public float velocityThreshold = .01f;
    Vector2 lastCharacterPosition;
    Vector2 CurrentCharacterPosition => new Vector2(character.transform.position.x, character.transform.position.z);

    [Header("Landing")]
    public AK.Wwise.Event landingAudio;

    [Header("Jump")]
    public Jump jump;
    public AK.Wwise.Event jumpAudio;

    [Header("Crouch")]
    public Crouch crouch;
    public AK.Wwise.Event crouchStartAudio, crouchedAudio, crouchEndAudio;

    void Reset()
    {
        // Setup stuff.
        character = GetComponentInParent<FirstPersonMovement>();
        groundCheck = transform.parent.GetComponentInChildren<GroundCheck>();

        // Jump audio.
        jump = GetComponentInParent<Jump>();
        if (jump)
        {

        }

        // Crouch audio.
        crouch = GetComponentInParent<Crouch>();
        if (crouch)
        {

        }
    }

    void Start()
    {
        lastCharacterPosition = CurrentCharacterPosition;
    }

    void OnEnable()
    {
        // Subscribe to events.
        groundCheck.Grounded += playLandingAudio;
        if (jump)
            jump.Jumped += playJumpAudio;
        if (crouch)
        {
            crouch.CrouchStart += playCrouchStartAudio;
            crouch.CrouchEnd += playCrouchEndAudio;
        }
    }

    void OnDisable()
    {
        // Unsubscribe to events.
        groundCheck.Grounded -= playLandingAudio;
        if (jump)
            jump.Jumped -= playJumpAudio;
        if (crouch)
        {
            crouch.CrouchStart -= playCrouchStartAudio;
            crouch.CrouchEnd -= playCrouchEndAudio;
        }
    }

    void FixedUpdate()
    {
        // Play moving audio if the character is moving and on the ground.
        float velocity = Vector3.Distance(CurrentCharacterPosition, lastCharacterPosition);
        if (velocity >= velocityThreshold && groundCheck.isGrounded)
        {
            if (crouch && crouch.IsCrouched)
            {
                step_running = false;
                updateMovingAudiosWwise(crouchedAudio);
            }
            else if (character.IsRunning)
            {
                step_running = false;
                updateMovingAudiosWwise(runningAudio);
            }
            else
                updateMovingAudiosWwise(stepAudio);
        }
        else
            updateMovingAudiosWwise(null);
        lastCharacterPosition = CurrentCharacterPosition;
    }

    // Update Moving Events on Wwise
    void updateMovingAudiosWwise(AK.Wwise.Event event_to_trigger)
    {
        // Process Event

        if (event_to_trigger == stepAudio && !step_running)
        {
            runningAudio.Stop(gameObject);
            crouchedAudio.Stop(gameObject);

            sprint_running = false;
            crouched_running = false;

            step_running = true;

            resetAudio.Post(gameObject);
            event_to_trigger.Post(gameObject);
        }
        else if (event_to_trigger == runningAudio && !sprint_running)
        {
            stepAudio.Stop(gameObject);
            crouchedAudio.Stop(gameObject);

            step_running = false;
            crouched_running = false;

            sprint_running = true;

            stepAudio.Post(gameObject);
            event_to_trigger.Post(gameObject);
        }
        else if (event_to_trigger == crouchedAudio && !crouched_running)
        {
            stepAudio.Stop(gameObject);
            runningAudio.Stop(gameObject);

            step_running = false;
            sprint_running = false;

            crouched_running = true;

            event_to_trigger.Post(gameObject);
        }
        // If Null, Stop All Events
        else if (event_to_trigger == null)
        {
            stepAudio.Stop(gameObject);
            runningAudio.Stop(gameObject);
            crouchedAudio.Stop(gameObject);

            step_running = false;
            sprint_running = false;
            crouched_running = false;
        }
    }

    void playLandingAudio() => playRandomClipWwise(landingAudio);
    void playJumpAudio() => playRandomClipWwise(jumpAudio);
    void playCrouchStartAudio() => playRandomClipWwise(crouchStartAudio);
    void playCrouchEndAudio() => playRandomClipWwise(crouchEndAudio);

    // Run Event From Wwise
    void playRandomClipWwise(AK.Wwise.Event tgt_event)
    {
        tgt_event.Post(gameObject);                                     // Call Play Event
    }
}