using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public enum SensorType
    {
        Enemy,
        BARRIER,
        PRESS,
        GIANT
    }

    public Player player;
    public SensorType sensorType;

    // 플레이어가 직접 적과 부딫혔을 때 트리거를 키는 것에 대한 문제 (플레이어가 밀림)
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // 베리어는 isBarrierSensor가 켜진 센서에서만 작동
            if (sensorType == SensorType.BARRIER && player.skillType == Player.SkillType.BARRIER)
            {
                // Explosion
                GameManager.instance.CallExplosion(collision.transform.position + Vector3.up * 0.5f);
                collision.gameObject.SetActive(false);
                // Audio
                GameManager.instance.audioManager.SfxPlay("Explosion");
                // 스킬 종료
                player.skillType = Player.SkillType.NONE;
                GameManager.instance.OffSkill();
                GameManager.instance.OnCool();
            }

            // 밟기는 isPressSensor가 켜진 센서에서만 작동
            else if (sensorType == SensorType.PRESS && player.skillType == Player.SkillType.PRESS_DOWN)
            {
                player.PlayerJump();
                // Explosion
                GameManager.instance.CallExplosion(collision.transform.position + Vector3.up * 0.5f);
                collision.gameObject.SetActive(false);
                // Audio
                GameManager.instance.audioManager.SfxPlay("Explosion");

            }

            // 거대화로 적 처치는 isGiantSensor가 켜진 센서에서만 작동
            else if (sensorType == SensorType.GIANT && player.skillType == Player.SkillType.GIANT)
            {
                collision.gameObject.SetActive(false);
                GameManager.instance.score += 200;
                // Audio
                GameManager.instance.audioManager.SfxPlay("Explosion");
            }

            else if (player.skillType == Player.SkillType.INVISIBLE || player.skillType == Player.SkillType.BUSTER)
            {
                collision.isTrigger = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 트리거 돌려놓기
        if (collision.gameObject.tag == "Enemy")
        {
            collision.isTrigger = false;
        }
    }
}
