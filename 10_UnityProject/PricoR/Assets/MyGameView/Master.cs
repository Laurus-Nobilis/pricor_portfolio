using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Master : MonoBehaviour
{
    [Inject] public ItemMaster Item { get; }


    public static Master Instance { 
        get;
        private set; 
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            return;
        }

        Destroy(this);
        Debug.LogError("Instance nothing");
    }
}
