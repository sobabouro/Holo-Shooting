using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;

public class BulletControl : MonoBehaviour {

    //[SerializeField]
    //public SavingBulletVariable bulletControl = null;

    [SerializeField]
    private AudioMixer _mixer;
    
    private float speed;
    private float size;
    private float originalSpeedVal;
    private float originalSizeVal;
    private float assignSpeedVal;
    private float assignSizeVal;
    
    public float life_time = 5f;
    float time = 0f;

    private int boundcount = 4;

    void Start() {
        SavingBulletVariable bulletControl = GameObject.Find("BulletManager").GetComponent<SavingBulletVariable>();
        speed = bulletControl.BulletSpeed;
        size = bulletControl.BulletSize;
        Set(speed, size);
        TuningSound(speed, size);
    }
    public void Set(float speed, float size) {
        time = 0;
        transform.localScale = new Vector3(size, size, size);
        Vector3 direction = transform.forward;
        //this.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
        this.GetComponent<Rigidbody>().AddForce((direction * (speed)) ,  ForceMode.VelocityChange);
    }
    void Update() {
        time += Time.deltaTime;
        // print (time);
        if(time>life_time){
            Destroy(gameObject);
        }
    }
    private float ConvertDecibel(float volume) =>
        Mathf.Clamp(2f * Mathf.Log10(Mathf.Clamp(volume, 0f, 10f)), -80f, 0f);

    public void TuningSound(float speed_param, float size_param) {
        originalSpeedVal = speed_param - 1.0f;
        originalSizeVal = (size_param - 0.05f) / 0.03f;
        assignSpeedVal = 1f + originalSpeedVal * 0.1f;
        assignSizeVal = 1f / assignSpeedVal;
        GetComponent<AudioSource>().pitch = assignSpeedVal;
        _mixer.SetFloat("fireSoundShiftPitch", assignSizeVal);
        _mixer.GetFloat("fireSoundShiftPitch", out float nowSizeval);
        float tmp = nowSizeval;
        assignSizeVal = tmp + originalSizeVal * 0.1f;
        _mixer.SetFloat("fireSoundShiftPitch", assignSizeVal);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Shield")
        {
            Destroy(this.gameObject);
        }

        boundcount -= 1;
        if (this.boundcount <= 0)
        {
            Destroy(this.gameObject);
        }

    }
}

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor.UIElements;
// using UnityEngine;

// public class BulletControl : MonoBehaviour {
//     [SerializeField]
//     public SavingBulletVariable bulletControl = null;

//     private float speed;
//     private float size;
//     public float life_time = 10f;
//     float time = 0f;

//     void Start() {
//         SavingBulletVariable bulletControl = GameObject.Find("BulletManager").GetComponent<SavingBulletVariable>();
//         speed = bulletControl.BulletSpeed;
//         size = bulletControl.BulletSize;
//         //Debug.Log(speed);
//         Set(speed, size);
//     }
//     public void Set(float speed, float size) {
//         time = 0;
//         transform.localScale = new Vector3(size, size, size);
//         Vector3 direction = transform.forward;
//         //this.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
//         this.GetComponent<Rigidbody>().AddForce((direction * speed) ,  ForceMode.VelocityChange);
//     }
//     void Update() {
//         time += Time.deltaTime;
//         // print (time);
//         if(time>life_time){
//             Destroy(gameObject);
//         }
//     }
// }
