using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BattleManager : MonoBehaviour

{
    [SerializeField]

    private List<Fighter> fighters = new List<Fighter>();
    [SerializeField]
    private int requieredFighters = 2;
    [SerializedField]
    private float secondsbetweenAttacks = 1f;
    [SerializeField]
    private float secondsToStartBattle = 1f;
    [SerializeField]
    public UnityEvent onBattleStart;
    [SerializeField]
    public UnityEvent onBattleStop;
    private int currentFighterIndex = 0;
    private bool isBattleActive = false;

    public void AddFighter(Fighter fighter)
    {
        fighters.Add(fighter);
        CheckFighters();
    } 
    public void RemoveFighter(Fighter fighter)
    {
        fighters.Remove(fighter);
        CheckFighters();
    }
    private void CheckFighters()
    {
        if (fighters.Count < requieredFighters)
        {
            StopBattle();
        }
        else
        {
            StartBattle();
        }
    }
    private void StartBattle()
    {
        isBattleActive = true;
        onBattleStart?.Invoke();
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        currentFighterIndex = Random.Range(0, fighters.Count);
        Fighter attacker = fighters[currentFighterIndex];
        Fighter defender;
        do
        {
            currentFighterIndex = Random.Range(0, fighters.Count);
            defender =fighters[currentFighterIndex];
            while (attacker == defender);
        }
        attacker.Attack();
        float damage = attacker.GetDamage();
        defender.GetComponent<Health>().TakeDamage(damage);

        yield return new WaitForSeconds(secondsBetweenAttacks);
        if (defender.GetComponent<Health>().CurrentHealth > 0)
        {
            StartCoroutine(Attack());
        }
        else
        {
            StopBattle();
        }
    
    
    }
    private void StopBattle()
    {
        StopCoroutine(Attack());
        onBattleStop?.Invoke();

    }
        
    
}


