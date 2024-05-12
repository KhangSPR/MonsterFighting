using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class PortalSpawnAction : PortalSpawnManagerAbstract
{
    Portals[] PortalSpawns;

    protected override void Start()
    {
        base.Start();
        //Set Portals
        this.PortalSpawns = this.portalSpawnManagerCtrl.portalsSpawning.ToArray();
    }

    public void SpawnAction()
    {
        // Get the first portal from the PortalSpawns array
        Portals portal = this.PortalSpawns[0];

        //Spawn
        Transform transform = portalSpawnManagerCtrl.SpawnPoints.GetRandomIsEmpty();

        Transform minionPrefab = PortalSpawner.Instance.Spawn(LevelPortal(portal.rarityPortal), transform.position, Quaternion.identity);

        //Setting
        minionPrefab.gameObject.GetComponent<PortalCtrl>().Abilities.AbilitySummonEnemy.Portal = portal;

        minionPrefab.gameObject.SetActive(true);

        //Empty True
        transform.GetComponentInChildren<TileSpawn>().IsEmpty = true;

        // Delete the spawned portal from the PortalSpawns array
        RemovePortalFromSpawns();
    }
    void RemovePortalFromSpawns()
    {
        if (PortalSpawns.Length > 0)
        {
            List<Portals> tempList = new List<Portals>(PortalSpawns);
            tempList.RemoveAt(0);
            PortalSpawns = tempList.ToArray();
        }
    }
    string LevelPortal(RarityPortal rarityPortal)
    {
        switch (rarityPortal)
        {
            //Transform là Enemy
            case RarityPortal.Common:
                return PortalSpawner.PortalOne;
            case RarityPortal.Rare:
                return PortalSpawner.PortalTwo;
            case RarityPortal.Epic:
                return PortalSpawner.PortalOne;
            case RarityPortal.Legendary:
                return PortalSpawner.PortalOne;
        }
        return PortalSpawner.PortalOne;

    }

}
