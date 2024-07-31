using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class BreakShield : MonoBehaviourPunCallbacks
{
    private int numberOfShield = 3;
    //[SerializeField]
    //public ShieldCounting ShieldCount = null;

    //private GameObject shield; //シールド

    //[SerializeField]
    //private UnityEvent onShieldBreak; //イベント発生時呼び出す
    [SerializeField]
    private Material[] _material; //割り当てるマテリアル

    [SerializeField]
    private ParticleSystem particle;

    public AudioClip sound1;
    AudioSource audioSource;

    //[SerializeField] UnityEvent win;
    //[SerializeField] UnityEvent lose;


    void Start()
    {
        this.enabled = false;
        this.numberOfShield = 3;
        audioSource = GetComponent<AudioSource>();
    }


    void OnCollisionEnter(Collision collision)               
    {
        this.numberOfShield -= 1; //シールドの残機をマイナス１する
        Debug.Log(this.numberOfShield); //確認用
        Debug.Log("timpo");

        if (this.numberOfShield >= 0)
        {
            this.GetComponent<Renderer>().material = _material[this.numberOfShield]; 
            //start particle
            particle.Play();
            audioSource.PlayOneShot(sound1);
        }

        //シールドがすべて破壊された時の処理
        else if (this.numberOfShield < 0 && photonView.IsMine)
        {
            Destroy(this.gameObject); //シールドは消える
            Debug.Log("broken");
            photonView.RPC(nameof(changescene), RpcTarget.All, "a");
        }
    }

    [PunRPC]
    private void changescene(string a)
    {
        Debug.Log("change"); 

        if (photonView.IsMine)
        {
            gotolose();
        }
        else
        {
            gotowin();
        }
    }

    void gotolose()
    {
        Debug.Log("lose");
        SceneManager.LoadScene("Result_Lose");
    }

    void gotowin()
    {
        Debug.Log("win");
        SceneManager.LoadScene("Result_Win");

    }

}
