using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ActionGameCharacterBase character = other.gameObject.GetComponent<ActionGameCharacterBase>();
        if (character == null) return;
        character.CalHp(-99999999);
    }
}
