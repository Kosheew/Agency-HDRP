using System.Collections;
using System.Collections.Generic;
using Characters.Enemy;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;
    private List<AlertController> enemies = new List<AlertController>();

    void Awake() {
        if (instance == null) instance = this;
    }

    public void RegisterEnemy(AlertController enemy) {
        enemies.Add(enemy);
    }

    public void RaiseAlert(Vector3 location) {
        foreach (AlertController enemy in enemies) {
            enemy.IncreaseAlert(location, 0.5f);
        }
    }
}
