using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthEnemy : MonoBehaviour
{
    public GameObject enemy;
    [Tooltip("health not healthBar gameObject")]
    public RectTransform healthBar;
    public Text healthText;
    [Tooltip("script name of gameObj")]
    [SerializeField]
    private string scriptName = "Enemy";
    private float ratio;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //health bar function
    public void OnHitpointChange()
    {
        //check if gameObj exist
        if (!enemy)
            return;
        switch (scriptName)
        {
            case "Boss":
                Boss bossScript = enemy.GetComponent<Boss>();
                ChangeHealth(bossScript);
                break;
            default:
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                ChangeHealth(enemyScript);
                break;
        }
    }

    //change health bar - boss
    private void ChangeHealth(Boss bossScript)
    {
        ratio = (float)bossScript.hitpoint / (float)bossScript.maxHitpoint;
        healthBar.localScale = new Vector3(ratio, 1, 1);
        healthText.text = bossScript.hitpoint.ToString() + " / " + bossScript.maxHitpoint.ToString();
    }
    //change health bar - enemy
    private void ChangeHealth(Enemy enemyScript)
    {
        ratio = (float)enemyScript.hitpoint / (float)enemyScript.maxHitpoint;
        healthBar.localScale = new Vector3(ratio, 1, 1);
        healthText.text = enemyScript.hitpoint.ToString() + " / " + enemyScript.maxHitpoint.ToString();
    }
}
