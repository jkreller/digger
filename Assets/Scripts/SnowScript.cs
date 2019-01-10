using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowScript : MonoBehaviour
{

    public GameObject snow;
    double scale;
    double yPosition;
    double timer = 0.5;
    // Start is called before the first frame update
    void Start()
    {
        scale = this.transform.localScale.x;
        yPosition = this.transform.position.y;
      


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.transform.position);

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            GameObject generatedSnow1 = (GameObject)Instantiate(snow, new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x), transform.position.y + transform.localScale.y+8 / 2, Random.Range(-2,2)), transform.rotation);
            GameObject generatedSnow2 = (GameObject)Instantiate(snow, new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x), transform.position.y + transform.localScale.y+8 / 2, Random.Range(-2,2)), transform.rotation);
            timer = Random.Range(0f, 0.3f);
        }


    }
}
