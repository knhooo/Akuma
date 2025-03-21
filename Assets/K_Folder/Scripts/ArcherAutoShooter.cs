using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcherAutoShooter : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 10f;
    public float shootInterval = 2f;

    void Start()
    {
        StartCoroutine(AutoShoot());
    }

    IEnumerator AutoShoot()
    {
        while (true)
        {
            Transform target = FindClosestMonster();
            if (target != null)
            {
                ShootAtTarget(target.position);
            }
            yield return new WaitForSeconds(shootInterval);
        }
    }

    Transform FindClosestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float dist = Vector2.Distance(transform.position, monster.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = monster.transform;
            }
        }

        return closest;
    }

    void ShootAtTarget(Vector3 targetPos)
    {
        Vector2 direction = (targetPos - firePoint.position).normalized;

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        arrow.transform.localScale = Vector3.one;

        // ȭ�� ���� ���� (Sprite�� �������̸� angle �״�� / �����̸� -90f ��)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle); // ���� ����

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * arrowSpeed;
        }
    }
}
