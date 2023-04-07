using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniToBig : MonoBehaviour
{

    private Vector3 scaleChange;

    private void Start()
    {
        scaleChange = new Vector3(0.001f, 0.00005f, 0.001f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < 1.2f) transform.localScale += scaleChange;
    }
}
