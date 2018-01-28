using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup : MonoBehaviour {

    public int NUM_BULLETS;
    public Bullet Instance;
    List<Bullet> bulletList;

    public List<AudioClip> Pew = new List<AudioClip>();

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
            AudioSource.PlayClipAtPoint(Pew[Random.Range(0, Pew.Count)], new Vector3(pos.x, pos.y, 5.0f));

            b.Activate();
            b.RB.position = pos;
            b.RB.velocity = direction * speed;
        }
    }
}
