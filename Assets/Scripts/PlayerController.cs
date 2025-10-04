using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction p_moveAction;
    private InputAction p_attackAction;

    private Animator p_animator;
    private Rigidbody p_rigidbody;

    public float WalkSpeed;
    public float RotateSpeed;


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
    }

}
