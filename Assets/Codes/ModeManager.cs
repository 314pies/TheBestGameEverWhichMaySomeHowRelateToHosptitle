using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public InGameStatus Status = InGameStatus.PickingTeam;
    public PlayersPack localPlayer;
    Dictionary<int, PlayersPack> PlayerList = new Dictionary<int, PlayersPack>();

    public new PhotonView photonView;
    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        localPlayer.Name = PlayerPrefs.GetString("Name","Player");//Should load from player pref
        localPlayer.ID = PhotonNetwork.player.ID;
        Debug.Log("My PID: " + localPlayer.ID);
        localPlayer.team = Team.Unknown;
    }

    // Use this for initialization
    void Start()
    {
        BadTimer.maxValue = RoundTime;
        GoodTimer.maxValue = RoundTime;
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
    [Header("Picking Team")]
    public GameObject PickingTeamSetup;
    public Text[] BadGuyNames;
    public Image[] BadGuyOKs;
    public Text[] GoodGuyNames;
    public Image[] GoodGuyOKs;
    [Header("Counting Down")]
    public GameObject CountingDownWindow;
    public Text CoutingDownText;
    private void UpdateList()
    {
        foreach (Text text in BadGuyNames) text.text = "";
        foreach (Text text in GoodGuyNames) text.text = "";

        foreach (Image image in BadGuyOKs) image.enabled = false;
        foreach (Image image in GoodGuyOKs) image.enabled = false;


        int LastBadIndex = 0, LastGoodIndex = 0;
        bool CountDownEnabled = true;
        foreach (int _key in PlayerList.Keys)
        {
            if (PlayerList[_key].team == Team.Unknown)
            {
                CountDownEnabled = false;
            }
            if (PlayerList[_key].IsReady == false)
            {
                CountDownEnabled = false;
            }
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

        if (CountDownEnabled && LastGoodIndex > 0 && LastBadIndex > 0)
        {
            StartCoroutine(WaitAndCountingDown());
            Debug.Log("Start counting down");
        }
        else
        {
            Status = InGameStatus.PickingTeam;
        }
    }


    IEnumerator WaitAndCountingDown()
    {
        Status = InGameStatus.RoundStartCountingDown;
        CountingDownWindow.SetActive(true);
        float TimeLeft = 10, TimeGap = 1.0f;
        while (true)
        {
            CoutingDownText.text = "Game will start in..." + TimeLeft;
            yield return new WaitForSeconds(TimeGap);
            TimeLeft -= TimeGap;
            if (Status != InGameStatus.RoundStartCountingDown)
                break;
            if (TimeLeft <= 0) break;
        }
        if (Status == InGameStatus.RoundStartCountingDown)
        {
            Debug.Log("Pass Counting");
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.room.IsOpen = false;//close the room
                PhotonNetwork.room.IsVisible = false;//close the room
                photonView.RPC("StartBadGuyDeployment", PhotonTargets.All);
            }
        }
        else
        {
            Debug.Log("Counting failed");
        }
        CountingDownWindow.SetActive(false);
    }

    [Header("Bad guy Deploying")]
    public float BadGuyDeployTime = 35.0f;
    public GameObject BadguySetup;
    public GameObject GoodguySetup;
    public GameObject GoodGuyCover;
    public Text GoodguyText;
    //public Text BadGuyTimeText;

    [PunRPC]
    public void StartBadGuyDeployment()
    {
        PickingTeamSetup.SetActive(false);
        Status = InGameStatus.BadguyDeploying;
        if (localPlayer.team == Team.BadSide)
        {
            BadguySetup.SetActive(true);
        }
        else if (localPlayer.team == Team.GoodSide)
        {
            GoodguySetup.SetActive(true);
        }

        StartCoroutine(BadguyDeployCountdown());
    }

    IEnumerator BadguyDeployCountdown()
    {
        float TimeLeft = BadGuyDeployTime, TimeGap = 1.0f;
        while (true)
        {
            yield return new WaitForSeconds(TimeGap);
            TimeLeft -= TimeGap;
            if (localPlayer.team == Team.BadSide)
            {
               // BadGuyTimeText.text = "" + TimeLeft;
            }
            else if (localPlayer.team == Team.GoodSide)
            {
                GoodguyText.text = "Starting in..." + TimeLeft;
            }
            if (TimeLeft <= 0) break;
        }
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("StarSaving", PhotonTargets.All);
        }
    }
    [Header("Gaming")]
    public Transform PlayerSpawnPos;
    public Patient patient;
    public float RoundTime = 120.0f;
    public float RoundTimeLeft;
    public Slider BadTimer;
    public Slider GoodTimer;

    [PunRPC]
    public void StarSaving()
    {
        if (localPlayer.team == Team.GoodSide)
        {
            GoodGuyCover.SetActive(false);
            Vector3 Pos = PlayerSpawnPos.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            GoodPlayerSpawn(Pos);
        }
        else if (localPlayer.team == Team.BadSide)
        {

        }

        patient.StartCreateWave();
        StartCoroutine(TimeCountDown());
    }

    public void GoodPlayerSpawn(Vector3 spawnPos)
    {
        PhotonNetwork.Instantiate("GoodPlayer", spawnPos, Quaternion.identity, 0);
    }


    IEnumerator TimeCountDown()
    {
        float _timeLeft = RoundTime, TimeGap = 1.0f;
        while (true)
        {
            yield return new WaitForSeconds(TimeGap);
            _timeLeft -= TimeGap;

            if (PhotonNetwork.isMasterClient)
                photonView.RPC("SynTime", PhotonTargets.All, _timeLeft);

            if (_timeLeft <= 0) break;
        }

        if (patient.TargetHealthAmount > patient.HealthAmount)
        {
            OnSomeoneWin(Team.BadSide);
        }
        //Round end
    }

    [PunRPC]
    public void SynTime(float _timeLeft)
    {
        RoundTimeLeft = _timeLeft;
        //Update uI

        GoodTimer.value = _timeLeft;
        BadTimer.value = _timeLeft;
    }

    [Header("Result")]
    public GameObject GoodguyWinWindow;
    public GameObject BadGuyWinWindow;

    public void OnSomeoneWin(Team winningTeam)
    {
        if (PhotonNetwork.isMasterClient)
            photonView.RPC("ReslutUpdate", PhotonTargets.All, (int)winningTeam);
    }

    [PunRPC]
    public void ReslutUpdate(int winningTeam)
    {
        Team result = (Team)winningTeam;
        if(result == Team.BadSide)
        {
            BadGuyWinWindow.SetActive(true);
        }
        else if(result == Team.GoodSide)
        {
            GoodguyWinWindow.SetActive(true);
        }
        StartCoroutine(WaitAndDisConnect());
    }

    public IEnumerator WaitAndDisConnect()
    {
        yield return new WaitForSeconds(10.0f);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer DisconnectedPlayer)
    {

        if (PlayerList.ContainsKey(DisconnectedPlayer.ID))
        {
            PlayerList.Remove(DisconnectedPlayer.ID);
            UpdateList();
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if (!PlayerList.ContainsKey(newPlayer.ID))
        {
            PlayerList.Add(newPlayer.ID, new PlayersPack(newPlayer.ID, Team.Unknown, null, false));
        }
        UpdateList();
        base.OnPhotonPlayerConnected(newPlayer);
    }
}
