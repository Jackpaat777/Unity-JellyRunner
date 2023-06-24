using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // 프리펩 보관 변수
    public GameObject[] prefabs;
    // 풀 담당 리스트
    List<GameObject>[] pools;

    void Awake()
    {
        // 프리펩의 길이만큼 리스트 생성
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            // 각 리스트를 생성자를 통해 초기화
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        // 반환할 변수 select 지정
        GameObject select = null;

        // 선택한 풀의 비활성화된 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // 발견하면 select에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 비활성화된 오브젝트가 없으면 추가
        if (!select)
        {
            // Hierarchy창이 아닌 ObjectManager 아래에 할당하기 위해 transform에 지정
            select = Instantiate(prefabs[index], transform);

            // 오브젝트를 새로 생성했으니 풀에 등록
            pools[index].Add(select);
        }

        // select 반환
        return select;
    }
}
