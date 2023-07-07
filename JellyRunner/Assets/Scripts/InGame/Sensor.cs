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

    // �÷��̾ ���� ���� �΋H���� �� Ʈ���Ÿ� Ű�� �Ϳ� ���� ���� (�÷��̾ �и�)
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // ������� isBarrierSensor�� ���� ���������� �۵�
            if (sensorType == SensorType.BARRIER && player.skillType == Player.SkillType.BARRIER)
            {
                // Explosion
                GameManager.instance.CallExplosion(collision.transform.position + Vector3.up * 0.5f);
                collision.gameObject.SetActive(false);
                // Audio
                GameManager.instance.audioManager.SfxPlay("Explosion");
                // ��ų ����
                player.skillType = Player.SkillType.NONE;
                GameManager.instance.OffSkill();
                GameManager.instance.OnCool();
            }

            // ���� isPressSensor�� ���� ���������� �۵�
            else if (sensorType == SensorType.PRESS && player.skillType == Player.SkillType.PRESS_DOWN)
            {
                player.PlayerJump();
                // Explosion
                GameManager.instance.CallExplosion(collision.transform.position + Vector3.up * 0.5f);
                collision.gameObject.SetActive(false);
                // Audio
                GameManager.instance.audioManager.SfxPlay("Explosion");

            }

            // �Ŵ�ȭ�� �� óġ�� isGiantSensor�� ���� ���������� �۵�
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
        // Ʈ���� ��������
        if (collision.gameObject.tag == "Enemy")
        {
            collision.isTrigger = false;
        }
    }
}
