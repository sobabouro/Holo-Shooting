using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive (false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void open()
    {
        Invoke("open0", 4.7f);
        Invoke("move2", 6.2f);
    }

    void open0()
    {
        gameObject.SetActive (true);
    }


    void move2()
    {
        this.GetComponent<Rigidbody>().AddForce(-transform.forward * 1000f);
    }

}
