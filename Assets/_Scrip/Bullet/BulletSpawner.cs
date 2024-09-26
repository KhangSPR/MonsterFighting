using UnityEngine;

public class BulletSpawner : Spawner
{
    private static BulletSpawner instance;
    public static BulletSpawner Instance { get => instance; }
    public static string bulletOne = "Bullet_1"; // bullet 1
    public static string bulletTwo = "Bullet_2"; // bullet 2
    public static string bulletThree = "Bullet_3"; // bullet 2
    public static string bulletFour = "Bullet_4"; // bullet 2
    public static string bulletFive = "Bullet_5"; // bullet 2
    public static string bulletSix = "Bullet_6"; // bullet 2
    public static string bulletSeven = "Bullet_7"; // bullet 2
    public static string bulletEight = "Bullet_8"; // bullet 2
    public static string bulletNine = "Bullet_9";
    public static string bulletTen = "Bullet_10";
    public static string Bullet_DarkSorceress = "Bullet_DarkSorceress";

    protected override void Awake()
    {
        base.Awake();

        BulletSpawner.instance = this;

    }

}
