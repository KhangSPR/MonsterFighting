public class SkillSpawner : Spawner
{
    private static SkillSpawner instance;
    public static SkillSpawner Instance { get => instance; }

    const string Stone = "Stone"; // Item 1
    const string Poison = "Poition"; // Item 1
    const string Electric = "Electric"; // Item 1
    const string Fire = "Meteorite"; // Item 1
    const string Glace = "GlaceRain"; // Item 1
    const string Heal = "FlareHeal"; // Item 1
    const string Power = "FlarePower"; // Item 1

    protected override void Awake()
    {
        base.Awake();
        SkillSpawner.instance = this;
    }
    public string GetSkillType(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Stone:
                return Stone;
            case SkillType.Poison:
                return Poison;
            case SkillType.Electric:
                return Electric;
            case SkillType.Fire:
                return Fire;
            case SkillType.Glace:
                return Glace;
        }
        return "";
    }
    public string GetMedicineType(Medicine medicineType)
    {
        switch (medicineType)
        {
            case Medicine.Heal:
                return Heal;
            case Medicine.Power:
                return Power;
        }
        return "";
    }
}
