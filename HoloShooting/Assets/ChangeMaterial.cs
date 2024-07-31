using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] Material mat = default;
    [SerializeField] Material maton;
    [SerializeField] public GameObject plate;
    private int matcount = 0;

    // Start is called before the first frame update
    void Start()
    {
        matcount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMat()
    {
        if(matcount == 0)
        {
            GetComponent<MeshRenderer>().material = maton;
            plate.SetActive(true);
            matcount = 1;
        }
        else
        {
            GetComponent<MeshRenderer>().material = mat;
            plate.SetActive(false);
            matcount = 0;
        }
    }
}
