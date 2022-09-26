using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] private float lifetime;
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;


    private Vector3 iniPos;
    private Vector3 targetPos;
    private float timer;
    private void Start()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
        float direction = Random.rotation.eulerAngles.z;
        iniPos = transform.position;
        float dist = Random.Range(minDist, maxDist);
        targetPos = iniPos + (Quaternion.Euler(0,0,direction) * new Vector3 (dist,dist,0));
        transform.localScale = Vector3.zero;

    }

    private void Update()
    {
        timer += Time.deltaTime;
        float fraction = lifetime / 2f;
        if (timer > lifetime) Destroy(gameObject);
        else if (timer < fraction)
            text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));
        {

        }
        transform.position =Vector3.Lerp(iniPos, targetPos, Mathf.Sin(timer/lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero,new Vector3(0.04f, 0.04f, 0.04f), Mathf.Sin(timer/lifetime));



        

    }
    public void SetDamageText(float damage)
    {
        text.text = damage.ToString();
    }
}
