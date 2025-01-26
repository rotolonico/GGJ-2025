using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    // input system
    private UIPlayerInput inputActions;
    private Rigidbody2D playerRB;

    // player parameters
    [SerializeField] private float movSpeed;
    [SerializeField] private float deceleration;
    [SerializeField] private bool lockYAxis = false;

    private Vector2 speed;

    private Vector2 inputMoveVector;
    private Vector2 inputLookVector;

    // Reference to the arm (Dummy object)
    [SerializeField] private Transform armDummy;

    [SerializeField] private MonoBehaviour armedScript;
    [SerializeField] private MonoBehaviour storedScript;
    private IBobble bobbleArmed;
    private IBobble bobbleStored;

    // player movement state
    private bool isMoving = false;

    private void Awake()
    {
        inputActions = new UIPlayerInput();

        // Attivare l'Action Map Player
        inputActions.PlayerInput.Enable();

        // eventi "Move"
        inputActions.PlayerInput.Move.performed += OnMove;
        inputActions.PlayerInput.Move.canceled += OnMoveStop;

        // eventi
        inputActions.PlayerInput.Look.performed += OnLook;
        inputActions.PlayerInput.Shoot.performed += _ => bobbleArmed.ApplyEffect(true);
        inputActions.PlayerInput.Shoot.canceled += _ => bobbleArmed.ApplyEffect(false);
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        inputMoveVector = context.ReadValue<Vector2>();
        isMoving = false;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        inputLookVector = context.ReadValue<Vector2>();

        if (inputLookVector.sqrMagnitude > 0.01f)  // Controllo per evitare rotazione errata con input zero
        {
            float angle = Mathf.Atan2(inputLookVector.y, inputLookVector.x) * Mathf.Rad2Deg + 90;
            armDummy.rotation = Quaternion.Euler(0, 0, angle);

            //Debug.Log($"Braccio ruotato a: {angle} gradi");
        }

    }

    private void OnMoveStop(InputAction.CallbackContext context)
    {
        Debug.Log("move stop");
        isMoving = true;
    }

    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();


        if (armedScript != null)
            bobbleArmed = armedScript as IBobble;

    }

    public void InputPlayer(InputAction.CallbackContext _context)
    {
        inputMoveVector = _context.ReadValue<Vector2>();
        isMoving = false;
    }

    void FixedUpdate()
    {
        speed = lockYAxis ? new Vector2(inputMoveVector.x, 0) * movSpeed : inputMoveVector * movSpeed;
        playerRB.linearVelocity = isMoving ? Vector2.zero : speed;
    }

    public void ChangeBobble()
    {

    }

    public void ShootBobble(bool isShooting)
    {
        bobbleArmed.ApplyEffect(isShooting);
    }

    private void OnValidate()
    {
        if (armedScript != null && armedScript is not IBobble)
            armedScript = null;

    }

    // graphic debugging
    void OnDrawGizmos()
    {
        if (inputLookVector.sqrMagnitude > 0.01f)
        {
            // Colore verde per il raggio di debug
            Gizmos.color = Color.green;

            // Direzione dello sguardo
            Vector3 lookDirection = new Vector3(inputLookVector.x, inputLookVector.y, 0).normalized;

            // Disegna una linea dal braccio Dummy nella direzione di sguardo
            Gizmos.DrawLine(armDummy.position, armDummy.position + lookDirection * 2f);

            // Disegna una sfera alla fine della linea per indicare la direzione
            Gizmos.DrawSphere(armDummy.position + lookDirection * 2f, 0.1f);
        }
    }
}
