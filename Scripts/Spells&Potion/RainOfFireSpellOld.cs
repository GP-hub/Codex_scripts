using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RainOfFireSpellOld : AoESpell
{
    [SerializeField] 
    private GameObject prefab;

    [SerializeField]
    private float rainDuration;

    private bool rain;

    // Start is called before the first frame update
    void Start()
    {
        rain = true;
        StartCoroutine(Rain());
        StartCoroutine(Stop());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Rain()
    {
        while (rain == true)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
            GameObject clone = Instantiate(prefab, transform.position + new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4)), Quaternion.Euler(new Vector3(90, 0, 0)));
            Destroy(clone, 2f);
        }
    }

    IEnumerator Stop()
    {
        while (true)
        {
            yield return new WaitForSeconds(rainDuration);
            Debug.Log("STOP");
            rain = false;
            break;
        }
    }

    public override void Execute()
    {
        tickElapsed += Time.deltaTime;

        if (tickElapsed >= 1)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].TakeDamage(damage / duration, Player.MyInstance);
            }

            tickElapsed = 0;
        }
    }
}
