using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class SwipeDetection : MonoBehaviour
{
    [Inject] private InputReader inputReader;

    [Inject] private SwipeSettings _swipeSettings;

    private Vector2 initialTouchPosition;
    private float initialTouchTime;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        inputReader.OnTouchStarted += context => AssignInitialSwipeVariables(context);
        inputReader.OnTouchFinished += context => DetectSwipe(context);
    }

    private void AssignInitialSwipeVariables(InputAction.CallbackContext context)
    {
        initialTouchPosition = inputReader.TouchPosition;
        initialTouchTime = (float)context.startTime;
    }

    private void DetectSwipe(InputAction.CallbackContext context)
    {
        Vector2 finalTouchPosition = inputReader.TouchPosition;
        float swipeSeconds = (float)context.time - initialTouchTime;

        bool isSwipeLongEnough = Vector2.Distance(initialTouchPosition, finalTouchPosition) >= _swipeSettings.minimumDistance;
        bool isSwipeDoneFastEnough = swipeSeconds <= _swipeSettings.maximumTime;

        if (isSwipeLongEnough && isSwipeDoneFastEnough)
        {
            Debug.Log($"Swipe Detected from: {initialTouchPosition} to: {finalTouchPosition} done in: {swipeSeconds} seconds");
        }
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        inputReader.OnTouchStarted -= context => AssignInitialSwipeVariables(context);
        inputReader.OnTouchFinished -= context => DetectSwipe(context);
    }

}
