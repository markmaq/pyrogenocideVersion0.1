using UnityEngine;

public class enemy : MonoBehaviour
{
    public int enemyHealth = 100;
    int currentEnemyHealth;
    private Animator enemyAnim;

    void Start()
    {
        currentEnemyHealth = enemyHealth;
        enemyAnim = GetComponent<Animator>();
    }


    void Update()
    {
        if (currentEnemyHealth <= 0)
        {
            //Destroy(this.gameObject, 4f);


        }
    }

    public void TakeDamage(int damage)
    {
        currentEnemyHealth -= damage;
        enemyAnim.SetBool("isDead", true);
    }
}
