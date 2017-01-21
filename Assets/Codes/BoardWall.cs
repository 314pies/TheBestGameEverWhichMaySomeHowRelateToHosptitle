using UnityEngine;
using System.Collections;

public class BoardWall : MonoBehaviour {

	public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "wave")
        {
            other.SendMessage("DestroySelf");
        }

    }
}
