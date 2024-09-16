using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PodMovement playerMovement;
    public InteractionHandler PlayerInteractionHandler;

    private PlayerInputAction _playerInputAction;
    
    private InputAction _moveAction;
    
    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();

        _moveAction = _playerInputAction.Player.Move;
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _playerInputAction.Player.Break.performed += OnBreakPerformed;
        _playerInputAction.Player.Break.canceled += OnBreakCanceled;
        _playerInputAction.Player.Zoom1.performed += OnZoom1Performed;
        _playerInputAction.Player.Zoom2.performed += OnZoom2Performed;
        _playerInputAction.Player.Pause.performed += OnPausePerformed;
        _playerInputAction.Player.Place.performed += OnPlacePerformed;
        _playerInputAction.Player.Pause.Enable();
        _playerInputAction.Player.Break.Enable();
        _playerInputAction.Player.Zoom1.Enable();
        _playerInputAction.Player.Zoom2.Enable();
        _playerInputAction.Player.Place.Enable();

    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _playerInputAction.Player.Break.performed -= OnBreakPerformed;
        _playerInputAction.Player.Break.canceled -= OnBreakCanceled;
        _playerInputAction.Player.Zoom1.performed -= OnZoom1Performed;
        _playerInputAction.Player.Zoom2.performed -= OnZoom2Performed;
        _playerInputAction.Player.Pause.performed -= OnPausePerformed;
        _playerInputAction.Player.Place.performed -= OnPlacePerformed;
        _playerInputAction.Player.Pause.Disable();
        _playerInputAction.Player.Break.Disable();
        _playerInputAction.Player.Zoom1.Disable();
        _playerInputAction.Player.Zoom2.Disable();
        _playerInputAction.Player.Place.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 moveDir = _moveAction.ReadValue<Vector2>();
        playerMovement.Movement(moveDir);
    }

    private void OnZoom2Performed(InputAction.CallbackContext context)
    {
        GameManager.Instance.SetZoomLevelTwo(); 
    }

    private void OnZoom1Performed(InputAction.CallbackContext context)
    {

        GameManager.Instance.SetZoomLevelOne();
    }
    private void OnPausePerformed(InputAction.CallbackContext context) { 
        GameManager.Instance.ToggleGamePause();
    }

    private void OnBreakPerformed(InputAction.CallbackContext context)
    {
        playerMovement.BreakStart();
    }
    private void OnBreakCanceled(InputAction.CallbackContext context)
    {
        playerMovement.BreakEnd();
    }

    private void OnPlacePerformed(InputAction.CallbackContext context) {
        if (PlayerInteractionHandler.CarriedBox != null) {
            PlayerInteractionHandler.PlaceBox();
        } else {
            PlayerInteractionHandler.PickUpTile();
        }
    }
}
