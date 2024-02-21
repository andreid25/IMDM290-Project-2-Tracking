using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionCheck : MonoBehaviour
{
    public List<AudioClip> slapSounds;
    
    // Start is called before the first frame update
    void OnCollisionEnter(Collision col)
    {
        
        GetComponent<AudioSource>().clip = slapSounds[(int)UnityEngine.Random.Range(0, 2)];
        GetComponent<AudioSource>().Play();
        //UnityEngine.Debug.Log("collided");
        ProjectileScript ps =  col.gameObject.GetComponent<ProjectileScript>();
       if(ps)
       {
         ps.hit = true;
       }
    }
}
