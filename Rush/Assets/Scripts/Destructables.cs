using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour
{
    public int max;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount >= max){
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
