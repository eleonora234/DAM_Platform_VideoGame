using UnityEngine;

public class platformMove : MonoBehaviour
{
    float startingX;
    public float targetX;
    public float Speed;

    bool goingRight = true;
    bool goingLeft ;

    private void FixedUpdate()
    {
        if (goingRight)
        {
            transform.position += new Vector3 (Speed, 0, 0);
            if (transform.position.x > targetX)
            { 
                goingRight = false;
                goingLeft = true;
            }

        }


        if (goingLeft)
        {
         
            transform.position -= new Vector3(Speed, 0, 0);
            if (transform.position.x < startingX)
            {
                goingRight = true;
                goingLeft = false;
            }
        }

    }


    private void Awake()
    {
        startingX = transform.position.x;
    }
}
