using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionCheck : MonoBehaviour
{
    public bool collided;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision col)
    {
        UnityEngine.Debug.Log("collided");
        collided = true;
    }
}
