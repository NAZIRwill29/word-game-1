using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Collectable
{
    [Tooltip("HpPotion / MpPotion / ExpPotion / IDPotion")]
    public string potionName;
    [Tooltip("20 / 50")]
    public int potionAmount;
    private Player playerScript;
    protected override void Start()
    {
        base.Start();
        playerScript = GameManager.instance.player;
    }
    protected override void OnCollide(Collider2D coll)
    {
        if (collected)
            return;
        if (coll.name == "Player")
        {
            // Debug.Log("player");
            collected = true;
            StartCoroutine(DeleteObj());
            switch (potionName)
            {
                case "HpPotion":
                    playerScript.Heal(potionAmount, true);
                    break;
                case "MpPotion":
                    playerScript.Heal(potionAmount, false);
                    break;
                case "ExpPotion":
                    GameManager.instance.GrantXp(potionAmount);
                    GameManager.instance.ShowText("+" + potionAmount + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
                    break;
                case "IDPotion":
                    playerScript.IncreaseDamagePotion(potionAmount);
                    break;
                default:
                    Debug.Log("none");
                    break;
            }
        }
    }

    //coroutine destroy
    private IEnumerator DeleteObj()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
