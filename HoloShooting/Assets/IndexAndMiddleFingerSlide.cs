using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine.Events;
using UnityEngine.UI;

public class IndexAndMiddleFingerSlide : MonoBehaviour {
    [SerializeField, Header("Target")]
    Handedness handType;

    [SerializeField, Range(0, 90)]
    private float indexThreshold = 5;

    [SerializeField, Range(0, 90)]
    private float middleThreshold = 5;

    [SerializeField, Range(1, 10)]
    private float rateOfAcceleration = 5;

    [SerializeField] 
    public GameObject menu = null;

    private bool? handdetected;
    private Vector3 previousPosition;
    private int countdown;
    private int countup;
    private Vector3 AppearancePosition;

    private void FixedUpdate() {
        handdetected = HandJointUtils.FindHand(handType)?.TryGetJoint(TrackedHandJoint.Palm, out MixedRealityPose PalmPose);
        if (handdetected != null && handdetected == true) {
            if (IndexfingerDetected() && MiddlefingerDetected()) {
                if (Mathf.Approximately(Time.deltaTime, 0)) {
                    return;
                }
                HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose);
                float v = (Vector3.Distance(pose.Position, previousPosition) / Time.deltaTime) ;
                if (((previousPosition.y - pose.Position.y) > 0)/* && menu.activeSelf == false*/)  {
                    if (countdown == 1)
                    {
                        AppearancePosition = pose.Position;
                        AppearancePosition.x += 0.2f;
                        AppearancePosition.y -= 0.18f;
                    }
                    if (countdown >= 20 && menu.activeSelf == false/* && AppearancePosition.y - pose.Position.y > 0.1f*/) {
                        menu.SetActive(true);
                        countdown = 0;
                        Transform myTransform = menu.transform;
                        menu.transform.position = AppearancePosition;
                    }
                    countdown++;
                    countup = 0;
                }
                else if ( ((previousPosition.y - pose.Position.y) < 0)/*&& menu.activeSelf*/) {
                    if (countup >= 20 && menu.activeSelf/* && AppearancePosition.y - pose.Position.y < 0.1f*/) {
                        menu.SetActive(false);
                        countup = 0;
                    }
                    countup++;
                    countdown = 0;
                }
                previousPosition = pose.Position;
                
            }
        }
    }

    // // 手のひらが自分に向いている（true）
    // private bool PalmDetected() {
    //     var jointedHand = HandJointUtils.FindHand(handType);
    //     if (jointedHand.TryGetJoint(TrackedHandJoint.Palm,out MixedRealityPose palmPose)) {
    //         if (Vector3.Angle(palmPose.Up, CameraCache.Main.transform.forward) < facingThreshold) {
    //             return true;
    //         }
    //     }
    //     return false;
    // }
    
    // 人差し指が伸びている（true）
    private bool IndexfingerDetected() {
        var jointedHand = HandJointUtils.FindHand(Handedness.Right);
        if (jointedHand.TryGetJoint(TrackedHandJoint.Palm, out MixedRealityPose PalmPose)) {
            //各関節のpose
            MixedRealityPose indexTipPose, indexDistalPose, IndexKnucklePose, indexMiddlePose;
            if (jointedHand.TryGetJoint(TrackedHandJoint.IndexTip, out indexTipPose) && jointedHand.TryGetJoint(TrackedHandJoint.IndexDistalJoint, out indexDistalPose) && jointedHand.TryGetJoint(TrackedHandJoint.IndexMiddleJoint, out indexMiddlePose) && jointedHand.TryGetJoint(TrackedHandJoint.IndexKnuckle, out IndexKnucklePose)) {
                // 各関節の位置情報がワールド座標で算出されるされるため、
                // finger(f1~f14)は各指の関節をPalmの子オブジェクトに変換している
                Vector3 f1 = IndexKnucklePose.Position - PalmPose.Position;
                Vector3 f2 = indexMiddlePose.Position - IndexKnucklePose.Position;
                Vector3 f3 = indexDistalPose.Position - indexMiddlePose.Position;
                Vector3 f4 = indexTipPose.Position - indexDistalPose.Position;

                // Vector3.Angle()で各関節間の角度を取得
                float c = Vector3.Angle(PalmPose.Position, f1);
                float d = Vector3.Angle(f1, f2);
                float e = Vector3.Angle(f2, f3);
                float f = Vector3.Angle(f3, f4);

                float aba = (Mathf.Abs(d) + Mathf.Abs(e) + Mathf.Abs(f)) / 3;

                if (aba < indexThreshold) {
                    return true;
                }
            }
        }
        return false;
    }

    // 中指が伸びている（true）
    private bool MiddlefingerDetected() {
        var jointedHand = HandJointUtils.FindHand(Handedness.Right);
        if (jointedHand.TryGetJoint(TrackedHandJoint.Palm, out MixedRealityPose PalmPose)) {
            MixedRealityPose middleTipsPose, middleDistalPose, middleKnucklePose, middleMiddlePose;
            if (jointedHand.TryGetJoint(TrackedHandJoint.MiddleTip, out middleTipsPose) &&
                jointedHand.TryGetJoint(TrackedHandJoint.MiddleDistalJoint, out middleDistalPose) &&
                jointedHand.TryGetJoint(TrackedHandJoint.MiddleKnuckle, out middleKnucklePose) && jointedHand.TryGetJoint(TrackedHandJoint.MiddleMiddleJoint, out middleMiddlePose)) {
                Vector3 f1 = middleKnucklePose.Position - PalmPose.Position;
                Vector3 f2 = middleMiddlePose.Position - middleKnucklePose.Position;
                Vector3 f3 = middleDistalPose.Position - middleMiddlePose.Position;
                Vector3 f4 = middleTipsPose.Position - middleDistalPose.Position;

                float c = Vector3.Angle(PalmPose.Position, f1);
                float d = Vector3.Angle(f1, f2);
                float e = Vector3.Angle(f2, f3);
                float f = Vector3.Angle(f3, f4);

                float aba = (Mathf.Abs(d) + Mathf.Abs(e) + Mathf.Abs(f)) / 3;

                if (aba < middleThreshold) {
                    return true;
                }
            }
        }
        return false;
    }
}
