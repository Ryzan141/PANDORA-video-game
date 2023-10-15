using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    int hp;
    [SerializeField]
    int damage;

    int currentHp;
    public Action NewInfo;
    public Action Died;
    public bool dead = false;
    GameObject sceneManager;
    private void Awake()
    {
        currentHp = hp;
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
    }
    public void TakeDmg(int dmg)
    {
        currentHp -= dmg;
        if(currentHp < 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Died.Invoke();
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other is CapsuleCollider)
            return;
        Debug.Log("enemy now nearby");
        if (other.gameObject.CompareTag("Player"))
        {
            sceneManager.GetComponent<Game_Manager>().nearbyEnemies.Add(this);
            Debug.Log("added");
        }        
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("enemy now faraway");
        if (other.gameObject.CompareTag("Player"))
        {
            sceneManager.GetComponent<Game_Manager>().nearbyEnemies.Remove(this);
            Debug.Log("removed");
        }        
    }
}
