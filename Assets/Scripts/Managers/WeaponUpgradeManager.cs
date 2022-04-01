using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeManager : MonoBehaviour
{
    // Maximum upgrade.
    public static int maxUpgrade = 0;

    // DiagonalArrowUpgrade element.
    public static int diagonalArrowUpgrade = 0;
    Text diagonalArrowLevel;
    Button diagonalArrowButton;

    // AttackSpeedUpgrade.
    public static int aspdUpgrade = 0;
    Text aspdLevel;
    Text upgradeText;
    Button aspdButton;

    // Player shooting.
    public PlayerShooting playerShooting;

    // Start is called before the first frame update
    void Start()
    {
        // Get component reference.
        diagonalArrowLevel = GameObject.Find("DiagonalArrowLevel").GetComponent<Text>();
        diagonalArrowButton = GameObject.Find("DiagonaArrowButton").GetComponent<Button>();
        aspdLevel = GameObject.Find("AspdLevel").GetComponent<Text>();
        aspdButton = GameObject.Find("AspdButton").GetComponent<Button>();
        upgradeText = GameObject.Find("UpgradeText").GetComponent<Text>();

        // Get player shooting.
        // playerShooting = GameObject.Find("Player").GetComponentInChildren<PlayerShooting>();
    }

    // Update is called once per frame
    void Update()
    {
        // If number of upgrade is less than maximum upgrade, enable the button and turn the color
        // into green.
        if (maxUpgrade > diagonalArrowUpgrade + aspdUpgrade) {
            diagonalArrowButton.interactable = true;
            aspdButton.interactable = true;
            upgradeText.text = "Upgrade is Available!!!";

            // The green color rgb is 103, 200, 28.
            diagonalArrowButton.GetComponentInChildren<Text>().color = new Color32(103, 200, 28, 255);
            aspdButton.GetComponentInChildren<Text>().color = new Color32(103, 200, 28, 255);
        }
        else 
        {
            // Else, disable the button and turn the button text into gray.
            diagonalArrowButton.interactable = false;
            aspdButton.interactable = false;
            upgradeText.text = "";

            diagonalArrowButton.GetComponentInChildren<Text>().color = 
                new Color32(211, 211, 211, 255);
            aspdButton.GetComponentInChildren<Text>().color = 
                new Color32(211, 211, 211, 255);
        }

        // Get input command from player. Q for diagonal arrow updgrade, E for attack speed upgrade.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            upgradeDiagonalArrow();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            upgradeAspd();
        }
    }

    // Function to upgrade diagonal arrow.
    public void upgradeDiagonalArrow()
    {
        // If the upgrade is less than maximum upgrade, upgrade the upgrade.
        if (diagonalArrowUpgrade + aspdUpgrade < maxUpgrade)
        {
            diagonalArrowUpgrade++;
            diagonalArrowLevel.text = "Lv " + diagonalArrowUpgrade;
            playerShooting.diagonalUpgrade++;
        }
    }

    // Function to upgrade attack speed.
    public void upgradeAspd()
    {
        // If the upgrade is less than maximum upgrade, upgrade the upgrade.
        if (diagonalArrowUpgrade + aspdUpgrade < maxUpgrade)
        {
            aspdUpgrade++;
            aspdLevel.text = "Lv " + aspdUpgrade;
            playerShooting.aspdUpgrade++;
        }
    }

}
