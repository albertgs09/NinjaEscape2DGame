using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeControl : MonoBehaviour
{
    public bool IsFiring;
    public Knife knife;
    public KnifeOpposite knifeL;
    public EnemyC ec;
    public Enemy2 ec2;
    public float bulletSpeed;

    public float timeBetweenShots;
    private float shotCounter;



    public Transform firePoint;


    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        if(ec != null)
        {
            if (IsFiring && ec.playerDead == false)
            {

                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0)
                {
                    //  gunSound.clip = pistol;
                    //gunSound.Play();
                    shotCounter = timeBetweenShots;
                    if (ec.facingRight)
                    {
                        KnifeOpposite newKnifeOpposite = Instantiate(knifeL, firePoint.position, firePoint.rotation) as KnifeOpposite;
                        newKnifeOpposite.speed = bulletSpeed;
                    }
                    else
                    {
                        Knife newKnife = Instantiate(knife, firePoint.position, firePoint.rotation) as Knife;
                        newKnife.speed = bulletSpeed;
                    }


                }
            }
            else
            {
                shotCounter = 0;
            }

        }

        if(ec2 != null)
        {
            if (IsFiring && ec2.playerDead == false)
            {

                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0)
                {
                    //  gunSound.clip = pistol;
                    //gunSound.Play();
                    shotCounter = timeBetweenShots;
                    if (ec2.facingRight)
                    {
                        KnifeOpposite newKnifeOpposite = Instantiate(knifeL, firePoint.position, firePoint.rotation) as KnifeOpposite;
                        newKnifeOpposite.speed = bulletSpeed;
                    }
                    else
                    {
                        Knife newKnife = Instantiate(knife, firePoint.position, firePoint.rotation) as Knife;
                        newKnife.speed = bulletSpeed;
                    }


                }
            }
            else
            {
                shotCounter = 0;
            }
        }
       
    }
}
