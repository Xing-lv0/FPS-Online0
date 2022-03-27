using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    //public ControlPlayer hostPlayer; //�ӵ������ߣ�
    public int hostID; //�ӵ�������ID��-1ΪNPC������1Ϊ��ң�
    public float damageValue = 10;

    public GameObject particle; //�ӵ����е�����Ч����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�����ӵ������ߣ�
    public void SetHost(int id)
    {
        hostID = id;
    }
    public void SetHost(ControlPlayer player)
    {
        //hostPlayer = player;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�ж��ӵ������ߣ�
        if (hostID > 0) //��ҵ��ӵ���
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<HealthControl>().Damage(damageValue);
                collision.gameObject.GetComponent<EnemyControl>().enemyAnimator.TriggerOnDamage();
            }
        }
        else //NPC���ӵ���
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<HealthControl>().Damage(damageValue);
                collision.gameObject.GetComponent<ControlPlayer>().controlBotAnimator.TriggerOnDamage();
            }
        }

        ParticleExplode();
        Destroy(this.gameObject);
    }

    private void ParticleExplode()
    {
        if (particle)
        {
            GameObject newParticle = Instantiate(particle, this.transform.position, particle.transform.rotation);

            Destroy(newParticle, 3);
        }
    }
}
