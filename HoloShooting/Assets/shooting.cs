using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Microsoft.MixedReality.Toolkit.Input;

public class shooting : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�e�𔭎˂�����ꏊ�͂ǂ��ɂ��那�H�H")]
    private GameObject firingPoint;

    [SerializeField]
    [Tooltip("�e�Ƃ���I�u�W�F�N�g�������ɑ��U�I�I")]
    private GameObject bullet;

    // ������ӂ̓��j���[�Ńv���C���[���ݒ肵���e�̃X�e�[�^�X���󂯎��֐���p���邩���X����Ȃ��q
    [SerializeField]
    [Tooltip("�e�̑��x")]
    private float speed = 30f;

    void Start()
    {
        // this.GetComponent<Rigidbody>().AddForce(transform.forward * 100f);
    }

    void Update() {
        // �Ȃɂ��f���炵���C�C�����̂�����ۂ�������ǂ��悤�Ɏ�������
        // �X�y�[�X�L�[�̉�������
        if (Input.GetKeyDown(KeyCode.Space)) {
            // �e�̔���
            SelectFire();
        }

    }
    private void SelectFire() {
        // �e�̔��ˍ��W���擾
        Vector3 bulletPosition = firingPoint.transform.position;
        // �擾�������W�ɒe��prehub��set
        GameObject setBullet = Instantiate(bullet, bulletPosition, transform.rotation);
        // �e�̐��ʁiz�������j| forward�Fz���i�j, right�Fx���i�ԁj, up�Fy���i�΁j
        Vector3 direction = setBullet.transform.forward;
        // �e��rightbody�ɏՌ��͂������� ��addForce�̑�2����.forceMode
        // https://ekulabo.com/force-mode#outline__2
        setBullet.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Acceleration);
        // �o���������e�̖��O��ύX
        setBullet.name = bullet.name;
        // �e���w�莞�Ԍo�ߌ�ɏ���
        Destroy(setBullet, 2.0f);
    }
}
