using UnityEngine;

public class ParticleImpact : AbstractCtrl
{
    private void OnParticleCollision(GameObject other)
    {
        // Kiểm tra xem đối tượng va chạm có tag "Enemy" hay không
        if (other.transform.parent != null && other.transform.parent.CompareTag("Enemy"))
        {
            Debug.Log("Impact: " + particleCtrl.particleDamesender.Damage);
            particleCtrl.particleDamesender.Send(other.transform.parent);

            var damagereceiver = other.transform.GetComponent<DamageReceiverByType>();
            if (damagereceiver != null)
            {
                ImpartEffectType(particleCtrl.SkillType, damagereceiver); // Gọi hàm xử lý hiệu ứng dựa trên loại kỹ năng
            }
        }
    }

    private void ImpartEffectType(SkillType skill, DamageReceiverByType damagereceiver)
    {
        switch (skill)
        {
            case SkillType.Meteorite:
                damagereceiver.StartBurning(); // Bắt đầu hiệu ứng đốt cháy với damage là 2 và thời gian 3 giây
                break;
            //case SkillType.Lightning:
            //    damagereceiver.StartTwitching(); // Bắt đầu hiệu ứng co giật với damage là 2 và thời gian 2 giây
            //    break;
            // Có thể thêm các case khác dựa trên các loại skill khác
            default:
                Debug.LogWarning("Không hỗ trợ loại kỹ năng này: " + skill);
                break;
        }
    }
}
