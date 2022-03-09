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
    public Transform defaultPositionTransform; //������ʼλ�ã�
    public Transform recoilPositionTransform; //������������λ�ã�
    public float lerpRatio = 0.2f; //��ֵ������

    // Start is called before the first frame update
    void Start()
    {
        
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

                GameObject newBullet = Instantiate(bullet, bulletStartTransform.position, bulletStartTransform.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * bulletStartSpeed;

                //������������
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
            while (this.transform.localPosition != recoilPositionTransform.localPosition)
            {
                print("this:" + this.transform.localPosition);
                print("default:" + recoilPositionTransform.localPosition);
                print("recoil:" + recoilPositionTransform.localPosition);
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, recoilPositionTransform.localPosition, lerpRatio);
                yield return null;
            }

            //�ָ�:
            while (this.transform.localPosition != defaultPositionTransform.localPosition)
            {
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, defaultPositionTransform.localPosition, lerpRatio);
                yield return null;
            }
        }
    }
}