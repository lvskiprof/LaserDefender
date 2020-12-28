using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    /***
    *       OnTriggerEnter2D() will handle when a laser object hits the Shredder.
    ***/
    private void OnTriggerEnter2D(Collider2D collision)
	{
        Destroy(collision.gameObject);  // Disable this laser object
    }   // OnTriggerEnter2D()
}   // class Shredder
