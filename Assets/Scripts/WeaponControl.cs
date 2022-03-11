using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public GameObject bullet; //�ӵ���
    public Transform bulletStartTransform; //�ӵ���ʼ״̬��
    public float bulletStartSpeed = 100; //�ӵ����ٶȣ�
    public bool keepFire = false; //���䣻
    public float fireInterval = 0.1f; //��������

    //������������
    //public bool haveRecoil = false;
    public Transform defaultPositionTransform; //������ʼλ�ã�
    public Transform recoilPositionTransform; //������������λ�ã�
    public float lerpRatio = 0.2f; //��ֵ������

    //��Ч��
    public AudioSource shotAudio;

    //�����ӽǿ��ƣ�
    public Camera mainCamera;
    public Camera weaponCamera;
    public Vector3 weaponCameraDefaultPosition;
    public Vector3 weaponCameraAimPosition;
    public float defaultFOV = 60;
    public float aimFOV = 30;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetCompent<Camera>();
        weaponCamera = GameObject.FindGameObjectWithTag("WeaponCamera").GetCompent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        FireControl();
    }

    //���ƿ���
    void FireControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            keepFire = true;
            StartCoroutine("Fire");
        }
        if (Input.GetMouseButtonUp(0))
        {
            keepFire = false;
            StopCoroutine("Fire");
        }
    }

    //Э�̿���
    IEnumerator Fire()
    {
        while (keepFire)
        {
            if (bullet != null && bulletStartTransform != null)
            {
                //�����ӵ���
                GameObject newBullet = Instantiate(bullet, bulletStartTransform.position, bulletStartTransform.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * bulletStartSpeed;

                PlayShotAudio(); //ǹ����

                //������������
                //haveRecoil = true;
                StopCoroutine("RecoilAnimation");
                StartCoroutine("RecoilAnimation");

                Destroy(newBullet, 5);
            }
            yield return new WaitForSeconds(fireInterval); //�жϺ������ȴ���
        }
    }

    //Э�̺�����������
    IEnumerator RecoilAnimation()
    {
        yield return null;

        if (defaultPositionTransform != null && recoilPositionTransform != null) 
        {
            //������
            //if (haveRecoil)
            {
                while (this.transform.localPosition != recoilPositionTransform.localPosition)
                {
                    //print("this:" + this.transform.localPosition);
                    //print("default:" + recoilPositionTransform.localPosition);
                    //print("recoil:" + recoilPositionTransform.localPosition);
                    this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, recoilPositionTransform.localPosition, lerpRatio * 4);
                    yield return null;
                }
            }

            //�ָ�:
            while (this.transform.localPosition != defaultPositionTransform.localPosition)
            {
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, defaultPositionTransform.localPosition, lerpRatio);
                yield return null;
            }
        }
    }

    private void PlayShotAudio()
    {
        if (shotAudio)
            shotAudio.Play();
    }

    private void CameraAim()
    {
        if (Input.GetMouseButtonDown(1))
        {

        }

        if (Input.GetMouseButtonUp(1))
        {

        }
    }
}
