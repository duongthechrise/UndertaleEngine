using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static UndertaleEngine.Game1.PlayerTurn1;

namespace UndertaleEngine.Game1
{
    public class BattleSystem
    {
        private ITurn_battle1 currentTurn;
        private Queue<ITurn_battle1> turnQueue = new Queue<ITurn_battle1>();
        private BattleScene battle;
        public BattleSystem(BattleScene battle)
        {
            this.battle = battle;
        }
        public void EnqueueTurn(ITurn_battle1 turn)
        {
            turnQueue.Enqueue(turn);
        }
        public void StartNextTurn()
        {
            if (turnQueue.Count == 0)
            {
                currentTurn = null;
                return;
            }
            currentTurn = turnQueue.Dequeue();
            currentTurn.Start(battle);
        }
        public void Update(GameTime gameTime)
        {
            if(currentTurn == null)
            {
                StartNextTurn();
                return;
            }
            currentTurn.Update(gameTime);
            if(currentTurn.IsFinished)
            {
                StartNextTurn();
            }
        }
        public void EnqueueTotalTurn1()
        {
            turnQueue.Enqueue(new PlayerTurn1());
            turnQueue.Enqueue(new Enemyturn1());
        }
        public void EnqueueTotalTurn2()
        {
            turnQueue.Enqueue(new PlayerTurn2());
            turnQueue.Enqueue(new Enemyturn2());
        }
    }
}

