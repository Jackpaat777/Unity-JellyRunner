using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public Player player;
    public bool isBarrierSensor;
    public bool isPressSensor;

    // 플레이어가 직접 적과 부딫혔을 때 트리거를 키는 것에 대한 문제 (플레이어가 밀림)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // 베리어는 isBarrierSensor가 켜진 센서에서만 작동
            if (isBarrierSensor && player.isBarrier)
            {
                // Explosion
                GameManager.instance.CallExplosion(collision.transform.position + Vector3.up * 0.5f);
                collision.gameObject.SetActive(false);
                // 스킬 종료
                player.isBarrier = false;
                GameManager.instance.onSkill = false;
            }
            // 밟기는 isPressSensor가 켜진 센서에서만 작동
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
        // 트리거 돌려놓기
        if (collision.gameObject.tag == "Enemy")
        {
            collision.isTrigger = false;
        }
    }
}
