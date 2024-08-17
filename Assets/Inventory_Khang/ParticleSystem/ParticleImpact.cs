using UnityEngine;

public class ParticleImpact : AbstractCtrl
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Impact:");
        // Kiểm tra xem đối tượng va chạm có tag "Enemy" hay không
        if (other.transform.parent.CompareTag("Enemy"))
        {
            Debug.Log("Impact: " + particleCtrl.particleDamesender.Damage);
            particleCtrl.particleDamesender.Send(other.transform.parent);

            var damagereceiver = other.transform.GetComponent<DamageReceiverByType>();
            if (damagereceiver != null)
            {
                ImpartEffectType(particleCtrl.particleDamesender.SkillType, damagereceiver); // Gọi hàm xử lý hiệu ứng dựa trên loại kỹ năng
            }
        }
    }

    private void ImpartEffectType(SkillType skill, DamageReceiverByType damagereceiver)
    {
        switch (skill)
        {
            case SkillType.Fire:
                damagereceiver.StartBurning(skill);
                break;
            case SkillType.Glace:
                damagereceiver.StartGlace(skill); 
                break;
            case SkillType.Electric:
                damagereceiver.StartElectric(skill);
                break;
            default:
                Debug.LogWarning("Không hỗ trợ loại kỹ năng này: " + skill);
                break;
        }
    }
}
