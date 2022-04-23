using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlacement : MonoBehaviour
{
    GameObject manager;

 
    void Start()
    {
        manager = GameObject.Find("Admin");
    }

    
    void Update()
    {
    }

    private void OnMouseDown()
    {
        manager.GetComponent<Game_manager>().moveplayer(this.transform.position.x, this.transform.position.z);
        Debug.Log(name);
    }
}
