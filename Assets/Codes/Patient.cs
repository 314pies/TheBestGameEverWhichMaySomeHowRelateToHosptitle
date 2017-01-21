using UnityEngine;
using UnityEngine.UI;
using System.Collections;


[RequireComponent(typeof(PhotonView))]
public class Patient : MonoBehaviour
{

    public ModeManager modeManager;
    public int HealthAmount = 0;
    public int TargetHealthAmount = 15;


    public PhotonView photonView;

    public Slider ProgressBar_Bad;
    public Slider ProgressBar_Good;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Use this for initialization
    void Start()
    {
        ProgressBar_Good.maxValue = TargetHealthAmount;
        ProgressBar_Bad.maxValue = TargetHealthAmount;
    }

    public void StartCreateWave()
    {
        InvokeRepeating("CreateWave", 2.0f, 1.0f);
    }

    public void RecieveWave()
    {
        HealthAmount++;
        photonView.RPC("UpdateHealthAmount", PhotonTargets.AllBuffered, HealthAmount);

    }

    //public Text textUI;
    [PunRPC]
    public void UpdateHealthAmount(int NewHealth)
    {
        Debug.Log("Update Health: " + NewHealth);
        HealthAmount = NewHealth;
        if (HealthAmount > TargetHealthAmount)
        {
           // if (PhotonNetwork.isMasterClient)
                modeManager.OnSomeoneWin(Team.GoodSide);

            Debug.Log("Good guy win");
        }
        else
        {
            ProgressBar_Bad.value = HealthAmount;
            ProgressBar_Good.value = HealthAmount;
            //textUI.text = HealthAmount + "/" + TargetHealthAmount;
        }
    }

    public float WaveSpeed = 10.0f;

    public void CreateWave()
    {
        if (PhotonNetwork.isMasterClient)
        {
            GameObject NewWave = PhotonNetwork.Instantiate("Wave", transform.position, Quaternion.identity, 0);
            Vector3 Speed = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Speed.Normalize();
            NewWave.GetComponent<Rigidbody>().velocity = Speed * WaveSpeed;
        }
    }


}
