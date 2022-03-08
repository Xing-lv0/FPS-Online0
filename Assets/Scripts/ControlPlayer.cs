using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    public float rotateSpeed = 180;
    [Range(1, 2)]
    public float rotateRatio = 1;
    public Transform playerTransform;
    public Transform viewTransform;
    private float rotateOffsetX; //x��תƫ�ƣ�
    public float xLimit = 90; //�Ƕ����ƣ�

    public CharacterController controller;
    public float moveSpeed = 10; //ˮƽ���٣�

    public float gravity = -9.8f;
    public float verticalVelocity = 0;
    public bool landed = false;
    public Transform landingCheckTransform; //��أ�
    public float landingCheckRadius = 0.1f; //����ж��뾶��
    public LayerMask floorLayer; //����㣻
    public float jumpHeight = 1; //��Ծ�߶ȣ�

    public GameObject bullet; //�ӵ���
    public Transform bulletStartTransform; //�ӵ���ʼ״̬��
    public float bulletStartSpeed = 100; //�ӵ����ٶȣ�
    public bool keepFire = false; //���䣻
    public float fireInterval = 0.25f; //��������

    public ControlBotAnimator controlBotAnimator; //�����˶������ƣ�

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        controlBotAnimator = this.GetComponent<ControlBotAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateControl();
        MoveControl();
        ControlFire();
    }

    void FixedUpdate()
    {
        
    }

    //�ӽ���ת��
    void RotateControl()
    {
        if (playerTransform == null || viewTransform == null)
            return;

        float offsetX = Input.GetAxis("Mouse X"); //���x����ƫ�ƿ���Player��y��ˮƽת����
        float offsetY = Input.GetAxis("Mouse Y"); //���y����ƫ�ƿ���View��x�ᴹֱת����

        playerTransform.Rotate(Vector3.up * offsetX * rotateSpeed * rotateRatio * Time.fixedDeltaTime); //����

        //����
        //print(offsetY);
        rotateOffsetX += offsetY * rotateSpeed * rotateRatio * Time.fixedDeltaTime;
        rotateOffsetX = Mathf.Clamp(rotateOffsetX, -xLimit, xLimit);
        Quaternion currentLocalRotation = Quaternion.Euler(new Vector3(rotateOffsetX,viewTransform.localEulerAngles.y, viewTransform.localEulerAngles.z));
        viewTransform.localRotation = currentLocalRotation;
    }

    //λ���ƶ���
    void MoveControl()
    {
        if (controller == null)
            return;

        float hInput = Input.GetAxis("Horizontal"); //�����ƶ���
        float vInput = Input.GetAxis("Vertical"); //ǰ���ƶ���

        Vector3 moveVector = Vector3.zero;

        moveVector += this.transform.right * moveSpeed * hInput * Time.deltaTime;
        moveVector += this.transform.forward * moveSpeed * vInput * Time.deltaTime;

        //�����ƶ���
        verticalVelocity += gravity * Time.deltaTime;

        moveVector += Vector3.up * verticalVelocity * Time.deltaTime; //���٣�

        //��ؼ�⣺
        if (landingCheckTransform != null)
        {
            landed = Physics.CheckSphere(landingCheckTransform.position, landingCheckRadius, floorLayer);
            if (landed && verticalVelocity < 0) //��⵽��أ����ٶ����£��������ٶȼ�Ϊ0��
            {
                landed = true;
                verticalVelocity = 0;
            }
        }

        //��Ծ��
        if (landed)
        {
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(2 * jumpHeight * (-gravity));
            }
        }

        controller.Move(moveVector);

        //���ö���������
        if (controlBotAnimator)
        {
            controlBotAnimator.moveSpeed = moveSpeed * vInput;
            controlBotAnimator.alerted = vInput != 0 ? true : false;
        }
    }

    //���ƿ���
    void ControlFire()
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

                Destroy(newBullet, 5);
            }
            yield return new WaitForSeconds(fireInterval); //�жϺ������ȴ���
        }
    }
}
