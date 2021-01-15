using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    static int  count = 1;
    // Start is called before the first frame update
    void Start()
    {
        this.name = "PlayerLaser #" + count.ToString();
        count++;
    }   // Start()

    // Update is called once per frame
    void Update()
    {
        
    }   // Update()
}   // class PlayerLaser