using UnityEngine;
using System.Collections;
using Photon;

public class MatchMaking_PUN : Photon.PunBehaviour
{
    private PhotonView PhotonView_Bridge;
    static public MatchMaking_PUN multiplayerController_Photon;

    public void ConnectToRegionalServer()
    {
        PhotonNetwork.ConnectUsingSettings("FirstImpression");
    }


    // Use this for initialization
    static int ExecutionCount = 0;
    //  int CurrentRegion = 0;
    void Awake()
    { 
        PhotonView_Bridge = GetComponent<PhotonView>();
    }

    void Start()
    {
     
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("We Have Joined The Lobby!");
    }


    public void StartRandomMatch()
    {
        StartAMatch(null);
    }

    string expectedRoomName = null;
    void StartAMatch(string RoomName)
    {
        if (PhotonNetwork.connectionState == ConnectionState.Connected)
        {
            if (RoomName == null)
            {
                PhotonNetwork.JoinRandomRoom();
                Debug.Log("Random Match");
            }
            else
            {
                PhotonNetwork.JoinRoom(RoomName);
            }
        }
        else//Log Some error message
        {
            Debug.Log("Not Connected to lobby yet.");
            if (PhotonNetwork.connectionState != ConnectionState.Connecting)
            {
                ConnectToRegionalServer();
            }
            StartCoroutine(WaitForLobbyConnection(RoomName));
        }
    }

    IEnumerator WaitForLobbyConnection(string RoomName)
    {
        int Count = 0;
        while (!PhotonNetwork.insideLobby)
        {
            yield return new WaitForSeconds(0.5f);
            Count++;
            if (Count > 20) break;
        }

        if (PhotonNetwork.insideLobby)
        {
            StartAMatch(RoomName);
        }
        else
        {
            Debug.LogError("Failed to connect to lobby.");
        }
    }


    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("Can't join A Room with name : " + expectedRoomName + "room!  Let's create One");
        CreateARoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!  Let's create One");

        CreateARoom();
    }

    void CreateARoom()
    {
         PhotonNetwork.CreateRoom(expectedRoomName);       
    }

    public override void OnCreatedRoom()
    {
        //MatchOptionPanel.MatchingStatus = MultiLan.AllStr[282]; //"Room created.";
        Debug.Log("We have created a room.");
        //   base.OnCreatedRoom();
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {        
        Debug.LogError("expectedGameMode is unsigned");
        // base.OnPhotonCreateRoomFailed(codeAndMsg);{
    }

    public override void OnJoinedRoom()
    {
        //Reset CustomProperties
        ExitGames.Client.Photon.Hashtable ResetProperties
               = new ExitGames.Client.Photon.Hashtable() { { "MS", null }, { "TB", null } };//MS = MyScore, TB = Team Belong
        PhotonNetwork.player.SetCustomProperties(ResetProperties);

        Debug.Log("Have join this room.......");
    }





    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {

        PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
        ExitGames.Client.Photon.Hashtable props = playerAndUpdatedProps[1] as ExitGames.Client.Photon.Hashtable;

        Debug.Log("Custom Properties is changed");
        //base.OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
       
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer DisconnectedPlayer)
    {

       
    }

    public override void OnDisconnectedFromPhoton()
    {
       
    }

    //Called when we left the room(not photon server)
    public override void OnLeftRoom()
    {
        
    }


    public override void OnPhotonMaxCccuReached()
    {
        Debug.LogError("Photon Cloud Max CCU Reached");
        //base.OnPhotonMaxCccuReached();
    }


    public void LeaveRoom()
    {
        Debug.Log("Leave room function invoked");
        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else//Is already not in room
        {
            Debug.Log("Already left room");
        }
    }


    //public void SendRPC(byte[] PackedData, int RecieverIGD)//Packed byte[], RecieverInGameID,-1 is for sending to everyone in room
    //{
    //    if (RecieverIGD == -1)
    //        PhotonView_Bridge.RPC("RecieveRPC", PhotonTargets.Others, PackedData);
    //    else
    //        PhotonView_Bridge.RPC("RecieveRPC", PhotonPlayer.Find(RecieverIGD), PackedData);
    //}


    //[PunRPC]
    //public void RecieveRPC(byte[] DataRiecieve)
    //{
    //    if (IngameConnecter != null)
    //        IngameConnecter.RecievedByteCode(DataRiecieve);
    //}


}
