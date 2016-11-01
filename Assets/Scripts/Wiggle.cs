using UnityEngine;
using System.Collections;

public class Wiggle : MonoBehaviour {
    bool rotateLeft;

    void Start()
    {
        rotateLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        Transform tr = GetComponent<Transform>();

        if (tr.eulerAngles.z > 20 && tr.eulerAngles.z < 25)
        {
            rotateLeft = false;
        }

        if (tr.eulerAngles.z > 340 && tr.eulerAngles.z < 345)
        {
            rotateLeft = true;
        }

        if (rotateLeft)
        {
            transform.Rotate(new Vector3(0, 0, 50) * Time.deltaTime);
        } else {
            transform.Rotate(new Vector3(0, 0, -50) * Time.deltaTime);
        }

    }

}
