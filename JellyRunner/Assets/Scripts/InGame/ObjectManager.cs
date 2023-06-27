using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // ������ ���� ����
    public GameObject[] prefabs;
    // Ǯ ��� ����Ʈ
    List<GameObject>[] pools;

    void Awake()
    {
        // �������� ���̸�ŭ ����Ʈ ����
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            // �� ����Ʈ�� �����ڸ� ���� �ʱ�ȭ
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        // ��ȯ�� ���� select ����
        GameObject select = null;

        // ������ Ǯ�� ��Ȱ��ȭ�� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // �߰��ϸ� select�� �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ��Ȱ��ȭ�� ������Ʈ�� ������ �߰�
        if (!select)
        {
            // Hierarchyâ�� �ƴ� ObjectManager �Ʒ��� �Ҵ��ϱ� ���� transform�� ����
            select = Instantiate(prefabs[index], transform);

            // ������Ʈ�� ���� ���������� Ǯ�� ���
            pools[index].Add(select);
        }

        // select ��ȯ
        return select;
    }

    public void DisableEnemy(int index)
    {
        // �ش� index�� pool���� Ȱ��ȭ�� ������Ʈ ��� ��Ȱ��ȭ
        foreach (GameObject item in pools[index])
        {
            if (item.activeSelf)
            {
                // Explosion
                GameManager.instance.CallExplosion(item.transform.position + Vector3.up * 0.5f);
                GameManager.instance.score += 200;
                item.SetActive(false);
            }
        }
    }
}
