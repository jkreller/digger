using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpfield : MonoBehaviour
{
    protected MolePlayer molePlayer;
    // Start is called before the first frame update
    void Start()
    {
        molePlayer = FindObjectOfType<MolePlayer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            

            foreach (ContactPoint2D hit in collision.contacts)
            {
                // check if player hits top or side
                if (hit.normal.y < 0)
                {
                    molePlayer.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    molePlayer.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1000, ForceMode2D.Impulse);

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
