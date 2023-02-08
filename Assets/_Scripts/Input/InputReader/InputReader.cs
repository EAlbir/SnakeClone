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

    public delegate void TouchEvent(Vector2 position, float time);
    public event TouchEvent OnTouchStarted;
    public event TouchEvent OnTouchFinished;

    #endregion

    private PlayerControls playerControls;

    private Vector2 TouchPosition
    {
        get
        {
            return playerControls.Game.TouchPosition.ReadValue<Vector2>();
        }
    }

    #region Methods

    public void Initialize()
    {
        playerControls = new PlayerControls();

        playerControls.Enable();

        SusbscribeToEvents();

        Debug.Log("InputManager Initialized");
    }

    private void SusbscribeToEvents()
    {
        playerControls.Game.Move.started += context => PlayerMoved(context);

        playerControls.Game.TouchPosition.started += context => FingerDown(context);
        playerControls.Game.TouchPosition.canceled += context => FingerUp(context);
    }

    private void PlayerMoved(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();

        if (OnMovePerformed != null) OnMovePerformed(movement);
    }

    private void FingerDown(InputAction.CallbackContext context)
    {
        if (OnTouchStarted != null) OnTouchStarted(TouchPosition, (float) context.startTime);
    }

    private void FingerUp(InputAction.CallbackContext context)
    {
        if (OnTouchFinished != null) OnTouchFinished(TouchPosition, (float) context.time);
    }

    public void Dispose()
    {
        playerControls.Disable();

        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents()
    {
        playerControls.Game.Move.performed -= context => PlayerMoved(context);

        playerControls.Game.TouchPosition.started -= context => FingerDown(context);
        playerControls.Game.TouchPosition.canceled -= context => FingerUp(context);
    }

    #endregion
}
