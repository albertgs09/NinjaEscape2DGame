using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeOpposite : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate(0, 0, 50 * Time.deltaTime);
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        //Destroy(gameObject, 2.5f);
    }
}
