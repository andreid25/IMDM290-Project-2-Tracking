using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision col)
    {
        //UnityEngine.Debug.Log("collided");
        ProjectileScript ps =  col.gameObject.GetComponent<ProjectileScript>();
       if(ps)
       {
         ps.hit = true;
       }
    }
}
