using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{

    [SerializeField] float speedOfSpin = 2.0f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 0.0f, (speedOfSpin * Time.deltaTime), Space.Self);        
    }
}
