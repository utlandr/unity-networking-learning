using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Unit controlled by player
public class PlayerUnit : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // This runs on ALL player units not just the ones I own

        // How do we check whether we can play with an object?
        if( hasAuthority == false )
        {
            return;
        }


        if( Input.GetKeyDown(KeyCode.Space) )
        {
            this.transform.Translate(0, 1, 0);

            
        }
    }
}
