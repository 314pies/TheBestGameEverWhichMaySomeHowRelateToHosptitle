using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;


public struct PlayersPack
{
    public int ID;
    public string Name;
    public Team team;
    public bool IsReady;

    public PlayersPack(int _id, Team _team, string _name, bool _IsReady)
    {
        ID = _id;
        team = _team;
        Name = _name;
        IsReady = _IsReady;
    }
}

[RequireComponent(typeof(PhotonView))]
public class ModeManager : Photon.PunBehaviour
{
    public PlayersPack localPlayer;
    Dictionary<int, PlayersPack> PlayerList = new Dictionary<int, PlayersPack>();

    public new PhotonView photonView;
    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        localPlayer.Name = "Player";//Should load from player pref
        localPlayer.ID = PhotonNetwork.player.ID;
        Debug.Log("My PID: " + localPlayer.ID);
        localPlayer.team = Team.Unknown;
    }

    // Use this for initialization
    void Start()
    {
        GoodPlayerSpawn();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectTeam(int TeamSelected)
    {
        localPlayer.team = (Team)TeamSelected;
        localPlayer = new PlayersPack(localPlayer.ID, localPlayer.team, localPlayer.Name, false);

        photonView.RPC("SyncTeam", PhotonTargets.AllBuffered, localPlayer.ID, (int)localPlayer.team, localPlayer.Name);
    }



    [PunRPC]
    public void SyncTeam(int ID, int _team, string _name)
    {
        Debug.Log("A___A: " + ID);
        if (!PlayerList.ContainsKey(ID))
        {
            PlayerList.Add(ID, new PlayersPack(ID, (Team)_team, _name, false));
        }
        else
        {
            PlayerList[ID] = new PlayersPack(ID, (Team)_team, _name, false);
        }
        UpdateList();
    }


    public void ChangeReadySatus()
    {
        if (localPlayer.team != Team.Unknown)
        {
            localPlayer.IsReady = !localPlayer.IsReady;
            photonView.RPC("SyncReadyStatus", PhotonTargets.AllBuffered, localPlayer.ID, localPlayer.IsReady);

        }
        else
        {
            Debug.Log("Much pick a team before ready");
        }
    }

    [PunRPC]
    public void SyncReadyStatus(int _ID, bool _IsReady)
    {
        if (PlayerList.ContainsKey(_ID))
        {
            PlayerList[_ID] = new PlayersPack(_ID, PlayerList[_ID].team, PlayerList[_ID].Name, _IsReady);
        }
        UpdateList();
    }

    public Text[] BadGuyNames;
    public Image[] BadGuyOKs;
    public Text[] GoodGuyNames;
    public Image[] GoodGuyOKs;

    private void UpdateList()
    {
        foreach (Text text in BadGuyNames) text.text = "";
        foreach (Text text in GoodGuyNames) text.text = "";

        foreach (Image image in BadGuyOKs) image.enabled = false;
        foreach (Image image in GoodGuyOKs) image.enabled = false;


        int LastBadIndex = 0, LastGoodIndex = 0;
        foreach (int _key in PlayerList.Keys)
        {
            Debug.Log(_key + ", ");
            if (PlayerList[_key].team == Team.BadSide)
            {
                if (LastBadIndex < BadGuyNames.Length)
                {
                    BadGuyNames[LastBadIndex].text = PlayerList[_key].Name;
                    BadGuyOKs[LastBadIndex].enabled = PlayerList[_key].IsReady;
                    LastBadIndex++;
                }
            }
            else if (PlayerList[_key].team == Team.GoodSide)
            {
                if (LastGoodIndex < GoodGuyNames.Length)
                {
                    GoodGuyNames[LastGoodIndex].text = PlayerList[_key].Name;
                    GoodGuyOKs[LastGoodIndex].enabled = PlayerList[_key].IsReady;
                    LastGoodIndex++;
                }
            }
        }
    }


    public void GoodPlayerSpawn()
    {
        PhotonNetwork.Instantiate("GoodPlayer", Vector3.zero, Quaternion.identity, 0);
    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer DisconnectedPlayer)
    {

        if (PlayerList.ContainsKey(DisconnectedPlayer.ID))
        {
            PlayerList.Remove(DisconnectedPlayer.ID);
            UpdateList();
        }
    }
}
