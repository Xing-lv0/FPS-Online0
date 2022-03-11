using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public GameObject bullet; //子弹；
    public Transform bulletStartTransform; //子弹初始状态；
    public float bulletStartSpeed = 100; //子弹初速度；
    public bool keepFire = false; //连射；
    public float fireInterval = 0.1f; //连射间隔；

    //后坐力动画：
    //public bool haveRecoil = false;
    public Transform defaultPositionTransform; //武器初始位置；
    public Transform recoilPositionTransform; //后坐力推至的位置；
    public float lerpRatio = 0.2f; //插值参数；

    //音效：
    public AudioSource shotAudio;

    //机瞄视角控制：
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

    //控制开火：
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

    //协程开火：
    IEnumerator Fire()
    {
        while (keepFire)
        {
            if (bullet != null && bulletStartTransform != null)
            {
                //生成子弹：
                GameObject newBullet = Instantiate(bullet, bulletStartTransform.position, bulletStartTransform.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * bulletStartSpeed;

                PlayShotAudio(); //枪声；

                //后坐力动画：
                //haveRecoil = true;
                StopCoroutine("RecoilAnimation");
                StartCoroutine("RecoilAnimation");

                Destroy(newBullet, 5);
            }
            yield return new WaitForSeconds(fireInterval); //中断函数，等待；
        }
    }

    //协程后坐力动画：
    IEnumerator RecoilAnimation()
    {
        yield return null;

        if (defaultPositionTransform != null && recoilPositionTransform != null) 
        {
            //后坐：
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

            //恢复:
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
