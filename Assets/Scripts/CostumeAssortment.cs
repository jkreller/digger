using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumeAssortment : MonoBehaviour
{
    public Transform selectButtonPrefab;

    protected MolePlayer molePlayer;

    // Start is called before the first frame update
    void Start()
    {
        molePlayer = FindObjectOfType<MolePlayer>();
        RefreshPurchasedCostumes();
    }

    public void RefreshPurchasedCostumes()
    {
        List<string> purchasedCostumes = loadGame.safeData.purchasedCostumes;
        string currentCostume = loadGame.safeData.currentCostume;
        string standardCostumeName = "mole_standard";
        string selectText = "Select";
        string selectedText = "Selected";

        foreach (Transform costume in transform)
        {
            Transform diamondsTextTransform = costume.Find("Diamonds");
            bool costumeSelected = costume.name == currentCostume || (costume.name == standardCostumeName && currentCostume == null);

            if (purchasedCostumes.Contains(costume.name) || costume.name == standardCostumeName)
            {
                Transform oldSelectButton = costume.Find("SelectButton(Clone)");
                if (oldSelectButton)
                {
                    Text oldSelectButtonText = oldSelectButton.GetComponent<Text>();
                    if (costumeSelected)
                    {
                        oldSelectButtonText.text = selectedText;
                    }
                    else
                    {
                        oldSelectButtonText.text = selectText;
                    }
                }
                else
                {
                    Button costumeButton = costume.GetComponent<Button>();
                    diamondsTextTransform.gameObject.SetActive(false);

                    Transform selectButton = Instantiate(selectButtonPrefab, diamondsTextTransform.position, Quaternion.identity, diamondsTextTransform.parent);
                    Text selectButtonText = selectButton.GetComponent<Text>();
                    selectButton.localScale = new Vector2(0.22f, 0.22f);
                    selectButton.GetComponent<Button>().onClick.AddListener(() => molePlayer.ChangeCostumeWithSelect(costume.name));

                    if (costumeSelected)
                    {
                        selectButtonText.text = selectedText;
                    }
                    else
                    {
                        selectButtonText.text = selectText;
                    }

                    costumeButton.onClick = new Button.ButtonClickedEvent();
                    costumeButton.onClick.AddListener(() => molePlayer.ChangeCostumeWithSelect(costume.name));
                }
            }
        }
    }
}
