using UnityEngine;
using System.Collections;

public class Pivot : MonoBehaviour {

    public Transform centre;
    public int speed;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(centre.position, Vector3.back, speed * Time.deltaTime);

    }
}
