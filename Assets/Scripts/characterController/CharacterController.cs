using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{

    // input system
    private DefaultInputActions inputActions;

    private Rigidbody2D playerRB;

    // player parameters
    [SerializeField] private float movSpeed;
    [SerializeField] public float deceleration;  // Valore tra 0 e 1 per rallentare gradualmente
    private float speedX, speedY;

    private Vector2 inputMoveVector;
    private Vector2 inputLookVector;

    // Reference to the arm (Dummy object)
    [SerializeField] private Transform armDummy;


    // player movement state
    private bool decelerate = false;
    public bool isRotating { get; set; } = false;

    private void Awake()
    {
        inputActions = new DefaultInputActions();

        // Attivare l'Action Map Player
        inputActions.Player.Enable();

        // eventi "Move"
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMoveStop;

        // eventi
        inputActions.Player.Look.performed += OnLook;

        
    }
    

    private void OnMove(InputAction.CallbackContext context)
    {
        inputMoveVector = context.ReadValue<Vector2>();
        decelerate = false;
    }

    private void OnLook(InputAction.CallbackContext context)
    {

        if (isRotating)
        {
            inputLookVector = context.ReadValue<Vector2>();

            if (inputLookVector.sqrMagnitude > 0.01f)  // Controllo per evitare rotazione errata con input zero
            {
                float angle = Mathf.Atan2(inputLookVector.y, inputLookVector.x) * Mathf.Rad2Deg;
                armDummy.rotation = Quaternion.Euler(0, 0, angle);

                Debug.Log($"Braccio ruotato a: {angle} gradi");
            }
        }
        
    }

    private void OnMoveStop(InputAction.CallbackContext context)
    {
        Debug.Log("move stop");
        decelerate = true;
    }

    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();

    }

    public void InputPlayer(InputAction.CallbackContext _context)
    {
        inputMoveVector = _context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        speedX = inputMoveVector.x * movSpeed;
        speedY = inputMoveVector.y * movSpeed;

        // modifica velocity
        if(decelerate)
        {
            playerRB.linearVelocity = new Vector2(0, 0);
            
        } else
        {
            playerRB.linearVelocity = new Vector2(speedX, speedY);
        }
        
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
