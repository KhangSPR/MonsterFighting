using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] EnergyManager energy;
    [SerializeField] TMPro.TextMeshProUGUI ui;

    // Update is called once per frame
    void Update()
    {

        // Consume Energy Test
        if (Input.GetKeyDown(KeyCode.Space)){
            energy.ConsumeEnergy();
        }

        // UI Test
        ui.text = string.Format("energy : {0}, timer : {1}", energy.currentEnergyAmount, energy.GetEnergyTimer());

    }
}
