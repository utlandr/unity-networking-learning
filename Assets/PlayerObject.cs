using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerObject : NetworkBehaviour
{
    public GameObject PlayerUnitPrefab;

    // This thing tracks the player
    void Start()
    {
        // WHO AM I
        if( isLocalPlayer == false)
        {
            // NOT ME
            return;
        }

        // WITNESS ME
        Debug.Log("PlayerObject::Start -- Spawning my own personal unit.");
        CmdSpawnMyUnit();   
        // Instantiate only creates an object on the local computer
        // Even if it has a NetworkIdentity, it will still not be on the network
        // or on any other client for that matter
        // NetworkServer.Spawn() is the only thing can call it into being.

       

        // Command the server to spawn our unit


    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer == false)
        {
            return;
        }
        //  Update runs on EVERYONE's computer whether or not they own this player object 
        if( Input.GetKeyDown(KeyCode.Space) )
        {
            //this.transform.Translate(0, 1, 0);

            // Spacebar hit. Instruct server about this  so it can do something.
            CmdMoveUnitUp();
        }
        
    }

    ////////////////////////////////// COMMANDS
    // Special functions that only get executed on the server

    GameObject myPlayerUnit;

    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject go = Instantiate(PlayerUnitPrefab);
        myPlayerUnit = go;
        
        //go.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        

        // Now object exists on server, propagate to all the clients
        // and wire up to the Network Identity
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
         
    }

    [Command]
    void CmdMoveUnitUp()
    {
        if(myPlayerUnit == null)
        {
            return;
        }

        myPlayerUnit.transform.Translate(0, 1, 0);
    }


}
