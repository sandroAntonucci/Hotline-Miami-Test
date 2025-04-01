using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    bool debounce = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) && debounce == false)
        {
            debounce = true;
            RaycastHit hit;
            bool success = Physics.Raycast(transform.position, transform.forward, out hit);
            if (success)
            {
                AIHandler AIHandler = hit.collider.GetComponent<AIHandler>();
                if (AIHandler)
                {
                    AIHandler.DealDamage(5, "damage_sim");
                    Debug.Log("Damage D:");
                }
            }
        } else if (Input.GetKeyUp(KeyCode.Y) && debounce == true)
        {
            debounce = false;
        }

    }
}
