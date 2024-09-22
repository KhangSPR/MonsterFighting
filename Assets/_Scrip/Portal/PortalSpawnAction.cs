using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class PortalSpawnAction : PortalSpawnManagerAbstract
{
    public Portals[] PortalSpawns;
    //public Portals[] PortalSpawns => portalSpawns;

    public void SpawnAction()
    {
        if (PortalSpawns.Length > 0)
        {
            // Lấy portal đầu tiên và spawn
            Portals portal = this.PortalSpawns[0];
            Transform transform = portalSpawnManagerCtrl.SpawnPoints.GetRandomIsEmpty();

            // Spawn portalPrefab dựa trên độ hiếm (rarityPortal)
            Transform portalPrefab = PortalSpawner.Instance.Spawn(LevelPortal(portal.rarityPortal), transform.position, Quaternion.identity);

            PortalCtrl portalCtrl = portalPrefab.gameObject.GetComponent<PortalCtrl>();

            if (portalCtrl.Abilities.AbilitySummon is AbilitySummonPortal abilitySummonPortal)
            {
                abilitySummonPortal.Portal = portal;
            }

            portalSpawnManagerCtrl.AbilitySummons.Add(portalCtrl.Abilities.AbilitySummon);

            portalPrefab.gameObject.SetActive(true);

            // Đánh dấu điểm spawn đã được sử dụng
            transform.GetComponentInChildren<TileSpawn>().IsEmpty = true;

            // Loại bỏ portal đã spawn khỏi danh sách
            RemovePortalFromSpawns();
        }
    }

    void RemovePortalFromSpawns()
    {
        if (PortalSpawns.Length > 0)
        {
            // Chuyển mảng sang List để thao tác dễ dàng
            List<Portals> tempList = new List<Portals>(PortalSpawns);

            // Loại bỏ portal đầu tiên đã spawn
            tempList.RemoveAt(0);

            // Cập nhật lại danh sách PortalSpawns
            PortalSpawns = tempList.ToArray();
        }
    }


    string LevelPortal(RarityPortal rarityPortal)
    {
        switch (rarityPortal)
        {
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
