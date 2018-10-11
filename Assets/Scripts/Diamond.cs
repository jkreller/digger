using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour {
    bool top = false;
    Vector2 actualPosition;
    double randomNumber;
    public int jumpscale;
    public int slower;
    public int value;
    public bool blue;
    public int diamondShow;



    /*
     * set random number for movement
     */
	void Start () {
        Random.seed = (int)System.DateTime.Now.Ticks;
        actualPosition = this.transform.position;
        randomNumber = Random.Range(0, 10000);
	}
	
    /*
     * Move the diamond from top to bottom and back
     */
    void Update()
    {
        if (randomNumber >= 0)
        {
            randomNumber = randomNumber - Time.deltaTime * 5000;
        } else
        {
            if (top)
                transform.Translate(Vector2.up * (20 - slower) * Time.deltaTime);
            else
                transform.Translate(-Vector2.up * (20 - slower) * Time.deltaTime);
		
            if (transform.position.y >= actualPosition.y + 2f-jumpscale)
            {
                top = false;
            }
		
            if (transform.position.y <= actualPosition.y - 2+jumpscale)
            {
                top = true;
            }
        }
    }

   
}
