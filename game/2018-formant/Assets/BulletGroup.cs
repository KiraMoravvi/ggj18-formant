using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup : MonoBehaviour {

    public int NUM_BULLETS;
    public Bullet Instance;
    List<Bullet> bulletList;

    // Use this for initialization
    void Start () {
        bulletList = new List<Bullet>();
        for (int i = 0; i < NUM_BULLETS; i++)
        {
            bulletList.Add(Instantiate(Instance));
            bulletList[i].Kill();
        }
	}

    public Bullet GetBullet()
    {
        for (int i = 0; i < NUM_BULLETS; i++)
        {
            if (!bulletList[i].gameObject.activeInHierarchy)
            {
                return bulletList[i];
            }
        }

        return null;
    }

    public void Fire(Vector3 pos, Vector3 direction, float speed)
    {
        Bullet b = GetBullet();

        if (b)
        {
            b.Activate();
            b.RB.position = pos;
            b.RB.velocity = direction * speed;
        }
    }
}
