using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class points : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int id = int.Parse(transform.parent.name);
        Text text = this.gameObject.GetComponent<Text>();
        int gotBlues = loadGame.safeData.levels.Find(x => x.id == id).blue;
        int totalBlues = loadGame.safeData.levels.Find(x => x.id == id).blueTotal;
        text.text = gotBlues.ToString() + "/" + totalBlues;
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
