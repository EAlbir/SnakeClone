using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameFlowManager : MonoBehaviour
{
    [Inject] InputReader inputReader;

    private void Awake()
    {
        inputReader.Initialize();
    }

    private void OnDestroy()
    {
        inputReader.Dispose();
    }
}
