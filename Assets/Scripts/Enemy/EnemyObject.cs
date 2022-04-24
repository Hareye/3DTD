using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyStats enemyStats;
    private EnemyController enemyController;
    private CurrencyController currencyController;

    private Queue<DamageInfo> damageQueue = new Queue<DamageInfo>();

    private string name;
    private string element;
    private float takeDamageMultiplier;

    private float maxHP;
    private float currHP;
    private float damage;
    private float attackSpeed;
    private float range;
    private float moveSpeed;

    private int worth;

    struct DamageInfo
    {
        public float damageTaken;
        public GameObject tower;

        public DamageInfo(float dmgTaken, GameObject obj)
        {
            damageTaken = dmgTaken;
            tower = obj;
        }
    }

    private void Start()
    {
        name = enemyStats.name;
        element = enemyStats.element;
        takeDamageMultiplier = enemyStats.damageMultiplier;

        maxHP = enemyStats.maxHealth;
        currHP = enemyStats.maxHealth;
        damage = enemyStats.damage;
        attackSpeed = enemyStats.attackSpeed;
        range = enemyStats.range;
        moveSpeed = enemyStats.speed;

        worth = enemyStats.worth;

        enemyController = transform.parent.GetComponent<EnemyController>();
        currencyController = GameObject.Find("CurrencyContainer").GetComponent<CurrencyController>();
    }

    private void Update()
    {
        if (damageQueue.Count != 0)
        {
            DamageInfo dInfo = damageQueue.Dequeue();

            if ((currHP - dInfo.damageTaken) <= 0)
                damageQueue.Clear();

            takeDamage(dInfo.damageTaken, dInfo.tower);
        }
    }

    public void queueDamage(float dmgTaken, GameObject tower)
    {
        Debug.Log("Queuing damage...");
        DamageInfo dInfo = new DamageInfo(dmgTaken, tower);
        damageQueue.Enqueue(dInfo);
    }

    private void takeDamage(float dmgTaken, GameObject tower)
    {
        Debug.Log("Taking damage...");
        currHP -= dmgTaken;
        checkDeath(tower);
    }

    private void checkDeath(GameObject tower)
    {
        if (currHP <= 0)
        {
            tower.GetComponent<TowerController>().RemoveTarget(this.gameObject);
            Debug.Log(name + " died...");
            enemyController.decrementEnemiesAlive();
            currencyController.addMoney(worth);
            Destroy(gameObject);
        }
    }

    public void setMaxHP(int hp)
    {
        maxHP = hp;
    }

    public void setCurrHP(int hp)
    {
        currHP = hp;
    }

    public void setMoveSpeed(float ms)
    {
        moveSpeed = ms;
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }

    public string getName()
    {
        return name;
    }

    public float getMaxHP()
    {
        return maxHP;
    }

    public float getCurrHP()
    {
        return currHP;
    }

    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    public float getDamage()
    {
        return damage;
    }
}
