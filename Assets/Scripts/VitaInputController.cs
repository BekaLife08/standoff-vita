using UnityEngine;

// =============================================================================
// VitaInputController.cs — PS Vita controls (Unity 2017.4 PSP2 / .NET 3.5)
// Attach to the player object (or a persistent "VitaInput" GameObject).
//
// CAMERA (Look Around):
//   - Right stick: Input.GetAxis("RightStickX") / ("RightStickY").
//   - Touchscreen: ONLY touch.deltaPosition while touch.phase == Moved.
//   - NEVER uses Input.mousePosition, NEVER fires a shot from screen touch.
//     Touch only rotates the camera; UI menus (buy/pause) still use Unity UI
//     touch as usual — this script does not disable touchscreen input.
//
// BUTTON MAP (gameplay):
//   - Move (WASD):          left stick  (Input "Horizontal"/"Vertical")
//   - Fire / plant / defuse bomb: R Trigger ONLY (JoystickButton5)
//   - Secondary attack / Aim:     L Trigger ONLY (JoystickButton4)
//   - Jump:                 Cross   (JoystickButton0)
//   - Crouch:               Circle  (JoystickButton1, hold)
//   - Buy menu:             Triangle(JoystickButton3)
//   - Pause:                Start   (JoystickButton7)
//   - Switch weapons:       D-Pad Up (JoystickButton8) / Down (JoystickButton6)
//
// MOBILE HUD (Step 4):
//   - public GameObject mobileTouchHUD: assign the on-screen fire/walk/crouch
//     buttons root. Awake() forces mobileTouchHUD.SetActive(false) on Vita.
//   - Touchscreen stays enabled for UI menus; only the HUD buttons are hidden.
//
// Consume per frame via the public fields, e.g.:
//   moveInput (Vector2), lookDelta (Vector2, pixels/frame already scaled),
//   isFiring, isAiming, jumpPressed, crouchHeld, buyPressed, pausePressed,
//   switchWeaponDelta (-1 / 0 / +1).
// Compatible with: Unity 2017.4, C# 4 (.NET 3.5), IL2CPP PSP2.
// =============================================================================
public class VitaInputController : MonoBehaviour
{
    // Vita gamepad buttons (Unity PSP2 JoystickButton mapping).
    private static readonly KeyCode BTN_FIRE = KeyCode.JoystickButton5;   // R Trigger
    private static readonly KeyCode BTN_AIM = KeyCode.JoystickButton4;    // L Trigger
    private static readonly KeyCode BTN_JUMP = KeyCode.JoystickButton0;   // Cross
    private static readonly KeyCode BTN_CROUCH = KeyCode.JoystickButton1; // Circle
    private static readonly KeyCode BTN_BUY = KeyCode.JoystickButton3;    // Triangle
    private static readonly KeyCode BTN_PAUSE = KeyCode.JoystickButton7;  // Start
    private static readonly KeyCode BTN_WEAPON_UP = KeyCode.JoystickButton8;   // D-Pad Up
    private static readonly KeyCode BTN_WEAPON_DOWN = KeyCode.JoystickButton6; // D-Pad Down

    [Header("Mobile HUD root (fire / walk / crouch buttons)")]
    [Tooltip("On-screen touch buttons. Hidden on Vita in Awake(). Touchscreen itself stays active for UI menus.")]
    public GameObject mobileTouchHUD;

    [Header("Camera look")]
    [Tooltip("Input Manager axes bound to the Vita right stick.")]
    public string rightStickXAxis = "RightStickX";
    public string rightStickYAxis = "RightStickY";

    [Tooltip("Degrees per second at full right-stick deflection.")]
    public float stickLookSpeed = 180f;

    [Tooltip("Camera degrees per pixel of touch drag (touch.deltaPosition).")]
    public float touchLookSensitivity = 0.15f;

    [Tooltip("Invert touch vertical look.")]
    public bool invertTouchY = false;

    [Header("Movement")]
    [Tooltip("Input Manager axes bound to the Vita left stick.")]
    public string moveXAxis = "Horizontal";
    public string moveYAxis = "Vertical";

    // ---- Per-frame public state (read from player/camera code) ----
    [HideInInspector] public Vector2 moveInput = Vector2.zero;
    [HideInInspector] public Vector2 lookDelta = Vector2.zero;

    [HideInInspector] public bool isFiring;      // held: R Trigger
    [HideInInspector] public bool isAiming;      // held: L Trigger
    [HideInInspector] public bool crouchHeld;    // held: Circle
    [HideInInspector] public bool jumpPressed;   // edge: Cross
    [HideInInspector] public bool buyPressed;    // edge: Triangle
    [HideInInspector] public bool pausePressed;  // edge: Start
    [HideInInspector] public int switchWeaponDelta; // +1 Up, -1 Down, 0 none (edge)

    void Awake()
    {
        // STEP 4: hide mobile virtual buttons during gameplay.
        // Touchscreen input itself is NOT disabled — UI menus keep working.
        if (mobileTouchHUD != null)
        {
            mobileTouchHUD.SetActive(false);
        }
    }

    void Update()
    {
        // ---- Movement: left stick (WASD equivalent) ----
        float mx = Input.GetAxis(moveXAxis);
        float my = Input.GetAxis(moveYAxis);
        moveInput = new Vector2(mx, my);

        // ---- Camera: right stick ----
        float rx = 0f;
        float ry = 0f;
        try
        {
            rx = Input.GetAxis(rightStickXAxis);
            ry = Input.GetAxis(rightStickYAxis);
        }
        catch
        {
            rx = 0f;
            ry = 0f;
        }
        Vector2 stickDelta = new Vector2(rx * stickLookSpeed * Time.deltaTime,
                                         -ry * stickLookSpeed * Time.deltaTime);

        // ---- Camera: touchscreen drags ONLY via deltaPosition while Moved ----
        // No Input.mousePosition anywhere. Touch never triggers fire/aim.
        Vector2 touchDelta = Vector2.zero;
        int touches = Input.touchCount;
        for (int i = 0; i < touches; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 d = touch.deltaPosition * touchLookSensitivity;
                if (invertTouchY)
                {
                    d.y = -d.y;
                }
                touchDelta += d;
            }
        }

        lookDelta = stickDelta + touchDelta;

        // ---- Buttons: exact Vita map, no extras ----
        isFiring = Input.GetKey(BTN_FIRE);     // R Trigger ONLY (fire/plant/defuse)
        isAiming = Input.GetKey(BTN_AIM);      // L Trigger ONLY (aim/secondary)

        jumpPressed = Input.GetKeyDown(BTN_JUMP);   // Cross
        crouchHeld = Input.GetKey(BTN_CROUCH);      // Circle (hold)
        buyPressed = Input.GetKeyDown(BTN_BUY);     // Triangle
        pausePressed = Input.GetKeyDown(BTN_PAUSE); // Start

        switchWeaponDelta = 0;
        if (Input.GetKeyDown(BTN_WEAPON_UP))
        {
            switchWeaponDelta = 1;
        }
        else if (Input.GetKeyDown(BTN_WEAPON_DOWN))
        {
            switchWeaponDelta = -1;
        }
    }

    /// <summary>
    /// Optional bridge: copy this frame's state into the existing
    /// Axlebolt.Standoff.Player.PlayerInputs structure without adding a
    /// hard assembly dependency (reflection-free, duck-typed via dynamic? No —
    /// plain field copy by the caller is preferred). Example caller:
    ///   vitaInput.FillValues(ref inputs.Horizontal, ref inputs.Vertical, ...);
    /// Kept minimal on purpose for Vita GC pressure.
    /// </summary>
    public void GetMoveAim(out float horizontal, out float vertical,
                           out Vector2 look, out bool fire, out bool aim)
    {
        horizontal = moveInput.x;
        vertical = moveInput.y;
        look = lookDelta;
        fire = isFiring;
        aim = isAiming;
    }
}
