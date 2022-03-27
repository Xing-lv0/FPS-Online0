using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour
{
    public float HP = 100;
    public Slider HP_slider;
    public float maxHP = 100;

    public GameObject deathParticle; //ËÀÍöÁ£×ÓÌØÐ§£»

    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        if (HP_slider)
        {
            HP_slider.value = HP / maxHP * HP_slider.maxValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        if (HP > 0)
        {
            HP -= damage;
            if (HP_slider)
            {
                HP_slider.value = HP / maxHP * HP_slider.maxValue;
            }
        }
        if (HP <= 0)
        {
            //ËÀÍö£º
            DeathParticleExplode();
            Destroy(this.gameObject);
        }
    }

    private void DeathParticleExplode()
    {
        if (deathParticle)
        {
            GameObject newParticle = Instantiate(deathParticle, this.transform.position, deathParticle.transform.rotation);

            Destroy(newParticle, 3);
        }
    }
}
