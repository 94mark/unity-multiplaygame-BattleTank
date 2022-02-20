using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankDamage : MonoBehaviour
{
    private MeshRenderer[] renderers;
    private GameObject expEffect = null;
    private int initHp = 100;
    private int currHp = 0;

    public Canvas hudCanvas;
    public Image hpBar;

    // Start is called before the first frame update
    void Awake()
    {
        //탱크 포파 후 투명 처리를 위한 메쉬렌더러 배열
        renderers = GetComponentsInChildren<MeshRenderer>();
        currHp = initHp;
        expEffect = Resources.Load<GameObject>("Large Explosion");

        hpBar.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currHp > 0 && other.tag == "CANNON")
        {
            currHp -= 20;
            hpBar.fillAmount = (float)currHp / (float)initHp;
            if (hpBar.fillAmount <= 4.0f)
                hpBar.color = Color.red;
            else if (hpBar.fillAmount <= 0.6f)
                hpBar.color = Color.yellow;

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
        hudCanvas.enabled = false;
        SetTankVisible(false);
        yield return new WaitForSeconds(3.0f);
        hpBar.fillAmount = 1.0f;
        hpBar.color = Color.green;
        hudCanvas.enabled = true;
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
