using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Manager : MonoBehaviour
{

    public GameObject player;
    public GameObject Area;
    public float speedEnemy;
    public GameObject enemyPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }


    #region Enemies manager
    #region Spawning enemies
    void SpawnEnemy()
    {
        // On fait apparaitre des Enemies avec 'T', on vérifie qu'il en existe pas déja 10 dans la zone de jeu grace au script 'AreaCollider'
        if (Input.GetKeyDown(KeyCode.T) /*&& Area.GetComponent<AreaCollider>().countCollisions < 10*/)
        {
            // On définie la zone de spawn 
            int spawnPointX = Random.Range(-24, 24);
            int spawnPointZ = Random.Range(-24, 24);
            Vector3 spawnPosition = new Vector3(spawnPointX, 0, spawnPointZ);
            // On instantiate les enemies dans la zone
            GameObject enemyClone = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity) as GameObject;
            // On lance la coroute qui va gérer les enemies
            StartCoroutine(handleEnemyFollow(enemyClone, true));
        }
    }
    #endregion Spawning enemies
    #region Coroutine following enemies
    IEnumerator handleEnemyFollow(GameObject enemyClone, bool condition)
    {

        while (condition == true && enemyClone != null && player != null)
        {
            // On fait regarder et avancer les clones enemy vers le joueur
            enemyClone.transform.LookAt(player.transform);
            enemyClone.transform.position += enemyClone.transform.forward * Time.deltaTime * speedEnemy;
            yield return null;
        }
        yield return null;
    }
    #endregion Coroutine following enemies
    #endregion enemies manager
}
