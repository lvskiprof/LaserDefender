using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    float   degreesOfSpinPerSecond = 360f;

    // Update is called once per frame and will make the bomb spin that has this script as a component.
    void Update()
    {
        gameObject.transform.Rotate(0, 0, degreesOfSpinPerSecond* Time.deltaTime);
    }   // Update()
}   // class Spinner