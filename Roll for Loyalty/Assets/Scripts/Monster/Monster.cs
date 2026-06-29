using System;
using UnityEngine;

public class Monster
{
    #region Variables
    private MonsterDanger dangerLevel;
    private int attackPower;
    private Sprite monster;

    //Getters
    public Sprite MonsterSprite { get { return monster; } }
    public int AttackPower { get { return attackPower; } }
    public int Reward 
    { 
        get
        {
            switch (dangerLevel)
            {
                case MonsterDanger.Easy:
                    return 1;
                case MonsterDanger.Meduim:
                    return 2;
                case MonsterDanger.Hard:
                    return 3;
                case MonsterDanger.Deadly:
                    return 4;
            }
            return 0; 
        }
    }
    #endregion
    #region Constructor
    public Monster(int enemyAttackPower)
    {
        RandomDangerLevel();
        CalculateAttackPower(enemyAttackPower);
        monster = Resources.Load<Sprite>("Monster");
    }
    #endregion
    #region Functions
    //Sets the monster difficulti level
    private void RandomDangerLevel()
    {
        Array values = Enum.GetValues(typeof(MonsterDanger));
        dangerLevel = (MonsterDanger)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    private void CalculateAttackPower(int enemyAttackPower)
    {
        int middle = 0;
        switch (dangerLevel)
        {
            case MonsterDanger.Easy:
                middle = enemyAttackPower - 10;
                if (middle < 10)
                {
                    middle = 10;
                }
                break;
            case MonsterDanger.Meduim:
                middle = enemyAttackPower - 5;
                if (middle < 10)
                {
                    middle = 10;
                }
                break;
            case MonsterDanger.Hard:
                middle = enemyAttackPower + 5;
                break;
            case MonsterDanger.Deadly:
                middle = enemyAttackPower + 10;
                break;
        }
        attackPower = UnityEngine.Random.Range(middle - 5, middle + 5);
    }
    #endregion
}
