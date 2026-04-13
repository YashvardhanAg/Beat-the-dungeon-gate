using UnityEngine;
using UnityEngine.UIElements;

public class Powerups : Numbers //inheritance
{
    private float rSpeed = 205.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OutBoundscheck();
        Translation(rSpeed); //Polymorphism
    }
}
