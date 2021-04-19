using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private Light sceneLight;

    public void ActivateExplosion()
    {
        explosionEffect.Play();
    }

    public void SwitchOffLight()
    {
        sceneLight.intensity = 0;
    }

    public void SwitchOnLight()
    {
        sceneLight.intensity = 1.5f;
    }
}
