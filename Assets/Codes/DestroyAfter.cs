using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour {

    public float time = 3;
	// Use this for initialization
	void Start () {
        StartCoroutine(Delete());
	}
	
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
