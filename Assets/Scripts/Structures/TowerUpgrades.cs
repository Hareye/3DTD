using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrades : MonoBehaviour
{
    Dictionary<string, int> costDict = new Dictionary<string, int>();
    Dictionary<string, float> valueDict = new Dictionary<string, float>();

    private void Start()
    {
        // HP
        // Earth (?)
        costDict.Add("level 1 hpCost", 100);
        costDict.Add("level_2_hpCost", 100);
        costDict.Add("level_3_hpCost", 100);
        costDict.Add("level_4_hpCost", 100);
        costDict.Add("level_5_hpCost", 100);

        valueDict.Add("level_1_newHP_Earth", 100);
        valueDict.Add("level_2_newHP_Earth", 100);
        valueDict.Add("level_3_newHP_Earth", 100);
        valueDict.Add("level_4_newHP_Earth", 100);
        valueDict.Add("level_5_newHP_Earth", 100);

        // DAMAGE
        // Fire | Lightning | Dark | Light
        costDict.Add("level_1_dmgCost", 400);
        costDict.Add("level_2_dmgCost", 800);
        costDict.Add("level_3_dmgCost", 1500);
        costDict.Add("level_4_dmgCost", 3000);
        costDict.Add("level_5_dmgCost", 6000);

        valueDict.Add("level_1_newDmg_Fire", 65);
        valueDict.Add("level_2_newDmg_Fire", 80);
        valueDict.Add("level_3_newDmg_Fire", 95);
        valueDict.Add("level_4_newDmg_Fire", 120);
        valueDict.Add("level_5_newDmg_Fire", 150);

        valueDict.Add("level_1_newDmg_Lightning", 30);
        valueDict.Add("level_2_newDmg_Lightning", 35);
        valueDict.Add("level_3_newDmg_Lightning", 40);
        valueDict.Add("level_4_newDmg_Lightning", 50);
        valueDict.Add("level_5_newDmg_Lightning", 60);

        valueDict.Add("level_1_newDmg_Dark", 25);
        valueDict.Add("level_2_newDmg_Dark", 30);
        valueDict.Add("level_3_newDmg_Dark", 35);
        valueDict.Add("level_4_newDmg_Dark", 45);
        valueDict.Add("level_5_newDmg_Dark", 60);

        valueDict.Add("level_1_newDmg_Light", 25);
        valueDict.Add("level_2_newDmg_Light", 30);
        valueDict.Add("level_3_newDmg_Light", 35);
        valueDict.Add("level_4_newDmg_Light", 45);
        valueDict.Add("level_5_newDmg_Light", 60);

        // FIRE RATE
        // Fire | Lightning | Dark | Light
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

        valueDict.Add("level_1_newFr_Lightning", 1.6f);
        valueDict.Add("level_2_newFr_Lightning", 1.7f);
        valueDict.Add("level_3_newFr_Lightning", 1.8f);
        valueDict.Add("level_4_newFr_Lightning", 1.9f);
        valueDict.Add("level_5_newFr_Lightning", 2f);

        valueDict.Add("level_1_newFr_Dark", 1.1f);
        valueDict.Add("level_2_newFr_Dark", 1.2f);
        valueDict.Add("level_3_newFr_Dark", 1.3f);
        valueDict.Add("level_4_newFr_Dark", 1.4f);
        valueDict.Add("level_5_newFr_Dark", 1.5f);

        valueDict.Add("level_1_newFr_Light", 1.1f);
        valueDict.Add("level_2_newFr_Light", 1.2f);
        valueDict.Add("level_3_newFr_Light", 1.3f);
        valueDict.Add("level_4_newFr_Light", 1.4f);
        valueDict.Add("level_5_newFr_Light", 1.5f);

        // RANGE
        // Fire | Water | Grass | Dark | Light
        costDict.Add("level_1_rangeCost", 300);
        costDict.Add("level_2_rangeCost", 700);
        costDict.Add("level_3_rangeCost", 1300);
        costDict.Add("level_4_rangeCost", 2100);
        costDict.Add("level_5_rangeCost", 3100);

        valueDict.Add("level_1_newRange_Fire", 0.75f);
        valueDict.Add("level_2_newRange_Fire", 0.8f);
        valueDict.Add("level_3_newRange_Fire", 0.85f);
        valueDict.Add("level_4_newRange_Fire", 0.9f);
        valueDict.Add("level_5_newRange_Fire", 1f);

        valueDict.Add("level_1_newRange_Water", 0.75f);
        valueDict.Add("level_2_newRange_Water", 0.8f);
        valueDict.Add("level_3_newRange_Water", 0.85f);
        valueDict.Add("level_4_newRange_Water", 0.9f);
        valueDict.Add("level_5_newRange_Water", 1f);

        valueDict.Add("level_1_newRange_Grass", 0.75f);
        valueDict.Add("level_2_newRange_Grass", 0.8f);
        valueDict.Add("level_3_newRange_Grass", 0.85f);
        valueDict.Add("level_4_newRange_Grass", 0.9f);
        valueDict.Add("level_5_newRange_Grass", 1f);

        valueDict.Add("level_1_newRange_Dark", 0.95f);
        valueDict.Add("level_2_newRange_Dark", 1f);
        valueDict.Add("level_3_newRange_Dark", 1.05f);
        valueDict.Add("level_4_newRange_Dark", 1.15f);
        valueDict.Add("level_5_newRange_Dark", 1.25f);

        valueDict.Add("level_1_newRange_Light", 0.95f);
        valueDict.Add("level_2_newRange_Light", 1f);
        valueDict.Add("level_3_newRange_Light", 1.05f);
        valueDict.Add("level_4_newRange_Light", 1.15f);
        valueDict.Add("level_5_newRange_Light", 1.25f);

        // HEAL AMOUNT
        // Grass
        costDict.Add("level_1_healCost", 500);
        costDict.Add("level_2_healCost", 1000);
        costDict.Add("level_3_healCost", 4000);
        costDict.Add("level_4_healCost", 8000);
        costDict.Add("level_5_healCost", 15000);

        valueDict.Add("level_1_newHeal_Grass", 30);
        valueDict.Add("level_2_newHeal_Grass", 40);
        valueDict.Add("level_3_newHeal_Grass", 70);
        valueDict.Add("level_4_newHeal_Grass", 100);
        valueDict.Add("level_5_newHeal_Grass", 150);

        // HEAL RATE
        // Grass
        costDict.Add("level_1_healRateCost", 1000);
        costDict.Add("level_2_healRateCost", 3000);
        costDict.Add("level_3_healRateCost", 5000);
        costDict.Add("level_4_healRateCost", 7000);
        costDict.Add("level_5_healRateCost", 9000);

        valueDict.Add("level_1_newHealRate_Grass", 0.3f);
        valueDict.Add("level_2_newHealRate_Grass", 0.35f);
        valueDict.Add("level_3_newHealRate_Grass", 0.4f);
        valueDict.Add("level_4_newHealRate_Grass", 0.45f);
        valueDict.Add("level_5_newHealRate_Grass", 0.5f);

        // SLOW PERCENT
        costDict.Add("level_1_slowPercentCost", 2000);
        costDict.Add("level_2_slowPercentCost", 6000);
        costDict.Add("level_3_slowPercentCost", 12000);

        // Water
        valueDict.Add("level_1_newSlowPercent_Water", 25);
        valueDict.Add("level_2_newSlowPercent_Water", 35);
        valueDict.Add("level_3_newSlowPercent_Water", 50);

        // SLOW DURATION
        costDict.Add("level_1_slowDurCost", 1000);
        costDict.Add("level_2_slowDurCost", 4000);
        costDict.Add("level_3_slowDurCost", 8000);

        // Water
        valueDict.Add("level_1_newSlowDur_Water", 0.75f);
        valueDict.Add("level_2_newSlowDur_Water", 1f);
        valueDict.Add("level_3_newSlowDur_Water", 1.25f);

        // SPECIAL
        costDict.Add("level_1_specialCost", 8000);
        costDict.Add("level_2_specialCost", 15000);
        costDict.Add("level_3_specialCost", 25000);

        // Fire | Water | Grass | Lightning | Dark | Light
        valueDict.Add("level_1_newSpecial_Fire", 0.75f);    // I have set none of the special values properly yet because idk what to set them as xd
        valueDict.Add("level_2_newSpecial_Fire", 1f);
        valueDict.Add("level_3_newSpecial_Fire", 1.25f);

        valueDict.Add("level_1_newSpecial_Water", 0.75f);
        valueDict.Add("level_2_newSpecial_Water", 1f);
        valueDict.Add("level_3_newSpecial_Water", 1.25f);

        valueDict.Add("level_1_newSpecial_Grass", 0.75f);
        valueDict.Add("level_2_newSpecial_Grass", 1f);
        valueDict.Add("level_3_newSpecial_Grass", 1.25f);

        valueDict.Add("level_1_newSpecial_Lightning", 0.75f);
        valueDict.Add("level_2_newSpecial_Lightning", 1f);
        valueDict.Add("level_3_newSpecial_Lightning", 1.25f);

        valueDict.Add("level_1_newSpecial_Dark", 0.75f);
        valueDict.Add("level_2_newSpecial_Dark", 1f);
        valueDict.Add("level_3_newSpecial_Dark", 1.25f);

        valueDict.Add("level_1_newSpecial_Light", 0.75f);
        valueDict.Add("level_2_newSpecial_Light", 1f);
        valueDict.Add("level_3_newSpecial_Light", 1.25f);
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
