using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    //public ControlPlayer hostPlayer; //子弹发射者；
    public int hostID; //子弹发射者ID，-1为NPC，大于1为玩家；
    public float damageValue = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //设置子弹发射者；
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
        //判断子弹所有者：
        if (hostID > 0) //玩家的子弹；
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<HealthControl>().Damage(damageValue);
            }
        }
        else //NPC的子弹；
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<HealthControl>().Damage(damageValue);
            }
        }

        Destroy(this.gameObject);
    }
}
