using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private GameObject a;
    [SerializeField] private GameObject b;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Delete()
    {
        a.SetActive(false);
        b.SetActive(false);
    }
}
