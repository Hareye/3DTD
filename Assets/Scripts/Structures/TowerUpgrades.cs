using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrades : MonoBehaviour
{
    Dictionary<string, int> costDict = new Dictionary<string, int>();
    Dictionary<string, float> valueDict = new Dictionary<string, float>();

    private void Start()
    {
        /*
        // HP Costs
        costDict.Add("level 1 hpCost", 100);
        costDict.Add("level_2_hpCost", 100);
        costDict.Add("level_3_hpCost", 100);
        costDict.Add("level_4_hpCost", 100);
        costDict.Add("level_5_hpCost", 100);

        // HP Values
        valueDict.Add("level_1_newHP", 100);
        valueDict.Add("level_2_newHP", 100);
        valueDict.Add("level_3_newHP", 100);
        valueDict.Add("level_4_newHP", 100);
        valueDict.Add("level_5_newHP", 100);
        */

        // DAMAGE
        costDict.Add("level_1_dmgCost", 150);
        costDict.Add("level_2_dmgCost", 600);
        costDict.Add("level_3_dmgCost", 1200);
        costDict.Add("level_4_dmgCost", 2000);
        costDict.Add("level_5_dmgCost", 4000);

        valueDict.Add("level_1_newDmg_Fire", 25);
        valueDict.Add("level_2_newDmg_Fire", 25);
        valueDict.Add("level_3_newDmg_Fire", 50);
        valueDict.Add("level_4_newDmg_Fire", 100);
        valueDict.Add("level_5_newDmg_Fire", 150);

        // FIRE RATE
        costDict.Add("level_1_frCost", 200);
        costDict.Add("level_2_frCost", 800);
        costDict.Add("level_3_frCost", 1500);
        costDict.Add("level_4_frCost", 3000);
        costDict.Add("level_5_frCost", 5000);

        valueDict.Add("level_1_newFr_Fire", 1.05f);
        valueDict.Add("level_2_newFr_Fire", 1.1f);
        valueDict.Add("level_3_newFr_Fire", 1.15f);
        valueDict.Add("level_4_newFr_Fire", 1.2f);
        valueDict.Add("level_5_newFr_Fire", 1.25f);

        // RANGE
        costDict.Add("level_1_rangeCost", 100);
        costDict.Add("level_2_rangeCost", 300);
        costDict.Add("level_3_rangeCost", 600);
        costDict.Add("level_4_rangeCost", 1500);
        costDict.Add("level_5_rangeCost", 3000);

        valueDict.Add("level_1_newRange_Fire", 1);
        valueDict.Add("level_2_newRange_Fire", 2);
        valueDict.Add("level_3_newRange_Fire", 3);
        valueDict.Add("level_4_newRange_Fire", 4);
        valueDict.Add("level_5_newRange_Fire", 5);

        // SLOW PERCENT
        costDict.Add("level_1_slowPercentCost", 2000);
        costDict.Add("level_2_slowPercentCost", 5000);
        costDict.Add("level_3_slowPercentCost", 10000);

        valueDict.Add("level_1_newSlowPercent_Water", 30);
        valueDict.Add("level_2_newSlowPercent_Water", 40);
        valueDict.Add("level_3_newSlowPercent_Water", 50);

        // SLOW DURATION
        costDict.Add("level_1_slowDurCost", 1000);
        costDict.Add("level_2_slowDurCost", 4000);
        costDict.Add("level_3_slowDurCost", 8000);

        valueDict.Add("level_1_newSlowDur_Water", 0.75f);
        valueDict.Add("level_2_newSlowDur_Water", 1f);
        valueDict.Add("level_3_newSlowDur_Water", 1.25f);
    }

    public int getCost(string name)
    {
        return costDict[name];
    }

    public float getValue(string name)
    {
        return valueDict[name];
    }
}
