using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbieController : MonoBehaviour
{
    /*private float xBound = 8f;
    private float yBound = 4f;
    private float zBound = 3f;*/

    private float speed = 5;
    private Vector3 dir = new Vector3(1,1,1);

    private bool playerTouched = false;

    private Rigidbody rigidbody;

    //private bool isGameOver = false;

    private void Start() 
    {
        rigidbody =  GetComponent<Rigidbody>();
    }

    void Update()
    {
        rigidbody.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }

    //bounce the ball with an angle of 45 degree
    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Wall"))
            transform.Rotate(Vector3.up, 45);
    }

    //reduce speed when the frisbie is in the player hand (to do)
    IEnumerator ReduceSpeed()
    {
        while(playerTouched)
        {
            yield return new WaitForSeconds(1);
            speed = Mathf.Min(1, speed - 1);
        }
    }

    //player throw the ball is forward direction with or without a diganol direction
    void RegularThrow(bool isDiagnol)
    {
        if (!isDiagnol)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        else
            transform.Translate(dir /*to do diagonal movement*/ * speed * Time.deltaTime);
    }

    //take as paramater the power of the player and launch a super attack
    void SuperSonicShot(float characterPower)
    {
        transform.Translate(dir * (speed + characterPower) * Time.deltaTime);
    }

    //launch a parabolic attack
    void Lob()
    {
        transform.Translate(dir /* Mathf.Sin*/ * speed * Time.deltaTime);
    }

}
