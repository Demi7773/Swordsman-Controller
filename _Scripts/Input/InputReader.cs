using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerControls.IGameplayActions, PlayerControls.IMenuActions
{

    [SerializeField] private PlayerControls _input;

    public Vector2 MoveInput = Vector2.zero;
    public bool IsPressingAttack = false;
    public bool IsPressingBlock = false;
    public bool IsPressingSprint = false;
    public bool IsPressingJump = false;

    public PlayerController2.InputAction DesiredAction = PlayerController2.InputAction.None;


    [SerializeField] private float _saveInputFor = 0.2f;
    private float _clearInputCountdown = 0f;





    private void Awake()
    {
        _input = new PlayerControls();
        _input.Gameplay.SetCallbacks(this);
        _input.Menu.SetCallbacks(this);

        SetGameplay();
    }
    public void SetGameplay()
    {
        _input.Menu.Disable();
        _input.Gameplay.Enable();
        //Debug.Log("Input: Gameplay");
    }
    public void SetMenu()
    {
        _input.Gameplay.Disable();
        _input.Menu.Enable();
        //Debug.Log("Input: Menu");
    }




    void PlayerControls.IMenuActions.OnUnpause(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    void PlayerControls.IGameplayActions.OnPause(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }




    void PlayerControls.IGameplayActions.OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPressingAttack = true;
            SetDesired(PlayerController2.InputAction.Attack);
        }
        if (context.canceled)
            IsPressingAttack = false;
    }

    void PlayerControls.IGameplayActions.OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPressingBlock = true;
            SetDesired(PlayerController2.InputAction.Block);
        }
        if (context.canceled)
            IsPressingBlock = false;
    }
    void PlayerControls.IGameplayActions.OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPressingSprint = true;
            SetDesired(PlayerController2.InputAction.Sprint);
        }
        if (context.canceled)
            IsPressingSprint = false;
    }



    void PlayerControls.IGameplayActions.OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetDesired(PlayerController2.InputAction.Dash);
        }    
    }

    void PlayerControls.IGameplayActions.OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPressingJump = true;
            SetDesired(PlayerController2.InputAction.Jump);
        }
        if (context.canceled)
            IsPressingJump = false;
    }

    void PlayerControls.IGameplayActions.OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }


  

    private void SetDesired(PlayerController2.InputAction action)
    {
        DesiredAction = action;
        _clearInputCountdown = _saveInputFor;
    }






    private void Update()
    {
        if (DesiredAction == PlayerController2.InputAction.None)
            return;

        _clearInputCountdown -= Time.unscaledDeltaTime;
        if (_clearInputCountdown < 0f )
        {
            DesiredAction = PlayerController2.InputAction.None;
            _clearInputCountdown = _saveInputFor;
        }
    }

}
