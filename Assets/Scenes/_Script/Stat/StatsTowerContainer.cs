namespace UIGameDataManager
{
    public class StatsTowerContainer
    {
        public Stat basePointsAttack;
        public Stat basePointsCriticalHit;
        public Stat basePointsLife;
        public Stat basePointsDefense;
        public Stat basePointsAttackSpeed;
        public Stat basePointsSpecialAttack;

        public StatsTowerContainer(CardCharacter CardTower)
        {
            basePointsAttack = new Stat(StatKeys.Attack, CardTower.basePointsAttack);
            //basePointsCriticalHit = new Stat(StatKeys.CriticalHit, CardTower.basePointsCriticalHit);
            basePointsLife = new Stat(StatKeys.Life, CardTower.basePointsLife);
            //basePointsDefense = new Stat(StatKeys.Defense, CardTower.basePointsDefense);
            basePointsAttackSpeed = new Stat(StatKeys.AttackSpeed, CardTower.basePointsAttackSpeed);
            basePointsSpecialAttack = new Stat(StatKeys.SpecialAttack, CardTower.basePointsSpecialAttack);
        }

        //public Stat getStat(StatKeys statKey)
        //{
        //    var fields = typeof(StatsTowerContainer).GetFields();

        //    foreach (var item in fields)
        //    {
        //        Stat value = (Stat)item.GetValue(this);

        //        if (value.statKey == statKey)
        //            return value;
        //    }

        //    return null;
        //}
    }
}
