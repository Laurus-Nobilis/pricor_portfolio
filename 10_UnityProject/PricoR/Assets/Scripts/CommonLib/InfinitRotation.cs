using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, 1.5f);
        transform.RotateAround(transform.position, transform.right, 1.5f);
    }
}
