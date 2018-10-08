using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondShow : MonoBehaviour
{
    public int index;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        int id = int.Parse(transform.parent.name);
        bool diamondShow = loadGame.safeData.levels.Find(x => x.id == id).diamondShow[index];
        if(diamondShow){
            //this.transform.gameObject.SetActive(true);
            rend.enabled = true;
        }
        else{
            //this.transform.gameObject.SetActive(false);
            rend.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
