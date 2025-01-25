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


    // player movement state
    private bool decelerate = false;
    

    private void Awake()
    {
        inputActions = new DefaultInputActions();

        // Attivare l'Action Map Player
        inputActions.Player.Enable();

        // Registrare l'evento della Action "Move"
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMoveStop;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        inputMoveVector = context.ReadValue<Vector2>();
        decelerate = false;
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
}
