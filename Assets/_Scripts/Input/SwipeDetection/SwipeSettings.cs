using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Swipe Settings", menuName = "Snake/Swipe Settings")]
public class SwipeSettings : ScriptableObject
{
    public float minimumDistance = 0.2f;
    public float maximumTime = 0.33f;
}
