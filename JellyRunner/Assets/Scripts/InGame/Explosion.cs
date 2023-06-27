using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // ������ �� 2�� �� ������Ʈ ��Ȱ��ȭ
        Invoke("Disable", 2f);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    public void StartExplosion()
    {
        anim.SetTrigger("OnExplosion");
    }
}
