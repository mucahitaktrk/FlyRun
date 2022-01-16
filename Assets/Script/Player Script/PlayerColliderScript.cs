using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderScript : MonoBehaviour
{
    public bool flyPlus = false;
    public bool flyMinus = false;
    public bool flyMultiple = false;
    public bool flyDivision = false;
    public bool col = false;
    public bool finish = false;
    public string value = null;

    // Layer 6 = Artı
    // Layer 7 = Eksi
    // Layer 8 = Çarpı
    // Layer 9 = Bölü

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            value = other.gameObject.name;
            flyPlus = true;
        }
        else if (other.gameObject.layer == 7)
        {
            value = other.gameObject.name;
            flyMinus = true;
        }
        else if (other.gameObject.layer == 8)
        {
            value = other.gameObject.name;
            flyMultiple = true;
        }
        else if (other.gameObject.layer == 9)
        {
            value = other.gameObject.name;
            flyDivision = true;
        }
        else if (other.gameObject.layer == 11)
        {
            value = gameObject.name;
            col = true;
        }
        else if (other.gameObject.layer == 12)
        {
            finish = true; 
        }
    }
}
