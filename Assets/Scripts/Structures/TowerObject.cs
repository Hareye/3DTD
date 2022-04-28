using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

public class TowerObject : MonoBehaviour, IDamageable
{
    [SerializeField] private TowerStats towerStats;

    private BuildController buildCon;
    private AttackDict attackDict;

    private string name;
    private string element;
    private float takeDamageMultiplier;

    private float maxHP;
    private float currentHP;
    private float baseDamage;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float damage;
    private float fireRate;
    private float range;
    private float special;

    private int hpLvl;
    private int dmgLvl;
    private int fireRateLvl;
    private int rangeLvl;
    private int specialLvl;

    private int resaleValue;
    private int cost;

    private float nextFire;
    GameObject currentTarget;
    [SerializeField] private List<GameObject> targets = new List<GameObject>(); // list of enemies in radius
    private GameObject detectionRadius;
    private Vector3 detectionScale;
    private bool drawRadius = false;
    public GameObject attackFX;

    private Queue<DamageInfo> damageQueue = new Queue<DamageInfo>();

    private bool keepAttacking = true;

    private Slider healthBar;
    [SerializeField] private Image healthBarImage;
    private Color maxHPColor;
    private Color minHPColor;

    struct DamageInfo
    {
        public float damageTaken;
        public GameObject enemy;

        public DamageInfo(float dmgTaken, GameObject obj)
        {
            damageTaken = dmgTaken;
            enemy = obj;
        }
    }

    private void Start()
    {
        Debug.Log("Creating TowerObject... (TowerObject)");
        name = towerStats.towerName;
        element = towerStats.element;
        takeDamageMultiplier = towerStats.damageMultiplier;

        maxHP = towerStats.maxHealth;
        currentHP = towerStats.maxHealth;
        baseDamage = towerStats.damage;
        damageMultiplier = towerStats.damageMultiplier;
        damage = towerStats.damage;
        fireRate = towerStats.attackSpeed;
        range = towerStats.range;
        special = towerStats.special;

        hpLvl = 0;
        dmgLvl = 0;
        fireRateLvl = 0;
        rangeLvl = 0;
        specialLvl = 0;

        cost = towerStats.cost;
        resaleValue = (int)Mathf.Ceil(cost * 0.75f);

        attackDict = transform.parent.parent.gameObject.GetComponent<AttackDict>();

        buildCon = FindObjectOfType<BuildController>();
        nextFire = 0.0f;
        detectionRadius = this.gameObject.transform.GetChild(0).gameObject;
        detectionScale = new Vector3(range, range, range);
        SetGlobalScale(detectionRadius.transform, detectionScale);

        healthBar = GetComponentInChildren<Slider>();
        maxHPColor = new Color(42f / 255f, 255f / 255f, 46f / 255f);
        minHPColor = new Color(255f / 255f, 87f / 255f, 61f / 255f);
        updateHealthBar();

        StartCoroutine(startAttackCycle());
    }

    private void Update()
    {
        if (damageQueue.Count != 0)
        {
            DamageInfo dInfo = damageQueue.Dequeue();

            if ((currentHP - dInfo.damageTaken) <= 0)
                damageQueue.Clear();

            takeDamage(dInfo.damageTaken, dInfo.enemy);
        }

        if (buildCon.getInBuild() && drawRadius)
        {
            detectionRadius.GetComponent<MeshRenderer>().enabled = true;
        } else
        {
            detectionRadius.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void queueDamage(float dmgTaken, GameObject enemy)
    {
        DamageInfo dInfo = new DamageInfo(dmgTaken, enemy);
        damageQueue.Enqueue(dInfo);
    }

    private void takeDamage(float dmgTaken, GameObject enemy)
    {
        currentHP -= dmgTaken;
        updateHealthBar();
        checkDeath(enemy);
    }

    private void updateHealthBar()
    {
        float hpPercent = getHealthPercent();

        healthBar.value = hpPercent;
        healthBarImage.color = Color.Lerp(minHPColor, maxHPColor, hpPercent / 100);
    }

    private float getHealthPercent()
    {
        return (currentHP / maxHP) * 100;
    }

    private void checkDeath(GameObject enemy)
    {
        if (currentHP <= 0)
        {
            enemy.GetComponent<EnemyObject>().removeTarget(this.gameObject);
            Destroy(gameObject);
        }
    }

    private IEnumerator startAttackCycle()
    {
        while (keepAttacking)
        {
            // Wait until there are targets in range and tower is ready to attack
            yield return StartCoroutine(readyToAttack());
            currentTarget = selectBestTarget();
            attackTarget(currentTarget);
        }
    }

    private IEnumerator readyToAttack()
    {
        yield return new WaitUntil(() => targets.Count > 0 && Time.time > nextFire);
    }

    public void AddTarget(GameObject target)
    {
        targets.Add(target);
    }

    public void RemoveTarget(GameObject target)
    {
        targets.Remove(target);
    }

    private void attackTarget(GameObject target)
    {
        if (target != null)
        {
            GameObject atk = Instantiate(attackDict.getAttackFX(name), target.transform.position, Quaternion.identity);
            
            switch (attackDict.getAttackName(name))
            {
                case "Blast":
                    atk.GetComponent<BlastAttack>().target = target;
                    atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                    target.GetComponent<IDamageable>().queueDamage(getDamage(), this.gameObject);
                    Destroy(atk, 1.0f);
                    break;
                case "Chill":
                    atk.GetComponent<ChillAttack>().setParentTower(this.gameObject);
                    atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                    Destroy(atk, 1.2f);
                    break;
                case "Zap":
                    atk.GetComponent<ZapAttack>().parentTower = this.gameObject;
                    atk.GetComponent<ZapAttack>().BeginAttack(this.gameObject, target, 3);
                    break;
            }

            nextFire = Time.time + (1 / fireRate);
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
                if (Vector3.Distance(this.transform.position, targets[i].transform.position) < closestDist)
                {
                    closestDist = Vector3.Distance(this.transform.position, targets[i].transform.position);
                    closestTarget = targets[i].gameObject;
                }
            }
        }
        return closestTarget;
    }

    public void setOutline(bool isSelected)
    {
        this.gameObject.GetComponent<Outline>().enabled = isSelected;
    }

    public void AddHP(float amount)
    {
        if (currentHP + amount > maxHP)
        {
            currentHP = maxHP;
        } else
        {
            currentHP += amount;
        }
        updateHealthBar();
    }

    public void healToFull()
    {
        currentHP = maxHP;
        updateHealthBar();
    }

    public string[] getUpgrades()
    {
        return towerStats.upgrades;
    }

    public void setHPLevel(int newLvl)
    {
        hpLvl = newLvl;
    }

    public void setDMGLevel(int newLvl)
    {
        dmgLvl = newLvl;
    }

    public void setFireRateLevel(int newLvl)
    {
        fireRateLvl = newLvl;
    }

    public void setRangeLevel(int newLvl)
    {
        rangeLvl = newLvl;
    }

    public void setSpecialLevel(int newLvl)
    {
        specialLvl = newLvl;
    }

    public string getSpecialDesc()
    {
        return towerStats.specialDesc;
    }

    public string getSpecialUpgradeDesc()
    {
        return towerStats.specialUpgradeDesc;
    }

    public int getHPLevel()
    {
        return hpLvl;
    }

    public int getDMGLevel()
    {
        return dmgLvl;
    }

    public int getFireRateLevel()
    {
        return fireRateLvl;
    }

    public int getRangeLevel()
    {
        return rangeLvl;
    }

    public int getSpecialLevel()
    {
        return specialLvl;
    }

    public int getResaleValue()
    {
        return resaleValue;
    }

    public string getElement()
    {
        return element;
    }

    public string getName()
    {
        return name;
    }

    public float getMaxHP()
    {
        return maxHP;
    }

    public float getCurrentHP()
    {
        return currentHP;
    }

    public void increaseDamage(float percent)
    {
        // increase damage by percent
        damageMultiplier += percent;
    }

    public void reduceDamage(float percent)
    {
        damageMultiplier -= percent;
    }

    public float getDamage()
    {
        return damage*damageMultiplier;
    }

    public float getFireRate()
    {
        return fireRate;
    }

    public float getRange()
    {
        return range;
    }

    public float getSpecial()
    {
        return special;
    }

    public int getCost()
    {
        return towerStats.cost;
    }

    public float getHeal()
    {
        return towerStats.heal;
    }

    public float getHealRate()
    {
        return towerStats.healRate;
    }

    public float getSlowPercent()
    {
        return towerStats.slowPercent;
    }

    public float getSlowDuration()
    {
        return towerStats.slowDuration;
    }
    
    private void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }
}
