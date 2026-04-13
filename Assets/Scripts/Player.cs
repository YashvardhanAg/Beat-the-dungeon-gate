using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    private GameManager gameManagerScript;
    private Vector3[] columnPositions = new Vector3[4];
    private int ind = 0;
    private float yPoint = -47.3f;
    private float zPoint = 36.28f;
    public bool dungeonCollided = false;
    private bool isPowerUpActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        for(int i =0; i<4; i++)
        {
            columnPositions[i] = new Vector3(gameManagerScript.xSpawnPoints[i], yPoint, zPoint);
        }
        transform.position = columnPositions[0];
    }
    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        //Player Controls
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && gameManagerScript.isGameActive)
        {
            ind++;
            if (ind > 3)
            {
                ind = 3;
            }
            transform.position = columnPositions[ind];
        }
        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && gameManagerScript.isGameActive)
        {
            ind--;
            if (ind < 0)
            {
                ind = 0;
            }
            transform.position = columnPositions[ind];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Numbers")&&!isPowerUpActive)
        {
            int scoreInd = other.GetComponent<Numbers>().value;
                    Destroy(other.gameObject);
                    switch (ind)
                    {
                        case 0:
                            gameManagerScript.score += scoreInd;
                            //addition
                            break;
                        case 1:
                            gameManagerScript.score -= scoreInd;
                            //subtraction
                            break;
                        case 2:
                            gameManagerScript.score *= scoreInd;
                            //mulitply
                            break;
                        case 3:
                            gameManagerScript.score %= scoreInd;
                            //divide
                            break;
                    }
        }

        if(other.CompareTag("Dungeon Gate"))
        {
            
            gameManagerScript.isGameActive = false;
            dungeonCollided = true;
            gameManagerScript.isGameOver();
        }
        if (other.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            StartCoroutine(PowerupEffect());
        }
    }

    IEnumerator PowerupEffect()
    {
        isPowerUpActive = true;
        yield return new WaitForSeconds(4.5f);
        isPowerUpActive = false;
    }
}
