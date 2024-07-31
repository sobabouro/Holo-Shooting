using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Microsoft.MixedReality.Toolkit.Input;

public class shooting : MonoBehaviour
{
    [SerializeField]
    [Tooltip("弾を発射させる場所はどこにするぅ？？")]
    private GameObject firingPoint;

    [SerializeField]
    [Tooltip("弾とするオブジェクトをここに装填！！")]
    private GameObject bullet;

    // ここら辺はメニューでプレイヤーが設定した弾のステータスを受け取る関数を用いるから後々いらない子
    [SerializeField]
    [Tooltip("弾の速度")]
    private float speed = 30f;

    void Start()
    {
        // this.GetComponent<Rigidbody>().AddForce(transform.forward * 100f);
    }

    void Update() {
        // なにか素晴らしいイイ感じのそれっぽい処理を良きように実装する
        // スペースキーの押下判定
        if (Input.GetKeyDown(KeyCode.Space)) {
            // 弾の発射
            SelectFire();
        }

    }
    private void SelectFire() {
        // 弾の発射座標を取得
        Vector3 bulletPosition = firingPoint.transform.position;
        // 取得した座標に弾のprehubをset
        GameObject setBullet = Instantiate(bullet, bulletPosition, transform.rotation);
        // 弾の正面（z軸方向）| forward：z軸（青）, right：x軸（赤）, up：y軸（緑）
        Vector3 direction = setBullet.transform.forward;
        // 弾のrightbodyに衝撃力を加える ↓addForceの第2引数.forceMode
        // https://ekulabo.com/force-mode#outline__2
        setBullet.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Acceleration);
        // 出現させた弾の名前を変更
        setBullet.name = bullet.name;
        // 弾を指定時間経過後に消去
        Destroy(setBullet, 2.0f);
    }
}
