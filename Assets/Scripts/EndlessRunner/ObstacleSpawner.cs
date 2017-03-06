using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private Coroutine currentCoroutine;
    public static ObstacleSpawner singleton;
    public Obstacle[] Obstacles;
    private List<UnityChan.Lane> takenLanes;
    public GameObject MiddleLane;
    public GameObject LeftLane;
    public GameObject RightLane;
    public GameObject MidRightLane;
    public GameObject MidLeftLane;

    void Start()
    {
        singleton = this;
        takenLanes = new List<UnityChan.Lane>();
    }

    public void StartSpawning()
    {
        currentCoroutine = StartCoroutine(Spawn());
    }

    public void StopSpawning()
    {
        StopCoroutine(currentCoroutine);
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            int blockedLanes = 0;
            int obstaclesToSpawn = Random.Range(2, 3);
            for (int i = 0; i < obstaclesToSpawn; i++)
            {
                List<Obstacle> availableObstacles;
                if (takenLanes.Count == 2 || takenLanes.Contains(UnityChan.Lane.Middle))
                {
                    availableObstacles = Obstacles.Where(x => x.TakenLanes == 1).ToList();
                }
                else
                {
                    availableObstacles = Obstacles.Where(x => x.TakenLanes <= 2).ToList();
                }
                availableObstacles = availableObstacles.Where(x => !x.Blocker || blockedLanes + x.TakenLanes < 3).ToList();


                int obstacleIndex = Random.Range(0, availableObstacles.Count);
                Obstacle obstacleToSpawn = availableObstacles[obstacleIndex];

                if (obstacleToSpawn.Blocker)
                {
                    blockedLanes += obstacleToSpawn.TakenLanes;
                }

                if (obstacleToSpawn.TakenLanes == 2)
                {
                    if (takenLanes.Contains(UnityChan.Lane.Left))
                    {
                        Instantiate(obstacleToSpawn, MidRightLane.transform.position, obstacleToSpawn.transform.rotation);
                        takenLanes.Add(UnityChan.Lane.Middle);
                        takenLanes.Add(UnityChan.Lane.Right);
                        //between mid and right
                    }
                    else if (takenLanes.Contains(UnityChan.Lane.Right))
                    {
                        Instantiate(obstacleToSpawn, MidLeftLane.transform.position, obstacleToSpawn.transform.rotation);
                        takenLanes.Add(UnityChan.Lane.Middle);
                        takenLanes.Add(UnityChan.Lane.Left);
                        //mid and left
                    }
                    else
                    {
                        int rand = Random.Range(1, 2);
                        if (rand == 1)
                        {
                            Instantiate(obstacleToSpawn, MidRightLane.transform.position, obstacleToSpawn.transform.rotation);
                            takenLanes.Add(UnityChan.Lane.Middle);
                            takenLanes.Add(UnityChan.Lane.Right);
                            //mid right
                        }
                        else
                        {
                            Instantiate(obstacleToSpawn, MidLeftLane.transform.position, obstacleToSpawn.transform.rotation);
                            takenLanes.Add(UnityChan.Lane.Middle);
                            takenLanes.Add(UnityChan.Lane.Left);
                            //mid left
                        }
                    }
                    if (obstaclesToSpawn == 3)
                    {
                        obstaclesToSpawn--;
                    }
                }
                else
                {
                    if (takenLanes.Count == 2)
                    {
                        if (!takenLanes.Contains(UnityChan.Lane.Left))
                        {
                            Instantiate(obstacleToSpawn, LeftLane.transform.position, obstacleToSpawn.transform.rotation);
                            takenLanes.Add(UnityChan.Lane.Left);
                            //left
                        }
                        else if (!takenLanes.Contains(UnityChan.Lane.Right))
                        {
                            Instantiate(obstacleToSpawn, RightLane.transform.position, obstacleToSpawn.transform.rotation);
                            takenLanes.Add(UnityChan.Lane.Right);
                            //right
                        }
                        else
                        {
                            Instantiate(obstacleToSpawn, MiddleLane.transform.position, obstacleToSpawn.transform.rotation);
                            takenLanes.Add(UnityChan.Lane.Middle);
                            //mid
                        }
                    }
                    else if (takenLanes.Count == 1)
                    {
                        if (takenLanes.Contains(UnityChan.Lane.Left))
                        {
                            int rand = Random.Range(1, 2);
                            if (rand == 1)
                            {
                                Instantiate(obstacleToSpawn, MiddleLane.transform.position, obstacleToSpawn.transform.rotation);
                                takenLanes.Add(UnityChan.Lane.Middle);
                            }
                            else
                            {
                                Instantiate(obstacleToSpawn, RightLane.transform.position, obstacleToSpawn.transform.rotation);
                                takenLanes.Add(UnityChan.Lane.Right);
                            }
                            //mid or right 
                        }
                        else if (takenLanes.Contains(UnityChan.Lane.Right))
                        {
                            int rand = Random.Range(1, 2);
                            if (rand == 1)
                            {
                                Instantiate(obstacleToSpawn, MiddleLane.transform.position, obstacleToSpawn.transform.rotation);
                                takenLanes.Add(UnityChan.Lane.Middle);
                            }
                            else
                            {
                                Instantiate(obstacleToSpawn, LeftLane.transform.position, obstacleToSpawn.transform.rotation);
                                takenLanes.Add(UnityChan.Lane.Left);
                            }
                            //left or mid
                        }
                        else
                        {
                            int rand = Random.Range(1, 2);
                            if (rand == 1)
                            {
                                Instantiate(obstacleToSpawn, LeftLane.transform.position, obstacleToSpawn.transform.rotation);
                                takenLanes.Add(UnityChan.Lane.Left);
                            }
                            else
                            {
                                Instantiate(obstacleToSpawn, RightLane.transform.position, obstacleToSpawn.transform.rotation);
                                takenLanes.Add(UnityChan.Lane.Right);
                            }
                            //left or right
                        }
                    }
                    else
                    {
                        int rand = Random.Range(1, 3);
                        if (rand == 1)
                        {
                            Instantiate(obstacleToSpawn, MiddleLane.transform.position, obstacleToSpawn.transform.rotation);
                            takenLanes.Add(UnityChan.Lane.Middle);
                        }
                        else if (rand == 2)
                        {
                            Instantiate(obstacleToSpawn, LeftLane.transform.position, obstacleToSpawn.transform.rotation);
                            takenLanes.Add(UnityChan.Lane.Left);
                        }
                        else
                        {
                            Instantiate(obstacleToSpawn, RightLane.transform.position, obstacleToSpawn.transform.rotation);
                            takenLanes.Add(UnityChan.Lane.Right);
                        }
                        //all
                    }
                }
            }
            takenLanes = new List<UnityChan.Lane>();
            yield return new WaitForSeconds(2);
        }
    }
}
