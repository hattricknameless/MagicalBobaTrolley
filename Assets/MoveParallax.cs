using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParallax : MonoBehaviour
{
    public float speedMove;
 private float length, startposX, startposY;
    public float parallaxEffectX;
    public float parallaxEffectY;

    // Start is called before the first frame update
    void Start()
    {
        startposX = transform.position.x;
        startposY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
startposX += speedMove * Time.deltaTime;

        float temp = (0 );
        float distX = (( speedMove * Time.deltaTime) * parallaxEffectX);
        float distY = (0);

        transform.position = new Vector3(startposX + distX, startposY + distY, transform.position.z);

        if (temp > startposX + length) startposX += length;
        else if (temp < startposX - length) startposX -= length;
    }
}
