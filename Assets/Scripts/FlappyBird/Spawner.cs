using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Collumn;

    private Coroutine coroutine;

    public void StartSpawning ()
	{
	    coroutine = StartCoroutine(Spawn());
	}

    public void StopSpawning()
    {
        StopCoroutine(coroutine);
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            if (FlappyBirdMain.singleton.GameOver)
                break;
            float rand = Random.Range(-7f, -2.5f);

            Instantiate(Collumn, new Vector3(12f, rand, 0f), Quaternion.identity);
            yield return new WaitForSeconds(2.5f);
        }
    }
}
