using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Quest Info", menuName = "Quest/Quest Info")]
public class QuestInfoSO : ScriptableObject
{
    [Header("Quest Information")]
    public string nameQuest;
    [TextArea] public string bio;
    public bool isFinishQuest;
    public int expReward; // Số kinh nghiệm nhận được khi hoàn thành nhiệm vụ.
    /*public int goldReward;*/ // Số vàng nhận được khi hoàn thành nhiệm vụ (có thể thêm nếu cần).
    public List<Objective> objectives; // Danh sách mục tiêu
    public UIGameDataMap.Resources[] Reward;

    [System.Serializable]
    public class Objective
    {
        //Loại mục tiêu, -Giết quái vật. - Nói chuyện với NPC.- Thu thập vật phẩm.
        public enum Type { kill, talk, collect, defend }
        public string ID; // ID định danh cho mục tiêu (ví dụ: ID quái vật, NPC, hoặc vật phẩm).
        public int requiredCount; //Số lượng cần thiết để hoàn thành mục tiêu.
        //[System.NonSerialized]
        public int currentCount; //: Số lượng hiện tại đã hoàn thành. NonSerialized giúp không lưu trữ giá trị này khi serialize.
        public string requiredText;
        public Type type; //Loại mục tiêu
        public bool QuestComPlete()
        {
            if(currentCount >= requiredCount)
            {
                return true;
            }
            return false;
        }
        //Kiểm tra xem mục tiêu có hoàn thành hay không khi nhận thông tin về loại và ID.
        public bool UpdateObjectives(Objective.Type type, string id, int count = 1)
        {
            if (this.type == type && this.ID == id)
            {
                currentCount += count;
                if (currentCount >= requiredCount)
                {
                    currentCount = requiredCount;
                    return true; // Mục tiêu đã hoàn thành
                }
            }
            return false; // Mục tiêu chưa hoàn thành
        }

        //        Cách hoạt động:
        //So sánh type và objectiveId hiện tại với giá trị được truyền vào.
        //Nếu khớp, tăng currentAmount lên 1.
        //Trả về true nếu số lượng hiện tại lớn hơn hoặc bằng số lượng yêu cầu(amount).

        //Ép cộng thêm một số lượng amount vào currentAmount
        public bool ForceAddObjective(int amount)
        {
            currentCount += amount;
            return currentCount >= amount;
        }
        //        Cộng giá trị amount vào currentAmount.
        //Trả về true nếu số lượng hiện tại lớn hơn hoặc bằng số lượng yêu cầu.


        //Trả về chuỗi mô tả của mục tiêu.
        public override string ToString()
        {
            switch (type)
            {
                case Type.kill:
                    return "Kill " + /* MonsterList.MonsterNameFromID(objectiveId) + " " +*/ currentCount + "/" + requiredCount;
                case Type.talk:
                    return "Talk to " /*+ NpcList.NpcNameFromID(objectiveId) */;
                case Type.collect:
                    return "Collect " + /* ItemList.ItemNameFromID(objectiveId) + " " +*/ currentCount + "/" + requiredCount;
            }
            return "";
        }
        //        Lớp Quest giúp quản lý thông tin và logic của các nhiệm vụ trong game.
        //Lớp Objective xử lý mục tiêu cụ thể của nhiệm vụ, bao gồm kiểm tra trạng thái hoàn thành, cập nhật tiến trình, và hiển thị mô tả.
        //Đoạn mã này được tổ chức tốt với tính năng serialization, logic kiểm tra trạng thái, và khả năng mở rộng để tích hợp thêm các hệ thống phức tạp hơn.
    }
}
