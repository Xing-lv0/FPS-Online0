using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public int ID; //����������ID;
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
    public float viewLerpRatio = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //weaponCamera = GameObject.FindGameObjectWithTag("WeaponCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        FireControl();
        ViewChange();

        Ray ray = new Ray(bulletStartTransform.position, bulletStartTransform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
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
                newBullet.GetComponent<BulletControl>().SetHost(ID);
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

    private void ViewChange()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StopCoroutine("ToDefaultView");
            StartCoroutine("ToAimView");
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopCoroutine("ToAimView");
            StartCoroutine("ToDefaultView");
        }
    }

    //�л����龵ͷ��
    IEnumerator ToAimView()
    {
        //print("to AimPosition: " + weaponCameraAimPosition);
        while (weaponCamera.transform.localPosition != weaponCameraAimPosition)
        {
            weaponCamera.transform.localPosition = Vector3.Lerp(weaponCamera.transform.localPosition,
                weaponCameraAimPosition, viewLerpRatio);
            weaponCamera.fieldOfView = Mathf.Lerp(weaponCamera.fieldOfView, aimFOV, viewLerpRatio);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, aimFOV, viewLerpRatio);

            yield return null; //�ȴ�1֡��
        }
    }

    //�л�����ͨ��ͷ��
    IEnumerator ToDefaultView()
    {
        //print("to DefaultPosition: " + weaponCameraDefaultPosition);
        while (weaponCamera.transform.localPosition != weaponCameraDefaultPosition)
        {
            weaponCamera.transform.localPosition = Vector3.Lerp(weaponCamera.transform.localPosition,
                weaponCameraDefaultPosition, viewLerpRatio);
            weaponCamera.fieldOfView = Mathf.Lerp(weaponCamera.fieldOfView, defaultFOV, viewLerpRatio);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, defaultFOV, viewLerpRatio);

            yield return null; //�ȴ�1֡��
        }
    }
}
