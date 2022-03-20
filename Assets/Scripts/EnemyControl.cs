using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    public NavMeshAgent enemyAgent;
    public GameObject patrolRoot;
    public Transform[] patrolSpots; //Ѳ�ߵ�λ��
    public int nextIndex = 0;

    public ControlPlayer playerControl;
    public float alertDistance = 10; //������룻

    public GameObject bullet;
    public float bulletStartSpeed = 100; //�ӵ����ٶȣ�
    public Transform firePositionTransform;
    public float sightAngle = 30; //��Ұ�нǣ�
    public bool isFiring; //���ڿ���
    public float fireInternal = 0.1f; //��������

    // Start is called before the first frame update
    void Start()
    {
        enemyAgent = this.GetComponent<NavMeshAgent>();
        playerControl = GameObject.FindObjectOfType<ControlPlayer>();

        if (patrolRoot)
        {
            patrolSpots = patrolRoot.GetComponentsInChildren<Transform>();
            SetNextDestnation();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(enemyAgent.transform.position, playerControl.enemySightPositionTransform.position) < alertDistance)
        {
            enemyAgent.destination = playerControl.enemySightPositionTransform.position; //������ң�
        }
        else
        {
            //�Զ�Ѳ�ߣ�
            if (!enemyAgent.pathPending && enemyAgent.remainingDistance < 5)
            //NavMeshAgent.pathPending:  /�����ڼ�������ж���δ������·����/ Is a path in the process of being computed but not yet ready?
            {
                SetNextDestnation();
            }
        }

        FireControl();
    }

    //������һ��Ŀ�ĵأ�
    private void SetNextDestnation()
    {
        if (patrolSpots.Length <= 1)
            return;

        enemyAgent.destination = patrolSpots[nextIndex].position;
        nextIndex = (nextIndex + 1) % patrolSpots.Length ;
    }

    //������ƣ�
    void FireControl()
    {
        Vector3 playerDirection = (playerControl.enemySightPositionTransform.position - firePositionTransform.position).normalized;
        if (Vector3.Angle(playerDirection, firePositionTransform.forward) < sightAngle)
        {
            if (!isFiring)
            {
                isFiring = true;
                StartCoroutine("Fire", playerDirection);
            }
        }
        else
        {
            if (isFiring)
            {
                isFiring = false;
                StopCoroutine("Fire");
            }
        }
    }

    IEnumerator Fire(Vector3 playerDirection)
    {
        //yield return new WaitForSeconds(fireInternal);
        while (isFiring)
        {
            Quaternion rotation = Quaternion.LookRotation(playerDirection);
            GameObject newBullet = Instantiate(bullet, firePositionTransform.position, rotation);
            newBullet.GetComponent<BulletControl>().SetHost(-1);
            newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * bulletStartSpeed;
            Destroy(newBullet, 5);

            yield return new WaitForSeconds(fireInternal);
        }

    }
}
