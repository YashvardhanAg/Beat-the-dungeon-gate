using Unity.Collections;
using UnityEngine;

public class Numbers : MonoBehaviour
{
     private float yRange = -50.0f;
     private float speed = 75f;
    private float rotateSpeed = 0;
     public int value; 
    // Update is called once per frame
    void Update()
    {
        OutBoundscheck();
        Translation(rotateSpeed);
    }
    protected void OutBoundscheck()
    {
        if (gameObject.transform.position.y < yRange)
        {
            Destroy(gameObject);
        }
    }
    protected void Translation(float rotateSpeed)
    {
        transform.Translate(0, -speed * Time.deltaTime, 0);
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}