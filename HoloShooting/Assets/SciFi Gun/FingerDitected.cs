using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.UI;
using Photon.Pun;
using Photon.Realtime;

public class FingerDitected : MonoBehaviour, IMixedRealityPointerHandler {
    [SerializeField, Header("Target")]
    Handedness handType;

    [SerializeField, Range(0, 90)]
    private float indexThreshold = 5;

    [SerializeField, Range(0, 90)]
    private float middleThreshold = 5;

    //[SerializeField]
    //private GameObject crosshairPrefab = null;

    [SerializeField]
    public GameObject bullet = null;

    [SerializeField]
    public PinchSlider speedSlider = null;

    [SerializeField]
    public PinchSlider intervalSlider = null;

    [SerializeField]
    public PinchSlider sizeSlider = null;

    [SerializeField]
    public SavingBulletVariable bulletManager = null;

    [SerializeField]
    public GameObject line = null;

    private bool? handdetected;
    private GameObject crosshair = null;
    private ShellHandRayPointer RayPointer;
    private float timer;
    private float fireTime;

    private void OnEnable() {
        // グローバルリスナーに登録
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
    }
    private void OnDisable() {
        // グローバルリスナーの登録を解除
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
    }
    public void OnPointerDown(MixedRealityPointerEventData eventData) {
        // つまんだときに呼ばれる処理
    }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) {
        // ドラッグしたときに呼ばれる処理
    }
    public void OnPointerUp(MixedRealityPointerEventData eventData) {
        // オブジェクトを離したときに呼ばれる処理
    }
    public void OnPointerClicked(MixedRealityPointerEventData eventData) {
        // クリックしたときに呼ばれる処理(オブジェクトにフォーカスしていなくても実行される)
        var hand = HandJointUtils.FindHand(handType);
        if (hand != null && hand.TryGetJoint(TrackedHandJoint.Palm, out MixedRealityPose PalmPose)) {
            handdetected = true;
            if (IndexfingerDetected() && MiddlefingerDetected()) {
                SelectFire();
            }
        }
        else {
            handdetected = false;
        }
    }
    void Update() {
        var hand = HandJointUtils.FindHand(handType);
        if (hand != null && hand.TryGetJoint(TrackedHandJoint.Palm, out MixedRealityPose PalmPose)) {
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose);
            if (IndexfingerDetected() && MiddlefingerDetected()) {
                line.gameObject.SetActive(true);
            }
            else {
                line.gameObject.SetActive(false);
            }
        }
        else {
            line.gameObject.SetActive(false);
        }
    }

    // 人差し指
    private bool IndexfingerDetected() {
        var jointedHand = HandJointUtils.FindHand(Handedness.Right);
        if (jointedHand != null && jointedHand.TryGetJoint(TrackedHandJoint.Palm, out MixedRealityPose PalmPose)) {
            //各関節のpose
            MixedRealityPose indexTipPose, indexDistalPose, IndexKnucklePose, indexMiddlePose;
            if (jointedHand.TryGetJoint(TrackedHandJoint.IndexTip, out indexTipPose) &&
                jointedHand.TryGetJoint(TrackedHandJoint.IndexDistalJoint, out indexDistalPose) &&
                jointedHand.TryGetJoint(TrackedHandJoint.IndexMiddleJoint, out indexMiddlePose) &&
                jointedHand.TryGetJoint(TrackedHandJoint.IndexKnuckle, out IndexKnucklePose)) {
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

    // 中指
    private bool MiddlefingerDetected() {
        var jointedHand = HandJointUtils.FindHand(Handedness.Right);
        if (jointedHand != null && jointedHand.TryGetJoint(TrackedHandJoint.Palm, out MixedRealityPose PalmPose)) {
            MixedRealityPose middleTipsPose, middleDistalPose, middleKnucklePose, middleMiddlePose;
            if (jointedHand.TryGetJoint(TrackedHandJoint.MiddleTip, out middleTipsPose) &&
                jointedHand.TryGetJoint(TrackedHandJoint.MiddleDistalJoint, out middleDistalPose) &&
                jointedHand.TryGetJoint(TrackedHandJoint.MiddleKnuckle, out middleKnucklePose) &&
                jointedHand.TryGetJoint(TrackedHandJoint.MiddleMiddleJoint, out middleMiddlePose)) {
                Vector3 f1 = middleKnucklePose.Position - PalmPose.Position;
                Vector3 f2 = middleMiddlePose.Position - middleKnucklePose.Position;
                Vector3 f3 = middleDistalPose.Position - middleMiddlePose.Position;
                Vector3 f4 = middleTipsPose.Position - middleDistalPose.Position;

                float c = Vector3.Angle(PalmPose.Position, f1);
                float d = Vector3.Angle(f1, f2);
                float e = Vector3.Angle(f2, f3);
                float f = Vector3.Angle(f3, f4);

                float aba = (Mathf.Abs(d) + Mathf.Abs(e) + Mathf.Abs(f)) / 3;

                if (aba > middleThreshold) {
                    return true;
                }

            }
        }
        return false;
    }

    private void SelectFire() {
        timer = Time.time;
        if (intervalSlider == null || speedSlider == null || sizeSlider == null || bulletManager == null) {
            Debug.LogError("Slider or BulletManager is null");
            return;
        }

        float interval = 1.3f - (Mathf.Log(intervalSlider.SliderValue + 1, 2)) / Mathf.Log(11, 2);
        float speed = (0.5f * speedSlider.SliderValue + 1);
        float size = 0.05f + (sizeSlider.SliderValue * 0.03f);

        bulletManager.BulletSpeed = speed;
        bulletManager.BulletSize = size;

        if (timer - fireTime >= interval) {
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose)) {
                // プレハブのbulletをインスタンス化
                GameObject bulletObj = PhotonNetwork.Instantiate("bullet", pose.Position, pose.Rotation, 0);
                // bulletObj.GetComponent<bulletManager>().Set(speed, size);
                // transform.localScale = new Vector3(size, size, size);
                // Vector3 direction = transform.forward;
                // GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Acceleration);
                fireTime = Time.time;
            }
        }
    }
}
