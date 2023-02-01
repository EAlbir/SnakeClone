using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputManager
{
    #region Events

    public delegate void MovePerformed(Vector2 movement);
    public event MovePerformed OnMovePerformed;

    #endregion

    #region Variables

    private PlayerControls _playerControls;
    private readonly SwipeSettings _swipeSettings;

    private Vector2 initialTouchPosition;
    private float initialTouchTime;

    #endregion

    #region Properties

    public Vector2 TouchPosition
    {
        get { return _playerControls.Game.TouchPosition.ReadValue<Vector2>(); }
    }

    #endregion

    #region Constructor

    public InputManager(SwipeSettings settings)
    {
        _swipeSettings = settings;
        _playerControls = new PlayerControls();

        Initialize(); //TODO: Move to GameManager
    }

    #endregion

    #region Methods

    private void Initialize()
    {
        _playerControls.Enable();

        _playerControls.Game.Move.performed += context => PlayerMoved(context);

        _playerControls.Game.TouchContact.started += context => TouchStarted(context);
        _playerControls.Game.TouchContact.canceled += context => DetectSwipe(context);

        Debug.Log("InputManager Initialized");
    }

    private void PlayerMoved(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();

        Debug.Log($"Movement Detected: {movement}");

        if (OnMovePerformed != null) OnMovePerformed(movement);
    }

    private void TouchStarted(InputAction.CallbackContext context)
    {
        initialTouchPosition = TouchPosition;
        initialTouchTime = (float)context.startTime;

        Debug.Log($"Touch Started at position: {initialTouchPosition} at time: {initialTouchTime}");
    }

    private void DetectSwipe(InputAction.CallbackContext context)
    {
        Vector2 finalTouchPosition = TouchPosition;
        float swipeSeconds = (float)(context.time) - initialTouchTime;

        Debug.Log($"Touch Finished at position: {finalTouchPosition} after: {swipeSeconds}");

        bool SwipeHasEnoughDistance = Vector2.Distance(initialTouchPosition, finalTouchPosition) >= _swipeSettings.minimumDistance;
        bool SwipeDoneFastEnough = swipeSeconds <= _swipeSettings.maximumTime;

        if (SwipeHasEnoughDistance && SwipeDoneFastEnough)
        {
            Debug.Log($"Swipe Detected from: {initialTouchPosition} to: {finalTouchPosition} done in: {swipeSeconds} seconds");
        }
    }

    private void Dispose()
    {
        _playerControls.Disable();

        _playerControls.Game.Move.performed -= context => PlayerMoved(context);
        _playerControls.Game.TouchContact.started -= context => TouchStarted(context);
        _playerControls.Game.TouchContact.canceled -= context => DetectSwipe(context);
    }

    #endregion

    #region Utils

    [Serializable]
    public class SwipeSettings
    {
        public float minimumDistance = 0.2f;
        public float maximumTime = 0.33f;
    }

    #endregion
}
