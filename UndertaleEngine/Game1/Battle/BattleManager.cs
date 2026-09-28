using System;
using System.Runtime.CompilerServices;

public enum  BattleState
{
    Player_SelectCommand,
    Player_Action,
    Enemy_PreAttack,
    Enemy_Attack,
    ReturnToPlayer 
}

public enum CommandType
{
    Fight,
    Act,
    Item,
    Mercy
}

public class BattleManager
{
    public BattleState CurrentState { get; private set; } = BattleState.Player_SelectCommand;
    public BattleManager()
    {
        EnterState(CurrentState);
    }
    public void Update(float dt)
    {
        switch(CurrentState)
        {
            case BattleState.Player_SelectCommand: Update_PlayerSelect(dt);
                break;
            case BattleState.Player_Action: Update_PlayerAction(dt);
                break;
            case BattleState.Enemy_PreAttack: Update_EnemyPreAttack(dt);
                break;
            case BattleState.Enemy_Attack: Update_EnemyAttack(dt);
                break;
            case BattleState.ReturnToPlayer: Update_ReturnToPlayer(dt);
                break;
        }
    }

    public void ChangeState(BattleState newState)
    {
        CurrentState = newState;
        EnterState(newState);
    }

    private void EnterState(BattleState state)
    {
        switch (state)
        {
            case BattleState.Player_SelectCommand:

                break;
            case BattleState.Player_Action:

                break;
            case BattleState.Enemy_PreAttack:

                break;
            case BattleState.Enemy_Attack:

                break;
            case BattleState.ReturnToPlayer:

                break;
        }
    }
    
    public void ChooseCommand(CommandType type)
    {
        if (CurrentState != BattleState.Player_SelectCommand)
            return;
        queuedCommand = type;
        ChangeState(BattleState.Player_Action);
    }
    private CommandType queuedCommand;
    private void Update_PlayerSelect(float dt)
    {

    }
    private float fakeActionTimer = 0f;

    private void Update_PlayerAction(float dt)
    {
        fakeActionTimer += dt;
        if (fakeActionTimer >= 1f)
        {
            fakeActionTimer = 0f;
            ChangeState(BattleState.Enemy_PreAttack);
        }
    }
    private float enemyPreAttackTimer = 0;
    private void Update_EnemyPreAttack(float dt)
    {
        enemyPreAttackTimer += dt;
        if (enemyPreAttackTimer >= 0.5f)
        {
            enemyPreAttackTimer = 0;
            ChangeState(BattleState.Enemy_Attack);
        }
    }
    private float enemyAttackTimer = 0;
    private void Update_EnemyAttack(float dt)
    {
        enemyAttackTimer += dt;
        if(enemyAttackTimer >= 2f)
        {
            enemyAttackTimer = 0;
            ChangeState(BattleState.ReturnToPlayer);
        }
    }
    private float returnTimer = 0;
    private void Update_ReturnToPlayer(float dt)
    {
        returnTimer += dt;
        if (returnTimer >= dt)
        {
            returnTimer = 0;
            ChangeState(BattleState.Player_SelectCommand);
        }
    }
}
