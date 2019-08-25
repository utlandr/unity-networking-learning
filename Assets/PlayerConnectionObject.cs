using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerConnectionObject : NetworkBehaviour
{
    public GameObject PlayerUnitPrefab;

    // SyncVars are variables where if their value changes on the SERVER, all clients are automatically
    // given the new value.
    [SyncVar(hook="OnPlayerNameChanged")]
    public string PlayerName = "Lokii";

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

        if( Input.GetKeyDown(KeyCode.S) )
        {
            CmdSpawnMyUnit();
        }
        
        if( Input.GetKeyDown(KeyCode.Q) )
        {
            string n = "God" + Random.Range(1, 100);

            Debug.Log("Sending server request to change name to: " + n);
            CmdChangePlayerName(n);
        }
    }

    // WARNING: iF YOU USE A HOOK ON A SYNC VAR, THEN OUR LOCAL VALUE DOES NOT GET AUTOMATICALLY UPDATED.
    void OnPlayerNameChanged(string newName)
    {
        Debug.Log("OnPlayerNameChanged: OldName: " + PlayerName + " NewName: " + newName);
        PlayerName = newName;
        gameObject.name = "PlayerConnectionObject ["+newName+"]";
    }

    ////////////////////////////////// COMMANDS
    // Special functions that only get executed on the server


    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject go = Instantiate(PlayerUnitPrefab);
        
        // One way of doing it
        //go.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        // Now object exists on server, propagate to all the clients
        // and wire up to the Network Identity
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
         
    }

    [Command]
    void CmdChangePlayerName(string n)
    {
        Debug.Log("CmdChangePlayerName: " + n);
        PlayerName = n;

        // Check that the name doesnt have  any blacklisted words in it?
        // If there is. what happens? Ignore? Or still call Rpc but with 
        // the original name?


        // Tell all the clients waht this player's name now is
        //RpcChangePlayerName(n);
        
    }

    ///////////////////////////////// RPCS
    // RPCs are special functions that ONLY get executed on the clients

//     [ClientRpc]
//     void RpcChangePlayerName(string n)
//     {
//         Debug.Log("RpcChangePlayerName: Asked to change athe player name on a particular PlayerConnectionObject: " + n);
//         PlayerName = n;
//     }
}
