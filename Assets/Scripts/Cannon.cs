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
        float xTarget = UnityEngine.Random.Range(Camera.main.transform.position.x - 1, Camera.main.transform.position.x + 1);
        float yTarget = UnityEngine.Random.Range(Camera.main.transform.position.y - .6f, Camera.main.transform.position.y + .6f);
        Vector3 target = new Vector3(xTarget, yTarget, Camera.main.transform.position.z);
        Vector3 start = gameObject.transform.position;

        GameObject projectile = Instantiate(projectiles[(int)UnityEngine.Random.Range(0, projectiles.Length)], start, Quaternion.identity);
        float timePassed = 0;
        float shotLength = UnityEngine.Random.Range(shotSpeedMin, shotSpeedMax);
        while (projectile.transform.position != target)
        {
            timePassed += Time.deltaTime;
            float yCurveVal = curve.Evaluate(timePassed / shotLength);
            Vector3 interpolatedPosition = Vector3.Lerp(start, target, timePassed/shotLength);
            interpolatedPosition.y += yCurveVal;
            projectile.transform.position = interpolatedPosition;
            yield return null;
        }
        Destroy(projectile);
    }
}
