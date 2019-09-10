using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShop : MonoBehaviour
{
    protected Animator animator;
    protected bool isPressed;
    protected MolePlayer mole;
    protected GameLogic gameLogic;
    protected CostumeAssortment costumeAssortment;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mole = FindObjectOfType<MolePlayer>();
        gameLogic = FindObjectOfType<GameLogic>();
        costumeAssortment = FindObjectOfType<CostumeAssortment>();
    }

    void Buy()
    {
        if (!string.IsNullOrEmpty(mole.costume) && !loadGame.safeData.purchasedCostumes.Contains(mole.costume))
        {
            int diamondsBefore = loadGame.safeData.diamonds;
            int costs = int.Parse(GameObject.Find(mole.costume).transform.Find("Diamonds").Find("DiamondAmount").GetComponent<Text>().text);

            if (!(diamondsBefore - costs < 0))
            {
                // pay
                loadGame.safeData.diamonds = diamondsBefore - costs;
                // add costume
                loadGame.safeData.purchasedCostumes.Add(mole.costume);
                // set costume
                loadGame.safeData.SetCurrentCostume(mole.costume);

                loadGame.Save(loadGame.safeData);

                gameLogic.UpdateDiamondText();
            }
        }

        costumeAssortment.RefreshPurchasedCostumes();
    }

    void SetButtonPressed() {
        animator.SetInteger("AnimState", 2);
        Buy();
    }

    void SetButtonReleased()
    {
        animator.SetInteger("AnimState", 0);
        isPressed = false;
    }

    void PressButton()
    {
        if (!isPressed) {
            isPressed = true;
            animator.SetInteger("AnimState", 1);
        }
    }

    void ReleaseButton()
    {
        if (isPressed)
        {
            animator.SetInteger("AnimState", 3);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D hit in other.contacts)
            {
                // if hit on top
                if (hit.normal.y < -0.9f) {
                    PressButton();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ReleaseButton();
        }
    }
}
