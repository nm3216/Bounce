using UnityEngine;
using System.Collections;

public class Loiter : MonoBehaviour {

    bool rotateLeft;
    float initialPos;

    void Start()
    {
        rotateLeft = true;
        initialPos = GetComponent<Transform>().position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform tr = GetComponent<Transform>();
        

        float displacement = tr.position.y - initialPos;

        if (displacement > 2)
        {
            rotateLeft = false;
        }

        if (displacement < -2)
        {
            rotateLeft = true;
        }

        if (rotateLeft)
        {
            tr.localPosition += new Vector3(0, 0.05f, 0);
        } else {
            tr.localPosition -= new Vector3(0, 0.05f, 0);
        }

    }
}
