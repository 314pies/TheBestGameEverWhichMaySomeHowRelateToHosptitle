using UnityEngine;
using System.Collections;

public class BadGuyDeployment : MonoBehaviour
{

    public LayerMask rayCasyLayer;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
          
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, rayCasyLayer))
            {                
                CreateItem(hit.point);
            }
        }
    }


    public int SelectedItem = -1;
    public string[] ItemList;

    public void SelectItem(int _selectItem)
    {
        SelectedItem = _selectItem;
    }
    public void CreateItem(Vector3 SpawnPos)
    {
        if (SelectedItem != -1)
            PhotonNetwork.Instantiate(ItemList[SelectedItem], SpawnPos,Quaternion.identity,0);
        else
            Debug.Log("No valid item is selected");
    }
}
