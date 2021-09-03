using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager instance;

    public Transform simpleBulletHolder;
    public GameObject simpleBulletPrefab;
    public List<GameObject> inactiveSimpleBullets;
    List<GameObject> activeSimpleBullets;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        activeSimpleBullets = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject RequestSimpleBullet()
    {
        if (inactiveSimpleBullets.Count > 0)
        {
            var bullet = inactiveSimpleBullets[0];
            bullet.transform.parent = null;
            bullet.SetActive(true);
            inactiveSimpleBullets.Remove(bullet);
            activeSimpleBullets.Add(bullet);
            return bullet;
        }
        else
        {
            Debug.Log("No more SimpleBullets available, must add more.");
            var newBullet = Instantiate(simpleBulletPrefab);
            activeSimpleBullets.Add(newBullet);
            return newBullet;
            //return null;
        }
    }

    public void ReturnSimpleBullet(Transform bullet)
    {
        bullet.parent = simpleBulletHolder;
        bullet.localPosition = Vector3.zero;
        bullet.gameObject.SetActive(false);
        //activeSimpleBullets.Remove(bullet.gameObject);
        inactiveSimpleBullets.Add(bullet.gameObject);

    }
}
