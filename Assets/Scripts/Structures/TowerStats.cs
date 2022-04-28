using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower Stats", menuName = "Tower/Stats")]
public class TowerStats : ScriptableObject
{
    public string towerName;
    public string element;
    public float damageMultiplier;

    public int maxHealth;
    public float damage;
    public float attackSpeed;
    public float range;
    public float special;

    public float heal;
    public float healRate;

    public float slowPercent;
    public float slowDuration;

    public string specialDesc;
    public string specialUpgradeDesc;

    public int cost;

    public string[] upgrades;
}
