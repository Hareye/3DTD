using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDict : MonoBehaviour
{
    [Header("Attacks")]
    [SerializeField] private GameObject flameAttackFX;
    [SerializeField] private GameObject chillAttackFX;
    [SerializeField] private GameObject blastAttackFX;
    [SerializeField] private GameObject zapAttackFX;

    [Header("Heals")]
    [SerializeField] private GameObject grassHealFX;

    Dictionary<string, GameObject> attackDict = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> healDict = new Dictionary<string, GameObject>();

    private void Start()
    {
        attackDict.Add("Water Tower", chillAttackFX);
        attackDict.Add("Fire Tower", flameAttackFX);
        attackDict.Add("Lightning Tower", zapAttackFX);
        attackDict.Add("Earth Tower", blastAttackFX);
        attackDict.Add("Grass Tower", null);

        healDict.Add("Grass Tower", grassHealFX);
    }

    public string getAttackName(string towerName)
    {
        switch (towerName)
        {
            case "Fire Tower":
                return "Flame Attack";
            case "Water Tower":
                return "Chill";
            case "Lightning Tower":
                return "Zap";
            case "Earth Tower":
                return "Blast";
            default:
                return null;
        }
    }

    public GameObject getAttackFX(string towerName)
    {
        return attackDict[towerName];
    }

    public GameObject getHealFX(string towerName)
    {
        return healDict[towerName];
    }
}
