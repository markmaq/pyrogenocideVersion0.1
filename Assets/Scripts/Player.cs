using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class player : MonoBehaviour
{
    [SerializeField] private float dragSensitivity = 1.5f;
    [SerializeField] private float _controllerSensitivity = 5f;
    public InputActionAsset inputActions;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction attackAction;

    private Vector2 moveAmount;
    private Vector2 lookAmount;

    private Animator playerAnim;
    private Rigidbody playerRB;

    public float playerSpeed;
    public float lookSpeed;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    public bool canCombo;





    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
        EnhancedTouchSupport.Enable();

    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
        EnhancedTouchSupport.Disable();
    }

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        attackAction = InputSystem.actions.FindAction("Attack");

        playerAnim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();

    }



    void Start()
    {

    }


    void Update()
    {
        moveAmount = moveAction.ReadValue<Vector2>();
        lookAmount = lookAction.ReadValue<Vector2>();

        PlayerRun();
        PlayerAttackControl();





        // Movement always from stick
        moveAmount = moveAction.ReadValue<Vector2>();

        // Start with look from stick (mouse/gamepad/on-screen right stick)
        Vector2 lookFromStick = lookAction.ReadValue<Vector2>();

        // Merge drag look
        Vector2 lookFromDrag = Vector2.zero;
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.screenPosition.x > Screen.width / 2) // only right side
            {
                lookFromDrag = touch.delta * dragSensitivity * Time.deltaTime;
                break; // first touch on right side is enough
            }

            // Final look = stick + drag
            lookAmount = lookFromStick + lookFromDrag;
        }

    }

    private void FixedUpdate()
    {
        Walking();
        LookRotationHorizontal();
        Debug.Log("Reading");
    }

    private void Walking()
    {
        playerRB.MovePosition(playerRB.position + transform.forward * moveAmount.y * playerSpeed * Time.deltaTime);
        playerRB.MovePosition(playerRB.position + transform.right * moveAmount.x * playerSpeed * Time.deltaTime);
        Debug.Log("Character is moving");
    }


    private void LookRotationHorizontal()
    {
        float rotationAmount = lookAmount.x * _controllerSensitivity * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
        playerRB.MoveRotation(playerRB.rotation * deltaRotation);
    }

    public void PlayerRun()
    {
        if (moveAmount.y > 0)
        {
            playerAnim.SetFloat("vertical", 1);
        }
        if (moveAmount.y == 0)
        {
            playerAnim.SetFloat("vertical", 0);
        }

    }

    private void PlayerAttackControl()
    {
        if (attackAction.WasPressedThisFrame())
        {
            playerAnim.SetTrigger("isattacking");
            Attack();


        }
    }

    private void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("hit");
            enemy.GetComponent<enemy>().TakeDamage(100);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}

