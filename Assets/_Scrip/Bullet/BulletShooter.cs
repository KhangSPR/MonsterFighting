using UnityEngine;

public abstract class BulletShooter : AbstractCtrl
{
    [Header("Bullet Shooter")]
    public bool isShooting = false;
    [SerializeField] protected Transform transformParent;
    public Transform TransformParent => transformParent;

    [SerializeField]
    protected Transform gunPoint;
    public Transform GunPoint => gunPoint;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTransFormParentObject();
        this.LoadGunPoint();
    }
    protected override void Update()
    {
        base.Update();
        this.isShooting = IsShooting();
        //if (this.isShooting) return;
    }
    protected Transform LoadTransFormParentObject()
    {
        if (this.transformParent != null) return this.transformParent;
        this.transformParent = transform.parent;
        Debug.Log(gameObject.transform.parent.name + ": LoadTransFormParentObject" + gameObject);
        return this.transformParent;
    }
    protected virtual void LoadGunPoint()
    {
        if (this.gunPoint != null) return;
        this.gunPoint = transform.Find("GunPoint");

        Debug.Log(gameObject.transform.parent.name + ": LoadGunPoint" + gameObject);

    }
    public void Shoot()
    {
        Vector3 shootingDirection = GetShootingDirection();
        Quaternion rotation;

        // Find the "Bow" object within the parent "Model" object
        Transform BulletFollowTargetTransform = transformParent.Find("Modle/LookAtTarget");

        // Check if the "Bow" object is found
        if (BulletFollowTargetTransform != null)
        {
            rotation = BulletFollowTargetTransform.rotation;
        }
        else
        {
            // If "Bow" object is not found, use the parent's rotation
            rotation = transformParent.rotation;
        }

        Transform newBullet = BulletSpawner.Instance.Spawn(GetBulletType(), gunPoint.position, rotation);

        Debug.Log(newBullet);

        //if (newBullet == null) return;
        newBullet.gameObject.SetActive(true);

        // Attach BulletCtrl component to the new bullet if not already attached
        BulletRegularCtrl bulletCtrl = newBullet.GetComponent<BulletRegularCtrl>();

        //if (bulletCtrl == null)
        //{
        //    bulletCtrl = newBullet.gameObject.AddComponent<BulletRegularCtrl>();
        //}

        if (transformParent.tag == "Enemy")
        {
            if (bulletCtrl.ObjLookAtTargetSetter != null)
            {
                bulletCtrl.ObjLookAtTargetSetter.target = this.enemyCtrl.EnemyAttack.GetTransFromFirstAttack();
            }
            bulletCtrl.DamageSender.Damage = enemyCtrl.EnemySO.basePointsAttack;
            bulletCtrl.ObjectCtrl = enemyCtrl;

        }
        else if (transformParent.tag == "Player")
        {
            if (bulletCtrl.ObjLookAtTargetSetter != null)
            {
                bulletCtrl.ObjLookAtTargetSetter.target = this.PlayerCtrl.PlayerAttack.GetTransFromFirstAttack();

            }
            bulletCtrl.DamageSender.Damage = playerCtrl.CharacterStatsFake.Attack;
            bulletCtrl.ObjectCtrl = playerCtrl;
        }
        else if (transformParent.tag == "Tower")
        {
            bulletCtrl.DamageSender.Damage = 2/*playerCtrl.ShootAbleObjectSO.damage*/;
        }
        bulletCtrl.SetDirection(shootingDirection);
        bulletCtrl.SetShotter(transform.parent);

        if (bulletCtrl.ObjLookAtTargetSetter == null)
        {
            bulletCtrl.SetBullet();
        }

    }
    public void ShootPX()
    {
        Vector3 TakePosition = transform.parent.position;
        // Lấy vị trí của enemy nằm trong khoảng tấn công
        if (transformParent.tag == "Enemy")
        {
            TakePosition = enemyCtrl.EnemyAttack.GetTransFromFirstAttack().position;
        }
        else
        {
            TakePosition = playerCtrl.PlayerAttack.GetTransFromFirstAttack().position;
        }
        // Chuyển đổi vị trí của player về Vector2
        Vector3 DefautPosition = new Vector2(transformParent.position.x, transformParent.position.y);

        if (TakePosition != DefautPosition)
        {
            Transform newBullet = BulletSpawner.Instance.Spawn(GetBulletType(), TakePosition, Quaternion.identity);
            newBullet.gameObject.SetActive(true);

            // Attach BulletCtrl component to the new bullet if not already attached
            BulletCtrl bulletCtrl = newBullet.GetComponent<BulletCtrl>();
            if (bulletCtrl == null)
            {
                bulletCtrl = newBullet.gameObject.AddComponent<BulletCtrl>();
            }
            if (transformParent.tag == "Enemy")
            {
                bulletCtrl.DamageSender.Damage = 2;
            }
            else
            {
                //bulletCtrl.DamageSender.dame = playerCtrl.ShootAbleObjectSO.damage;
            }

            bulletCtrl.SetShotter(transform.parent);
        }
    }
    //gravity
    public void ShootGravity()
    {
        Transform gunPoint = transform.Find("GunPoint");

        // Thực hiện tạo đối tượng đạn
        Transform newBullet = BulletSpawner.Instance.Spawn(GetBulletType(), gunPoint.position, gunPoint.rotation);

        if (newBullet == null) return;
        newBullet.gameObject.SetActive(true);

        newBullet.gameObject.GetComponent<Rigidbody2D>().velocity = transformParent.right * this.deFenSeCtrl.DefenseShooter.LaunchForce; // Gravity object

        BulletCtrl bulletCtrl = newBullet.GetComponent<BulletCtrl>();
        if (bulletCtrl == null)
        {
            bulletCtrl = newBullet.gameObject.AddComponent<BulletCtrl>();
        }

        bulletCtrl.SetShotter(transform.parent);

    }
    protected virtual string GetBulletType()
    {

        string parentName = RemoveCloneFromName(transformParent.name);
        Debug.Log(parentName);

        switch (parentName)
        {
            //Transform là Enemy
            case "Archery Skeleton":
                return BulletSpawner.bulletOne;
            case "Skeleton Magic":
                return BulletSpawner.bulletSix;
            case "Goblin Venomorb":
                return BulletSpawner.Bullet_GoblinVenomorb;
            //Transform là Player
            case "Archer":
                return BulletSpawner.bulletOne;
            case "Magic Fire":
                return BulletSpawner.bulletSix;
            case "Dark Sorceress":
                return BulletSpawner.Bullet_DarkSorceress;
            //Transform là Guard
            case "Guard_1":
                return BulletSpawner.bulletFive;
            case "Guard_2":
                return BulletSpawner.bulletSix;
            case "Guard_3":
                return BulletSpawner.bulletSeven;
            case "Defense_1":
                return BulletSpawner.bulletEight;

        }
        return parentName;
    }
    private string RemoveCloneFromName(string originalName)
    {
        if (originalName.EndsWith("(Clone)"))
        {
            // Nếu tên kết thúc bằng "(Clone)", loại bỏ phần "Clone"
            return originalName.Substring(0, originalName.Length - 7); // Loại bỏ 7 ký tự từ cuối (Clone)
        }
        else
        {
            // Nếu không có phần "(Clone)", trả về tên ban đầu
            return originalName;
        }
    }
    protected abstract Vector3 GetShootingDirection();
    protected abstract bool IsShooting();
}


