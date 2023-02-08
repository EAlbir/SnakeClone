using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class SwipeDetection : MonoBehaviour
{
    [Inject] private InputReader inputReader;

    [Inject] private SwipeSettings swipeSettings;

    private Vector2 initialTouchPosition;
    private float initialTouchTime;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        inputReader.OnTouchStarted += (position, time) => AssignInitialSwipeVariables(position, time);
        inputReader.OnTouchFinished += (position, time) => DetectSwipe(position, time);
    }

    private void AssignInitialSwipeVariables(Vector2 position, float time)
    {
        initialTouchPosition = position;
        initialTouchTime = time;

        Debug.Log($"Swipe Started from: {initialTouchPosition} at: {initialTouchTime} ");
    }

    private void DetectSwipe(Vector2 position, float time)
    {
        Vector2 finalTouchPosition = position;
        float swipeSeconds = time - initialTouchTime;

        bool isSwipeLongEnough = Vector2.Distance(initialTouchPosition, finalTouchPosition) >= swipeSettings.minimumDistance;
        bool isSwipeDoneFastEnough = swipeSeconds <= swipeSettings.maximumTime;

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
        inputReader.OnTouchStarted -= (position, time) => AssignInitialSwipeVariables(position, time);
        inputReader.OnTouchFinished -= (position, time) => DetectSwipe(position, time);
    }

}
