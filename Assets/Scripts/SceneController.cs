using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffectGO;
    [SerializeField] private GameObject lightGO;

    public void ActivateExplosion()
    {
        explosionEffectGO.SetActive(true);
    }

    public void SwitchOffLight()
    {
        lightGO.SetActive(false);
    }

    public void SwitchOnLight()
    {
        lightGO.SetActive(true);
    }
}
