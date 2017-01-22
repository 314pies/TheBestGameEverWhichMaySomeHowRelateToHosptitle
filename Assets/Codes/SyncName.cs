using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SyncName : MonoBehaviour {


    public Text NameUI;
    public string NameStr;
	
    // Use this for initialization
	void Start () {

        NameStr = PlayerPrefs.GetString("Name", "Player");
        StartCoroutine(WaitAndSync());
    }
    
    IEnumerator WaitAndSync()
    {
        yield return new WaitForSeconds(0.15f);
        GetComponent<PhotonView>().RPC("UpdateName",PhotonTargets.All, NameStr);
    }	


    [PunRPC]
    public void UpdateName(string _Name)
    {
        NameUI.text = _Name;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
