using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Projectile : SaiMonoBehaviour
{
    public GameObject defense;
    public GameObject target;

    public float speed = 10f;

    private float defenseX;
    private float targetX;

    private float dist;
    private float nextX;
    private float baseY;
    private float height;

    protected override void Start()
    {
        base.Start();
        defense = GameObject.FindGameObjectWithTag("");
        target = GameObject.FindGameObjectWithTag("");
    }
    protected override void Update()
    {
        base.Update();
        defenseX = defense.transform.position.x;
        targetX = target.transform.position.x;

        dist = targetX - defenseX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(defense.transform.position.y, target.transform.position.y, (nextX - defenseX) / dist);
        height = 2 * (nextX - defenseX) * (nextX - targetX) / (-0.25f * dist * dist);

        Vector3 movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LootAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        //if(transform.position == target.transform.position)
        //{
        //    Destroy(gameObject);
        //}
    }
    public static Quaternion LootAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0,0,Mathf.Atan2(rotation.y, rotation.x)*Mathf.Rad2Deg);
    }
}
