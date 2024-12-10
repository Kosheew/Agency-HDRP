using System;
using UnityEngine;

public class ShaderCollider : MonoBehaviour
{
    public Material targetMaterial; // Матеріал, який реагує
    public float effectRadius = 2f; // Радіус зони впливу

    private void Start()
    {
        targetMaterial = GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 triggerPosition = other.transform.position;

        // Передаємо позицію тригера і радіус у шейдер
        targetMaterial.SetVector("_TriggerPosition", triggerPosition);
        targetMaterial.SetFloat("_EffectRadius", effectRadius);
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 triggerPosition = other.transform.position;

        // Оновлюємо позицію тригера, якщо об'єкт переміщується
        targetMaterial.SetVector("_TriggerPosition", triggerPosition);
    }

    private void OnTriggerExit(Collider other)
    {
        // Скидаємо ефект після виходу
        targetMaterial.SetFloat("_EffectRadius", 0f);
    }
}