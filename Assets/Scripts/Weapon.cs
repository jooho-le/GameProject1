using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float attackPower = 20f;  // Weapon의 공격력 (인스펙터에서 설정 가능)

    // 무기와 충돌한 적에게 데미지를 입히는 함수
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 "Enemy" 태그를 가진 적이라면
        if (other.CompareTag("Enemy"))
        {
            // Enemy 컴포넌트를 가져와서 데미지 적용
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackPower);  // 공격력 만큼 데미지 적용
            }
        }
    }
}
