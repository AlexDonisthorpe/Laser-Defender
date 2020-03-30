using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    TMP_Text healthText;
    Player player;
    // Update is called once per frame

    private void Start()
    {
        healthText = GetComponent<TMP_Text>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
         healthText.text = player.GetHealth().ToString();       
    }
}
