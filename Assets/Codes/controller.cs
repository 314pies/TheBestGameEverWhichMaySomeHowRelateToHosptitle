using UnityEngine;
using System.Collections;

public class controller : Photon.MonoBehaviour
{
    public PlayerStatus status = PlayerStatus.Normal;

    public float NetworkSmoothing = 3.6f;
    public Rigidbody CharacterController;
    public int LeftVelocity = -10, DownVelocity = -10, UpVelocity = 10, RightVelocity = 10;
    public int JumpVelocity = 5;
    public Transform DrBird;

    public void Awake()
    {
        BubbleEffect.SetActive(false);
    }


    void Start()
    {
        CharacterController = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            if (status == PlayerStatus.Normal)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    //Debug.Log("aksdjhajhfdsg");
                    CharacterController.velocity = new Vector3(
                        CharacterController.velocity.x,
                        CharacterController.velocity.y,
                        DownVelocity
                        );
                }
                if (Input.GetKey(KeyCode.A))
                {
                    //Debug.Log("aksdjhajhfdsg");
                    CharacterController.velocity = new Vector3(
                        LeftVelocity,
                        CharacterController.velocity.y,
                        CharacterController.velocity.z
                        );
                    photonView.RPC("DrBirdRotation", PhotonTargets.All,true);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    //Debug.Log("aksdjhajhfdsg");
                    CharacterController.velocity = new Vector3(
                        RightVelocity,
                        CharacterController.velocity.y,
                        CharacterController.velocity.z
                        );
                    photonView.RPC("DrBirdRotation", PhotonTargets.All, false);
                }
                if (Input.GetKey(KeyCode.W))
                {
                    //Debug.Log("aksdjhajhfdsg");
                    CharacterController.velocity = new Vector3(
                        CharacterController.velocity.x,
                        CharacterController.velocity.y,
                        UpVelocity
                        );
                }
                /*if (Input.GetKeyDown("space"))
                {
                    //Debug.Log("aksdjhajhfdsg");
                    CharacterController.velocity = new Vector3(
                        CharacterController.velocity.x,
                        JumpVelocity,
                        CharacterController.velocity.z
                        );
                }*/
            }
        }
        else
        {

            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * NetworkSmoothing);
            //  transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }


    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

    public void Freeze(float TimeLegnth)
    {
        photonView.RPC("FreezeAllRemote", PhotonTargets.All, TimeLegnth);
    }


    [PunRPC]
    public void FreezeAllRemote(float FreezeTime)
    {
        status = PlayerStatus.Freeze;
        StartCoroutine(WaitAndUnFreeze(FreezeTime));
    }
    private IEnumerator WaitAndUnFreeze(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        status = PlayerStatus.Normal;
    }


    public void Explode(float force, Vector3 Dir)
    {
        photonView.RPC("Explosion", photonView.owner, force, Dir);
    }

    [PunRPC]
    public void Explosion(float force, Vector3 dir)
    {
        Debug.Log("GonnaFly: " + force + ", " + dir);
       // status = PlayerStatus.Exploing;
        GetComponent<Rigidbody>().velocity = dir;
        Debug.Log("Explode");
    }

    public GameObject BubbleEffect;
    public void TriggedTrap(bool IsTrapped)
    {
        photonView.RPC("Trap", PhotonTargets.AllBuffered, IsTrapped);
    }

    /// <summary>
    /// Trap or realease this player
    /// </summary>
    /// <param name="IsTrapped"></param>
    [PunRPC]
    public void Trap(bool IsTrapped)
    {
        if (IsTrapped)
        {
            status = PlayerStatus.Trapped;
            BubbleEffect.SetActive(true);
        }else
        {
            status = PlayerStatus.Normal;
            BubbleEffect.SetActive(false);
        }
    }

    [PunRPC]
    public void DrBirdRotation(bool LeftorRight)
    {
        if (LeftorRight) DrBird.transform.eulerAngles = new Vector3(0, 180, 0);
        else DrBird.transform.eulerAngles = new Vector3(0, 0, 0);
    }

}
