using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDamage : MonoBehaviour
{
    private MeshRenderer[] renderers;
    private GameObject expEffect = null;
    private int initHp = 100;
    private int currHp = 0;
    // Start is called before the first frame update
    void Awake()
    {
        //탱크 포파 후 투명 처리를 위한 메쉬렌더러 배열
        renderers = GetComponentsInChildren<MeshRenderer>();
        currHp = initHp;
        expEffect = Resources.Load<GameObject>("Large Explosion");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currHp > 0 && other.tag == "CANNON")
        {
            currHp -= 20;
            if(currHp <= 0)
            {
                StartCoroutine(this.ExplosionTank());
            }
        }
    }

    IEnumerator ExplosionTank()
    {
        Object effect = GameObject.Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3.0f);
        SetTankVisible(false);
        yield return new WaitForSeconds(3.0f);
        currHp = initHp;
        SetTankVisible(true);
    }

    void SetTankVisible(bool isVisible)
    {
        foreach (MeshRenderer _renderer in renderers)
        {
            _renderer.enabled = isVisible;
        }
    }
}
