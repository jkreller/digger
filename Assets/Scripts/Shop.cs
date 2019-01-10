using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Transform selectButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform costume in transform)
        {
            Transform diamondsTextTransform = costume.Find("Diamonds");
            diamondsTextTransform.gameObject.SetActive(false);

            Transform selectButton = Instantiate(selectButtonPrefab, diamondsTextTransform.position, Quaternion.identity, diamondsTextTransform.parent);
            selectButton.localScale = new Vector2(0.22f, 0.22f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
