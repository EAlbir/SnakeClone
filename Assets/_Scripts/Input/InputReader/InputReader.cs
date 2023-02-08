using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputReader
{
    #region Events

    public delegate void MovePerformed(Vector2 movement);
    public event MovePerformed OnMovePerformed;

    public delegate void TouchEvent(InputAction.CallbackContext context);
    public event TouchEvent OnTouchStarted;
    public event TouchEvent OnTouchFinished;

    #endregion

    private PlayerControls _playerControls;

    public Vector2 TouchPosition
    {
        get { return _playerControls.Game.TouchPosition.ReadValue<Vector2>(); }
    }

    #region Methods

    public void Initialize()
    {
        _playerControls = new PlayerControls();

        _playerControls.Enable();

        _playerControls.Game.Move.performed += context => PlayerMoved(context);

        _playerControls.Game.TouchContact.started += context => TouchStarted(context);
        _playerControls.Game.TouchContact.canceled += context => TouchFinished(context);

        Debug.Log("InputManager Initialized");
    }

    private void PlayerMoved(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();

        if (OnMovePerformed != null) OnMovePerformed(movement);
    }

    private void TouchStarted(InputAction.CallbackContext context)
    {
        if (OnTouchStarted != null) OnTouchStarted(context);
    }

    private void TouchFinished(InputAction.CallbackContext context)
    {
        if (OnTouchFinished != null) OnTouchFinished(context);
    }

    public void Dispose()
    {
        _playerControls.Disable();

        _playerControls.Game.Move.performed -= context => PlayerMoved(context);
        _playerControls.Game.TouchContact.started -= context => TouchStarted(context);
        _playerControls.Game.TouchContact.canceled -= context => TouchFinished(context);
    }

    #endregion
}
