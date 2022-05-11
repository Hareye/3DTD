using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyObject : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyStats enemyStats;
    private EnemyController enemyController;
    private CurrencyController currencyController;
    private ElementalSystem elementalSystem;

    [SerializeField] private List<GameObject> targets = new List<GameObject>(); // list of enemies in radius
    private GameObject currentTarget;
    private float nextAttack;

    private Queue<DamageInfo> damageQueue = new Queue<DamageInfo>();

    private string enemyName;
    private string element;
    private float takeDamageMultiplier;

    private float maxHP;
    private float currHP;
    private float baseDamage;
    private float damage;
    private float attackSpeed;
    private float range;
    private float moveSpeed;
    private Vector3 goal = new Vector3(0, -21, -205);

    private float healAmount;
    private float healRate;

    private int worth;

    private Slider healthBar;
    [SerializeField] private Image healthBarImage;
    private Color maxHPColor;
    private Color minHPColor;

    // Light / Dark marks
    private bool hasLightMark;
    private bool hasDarkMark;

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
        enemyName = enemyStats.name;
        element = enemyStats.element;
        takeDamageMultiplier = enemyStats.damageMultiplier;

        maxHP = enemyStats.maxHealth;
        currHP = enemyStats.maxHealth;
        baseDamage = enemyStats.damage;
        damage = enemyStats.damage;
        attackSpeed = enemyStats.attackSpeed;
        range = enemyStats.range;
        moveSpeed = enemyStats.speed;

        healAmount = enemyStats.healAmount;
        healRate = enemyStats.healRate;

        worth = enemyStats.worth;

        nextAttack = 0.0f;

        enemyController = transform.parent.GetComponent<EnemyController>();
        elementalSystem = transform.parent.GetComponent<ElementalSystem>();
        currencyController = GameObject.Find("CurrencyContainer").GetComponent<CurrencyController>();

        healthBar = GetComponentInChildren<Slider>();
        maxHPColor = new Color(42f / 255f, 255f / 255f, 46f / 255f);
        minHPColor = new Color(255f / 255f, 87f / 255f, 61f / 255f);
        updateHealthBar();
    }

    private void Update()
    {
        attackCycle();

        if (damageQueue.Count != 0)
        {
            DamageInfo dInfo = damageQueue.Dequeue();

            if ((currHP - dInfo.damageTaken) <= 0)
                damageQueue.Clear();

            takeDamage(dInfo.damageTaken, dInfo.tower);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Destroy(this.gameObject);
        }
    }

    private void attackCycle()
    {
        if (targets.Count > 0) // enemies in range
        {
            currentTarget = selectBestTarget();
            attackTarget(currentTarget);
        }
    }

    private GameObject selectBestTarget()
    {
        float closestDist = 999;
        GameObject closestTarget = targets[0];

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                if (Vector3.Distance(transform.position, targets[i].transform.position) < closestDist)
                {
                    closestDist = Vector3.Distance(transform.position, targets[i].transform.position);
                    closestTarget = targets[i].gameObject;
                }
            }
        }
        return closestTarget;
    }

    private void attackTarget(GameObject target)
    {
        if (Time.time > nextAttack)
        {
            if (target != null)
            {
                target.GetComponent<IDamageable>().queueDamage(damage, this.gameObject);
                nextAttack = Time.time + (1 / attackSpeed);
            }
        }
    }

    public void addTarget(GameObject target)
    {
        targets.Add(target);
    }

    public void removeTarget(GameObject target)
    {
        targets.Remove(target);
    }

    public void queueDamage(float dmgTaken, GameObject tower)
    {
        DamageInfo dInfo = new DamageInfo(dmgTaken, tower);
        damageQueue.Enqueue(dInfo);
    }

    public void queueDamagePlayer(float dmgTaken)
    {
        takeDamagePlayer(dmgTaken);
    }

    private void takeDamage(float dmgTaken, GameObject tower)
    {
        if (tower != null)
        {
            float elementalDamageMultiplier = elementalSystem.getElementalMultiplier(tower.GetComponent<TowerObject>().getElement(), element);
            float realDmg = dmgTaken * elementalDamageMultiplier;

            if (tower.GetComponent<TowerBuffHandler>().getLifestealEnabled())
                tower.GetComponent<TowerObject>().AddHP(realDmg * tower.GetComponent<TowerBuffHandler>().getLifestealPercent());

            currHP -= realDmg;
            updateHealthBar();
            checkDeath(tower);
        }
    }

    private void takeDamagePlayer(float dmgTaken)
    {
        currHP -= dmgTaken;
        updateHealthBar();
        if (currHP <= 0)
        {
            enemyController.decrementEnemiesAlive();
            currencyController.addMoney(worth);
            Destroy(gameObject);
        }
    }
    
    private void updateHealthBar()
    {
        float hpPercent = getHealthPercent();

        healthBar.value = hpPercent;
        healthBarImage.color = Color.Lerp(minHPColor, maxHPColor, hpPercent / 100);
    }

    private float getHealthPercent()
    {
        return (currHP / maxHP) * 100;
    }

    private void checkDeath(GameObject tower)
    {
        if (currHP <= 0)
        {
            if (tower != null)
                tower.GetComponent<TowerObject>().RemoveTarget(this.gameObject);

            enemyController.decrementEnemiesAlive();
            currencyController.addMoney(worth);
            Destroy(gameObject);
        }
    }

    public void addHP(float toAdd)
    {
        if (currHP + toAdd > maxHP)
        {
            currHP = maxHP;
        }
        else
        {
            currHP += toAdd;
        }
        updateHealthBar();
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

    public void reduceDamage(float percent)
    {
        // reduce damage by percent
        damage = (damage * (1 - percent));
    }

    public void resetDamage()
    {
        damage = baseDamage;
    }

    public string getElement()
    {
        return element;
    }

    public float getHeal()
    {
        return healAmount;
    }

    public float getHealRate()
    {
        return healRate;
    }

    public float getCurrentHP()
    {
        return currHP;
    }
    public string getName()
    {
        return enemyName;
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

    public float getDistFromGoal()
    {
        return Vector3.Distance(this.transform.position, goal);
    }

    public void applyLightMark(TowerObject towerObj)
    {
        float dmg = towerObj.getDamage() * 2; // change damage value based on upgrade level

        if (!hasLightMark)
            hasLightMark = true;

        if (checkMarks())
        {
            queueDamage(dmg, towerObj.gameObject);
        }
    }

    public void applyDarkMark(TowerObject towerObj)
    {
        float dmg = towerObj.getDamage() * 2; // change damage value based on upgrade level

        if (!hasDarkMark)
            hasDarkMark = true;

        if (checkMarks())
        {
            queueDamage(dmg, towerObj.gameObject);
        }
    }

    public bool getLightMark()
    {
        return hasLightMark;
    }

    public bool getDarkMark()
    {
        return hasDarkMark;
    }

    public bool checkMarks()
    {
        if (hasLightMark && hasDarkMark)
        {
            hasLightMark = false;
            hasDarkMark = false;
            return true;
        } else
        {
            return false;
        }
    }
}
