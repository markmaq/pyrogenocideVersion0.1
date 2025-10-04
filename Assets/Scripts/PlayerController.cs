using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction p_moveAction;
    private InputAction p_attackAction;

    private Vector2 p_moveAmt;
    private Vector2 p_lookAmt;

    private Animator p_animator;
    private Rigidbody p_rigidbody;

    public float WalkSpeed;
    public float RotateSpeed;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int comboStep;
    bool canCombo = true;
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        p_moveAction = InputSystem.actions.FindAction("Move");
        p_attackAction = InputSystem.actions.FindAction("Attack");

        p_animator = GetComponent<Animator>();
        p_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        p_moveAmt = p_moveAction.ReadValue<Vector2>();


        if (p_attackAction.WasPressedThisFrame())
        {
            Attack();

            if (comboStep == 0)
            {
                p_animator.SetTrigger("Attack1");
                comboStep = 1;
            }
            else if (comboStep == 1 && canCombo)
            {
                p_animator.SetTrigger("Attack2");
                comboStep = 2;
            }
            else if (comboStep == 2 && canCombo)
            {
                p_animator.SetTrigger("Attack3");
                comboStep = 3;
            }

            else if (comboStep == 3 && canCombo)
            {
                p_animator.SetTrigger("Attack4");
                comboStep = 0; // reset after finishing combo
            }

        }
        



    }
    private void FixedUpdate()
    {
        Walking();

    }



    private void Walking()
    {
        p_rigidbody.MovePosition(p_rigidbody.position + transform.forward * p_moveAmt.y * WalkSpeed * Time.deltaTime);
        p_rigidbody.MovePosition(p_rigidbody.position + transform.right * p_moveAmt.x * WalkSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit");

        }
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
