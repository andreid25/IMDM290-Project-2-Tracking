using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject[] projectiles;
    public float shotSpeedMin = 1.2f;
    public float shotSpeedMax = 2f;
    public AnimationCurve curve;
    public float timeBetweenShotMin = .5f;
    public float timeBetweenShotMax = 1.2f;
    public float xRange = 1;
    public float yRange = .6f;
    float shotBreak;
    float timeSinceLastFire = 0;
    // Start is called before the first frame update
    void Start()
    {
        shotBreak = UnityEngine.Random.Range(timeBetweenShotMin, timeBetweenShotMax);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastFire += Time.deltaTime;
        if (timeSinceLastFire >= shotBreak)
        {
            StartCoroutine(FireProjectile());
            timeSinceLastFire = 0;
            shotBreak = UnityEngine.Random.Range(timeBetweenShotMin, timeBetweenShotMax);
        }
    }

    IEnumerator FireProjectile()
    {
        float xTarget = UnityEngine.Random.Range(Camera.main.transform.position.x - xRange, Camera.main.transform.position.x + xRange);
        float yTarget = UnityEngine.Random.Range(Camera.main.transform.position.y - yRange, Camera.main.transform.position.y + yRange);
        Vector3 target = new Vector3(xTarget, yTarget, Camera.main.transform.position.z);
        Vector3 start = gameObject.transform.position;
        Vector3 rotation = new Vector3(UnityEngine.Random.Range(0, 180), UnityEngine.Random.Range(0, 180), UnityEngine.Random.Range(0, 180));

        GameObject projectile = Instantiate(projectiles[(int)UnityEngine.Random.Range(0, projectiles.Length)], start, Quaternion.identity);
        ProjectileScript projectileScript = projectile.AddComponent<ProjectileScript>();
        
        float timePassed = 0;
        float shotLength = UnityEngine.Random.Range(shotSpeedMin, shotSpeedMax);
        while (projectile.transform.position != target && !projectileScript.hit)
        {
            timePassed += Time.deltaTime;
            float yCurveVal = curve.Evaluate(timePassed / shotLength);
            Vector3 interpolatedPosition = Vector3.Lerp(start, target, timePassed/shotLength);
            interpolatedPosition.y += yCurveVal;
            projectile.transform.position = interpolatedPosition;
            projectile.transform.Rotate(rotation * Time.deltaTime);
            yield return null;
        }
        if (projectileScript.hit)
        {
            start = new Vector3(projectile.transform.position.x, projectile.transform.position.y, projectile.transform.position.z);
            xTarget = UnityEngine.Random.Range(start.x - 2, start.x + 2);
            yTarget = UnityEngine.Random.Range(start.y - 1.2f, start.y + 1.2f);
            target = new Vector3(xTarget, yTarget, start.z - 4);

            timePassed = 0;
            while (projectile.transform.position != target)
            {
                timePassed += Time.deltaTime;
                //float yCurveVal = curve.Evaluate(timePassed / shotLength);
                Vector3 interpolatedPosition = Vector3.Lerp(start, target, timePassed / 1);
                //interpolatedPosition.y += yCurveVal;
                projectile.transform.position = interpolatedPosition;
                projectile.transform.Rotate(rotation * Time.deltaTime);
                yield return null;
            }
        }
        Destroy(projectile);
    }
}
