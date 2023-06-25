using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public Player player;
    public bool isBarrierSensor;
    public bool isPressSensor;

    // �÷��̾ ���� ���� �΋H���� �� Ʈ���Ÿ� Ű�� �Ϳ� ���� ���� (�÷��̾ �и�)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // ������� isBarrierSensor�� ���� ���������� �۵�
            if (isBarrierSensor && player.isBarrier)
            {
                // Explosion
                GameManager.instance.CallExplosion(collision.transform.position + Vector3.up * 0.5f);
                collision.gameObject.SetActive(false);
                // ��ų ����
                player.isBarrier = false;
                GameManager.instance.onSkill = false;
            }
            // ���� isPressSensor�� ���� ���������� �۵�
            else if (isPressSensor && player.isPress)
            {
                player.PlayerJump();
                // Explosion
                GameManager.instance.CallExplosion(collision.transform.position + Vector3.up * 0.5f);
                collision.gameObject.SetActive(false);

            }
            else if (player.isInvisibile || player.isBust)
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
