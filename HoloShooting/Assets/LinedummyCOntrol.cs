using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;

public class LinedummyCOntrol : MonoBehaviour
{
    [SerializeField, Range(0, 90)]
    private float indexThreshold = 20;

    [SerializeField, Range(0, 90)]
    private float middleThreshold = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IndexfingerDetected() &&  MiddlefingerDetected() == false)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

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
