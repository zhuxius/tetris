using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shape : MonoBehaviour
{
    // Define that we want shape to fall 1 unit/sec
    public static float speed = 1.0f;

    float lastMoveDown = 0;

    public static int difficulty = 0;

    // Use this for initialization
    void Start()
    {
        if (!InGrid())
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.gameOver);
            Invoke("OpenGameOverScene", 0.5f);
        }
    }

    void OpenGameOverScene()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }

    void IncreaseSpeed()
    {
        Shape.speed *= 0.90f;
    }


    // Update is called once per frame
    void Update()
    {
        // Move Left
        if (Input.GetKeyDown("a"))
        {
            transform.position += new Vector3(-1, 0, 0);

            // Debug.Log(transform.position);

            if (!InGrid())
            {
                transform.position += new Vector3(1, 0, 0);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }
        }

        // Move Right
        if (Input.GetKeyDown("d"))
        {
            transform.position += new Vector3(1, 0, 0);

            // Debug.Log(transform.position);

            if (!InGrid())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }
        }

        // Move Down

        //if (Input.GetKey("s"))
        if (Input.GetKey("s") || Time.time - lastMoveDown >= Shape.speed)
        {
            transform.position += new Vector3(0, -1, 0);

            // Debug.Log(transform.position);

            if (!InGrid())
            {

                transform.position += new Vector3(0, 1, 0);

                int rowsDeleted = GameBoard.DeleteAllFullRows();

                if (rowsDeleted > 0)
                {
                    IncreaseTextUIScore(rowsDeleted);
                }

                enabled = false;

                FindObjectOfType<ShapeSpawner>().SpawnShape();

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeStop);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeMove);
            }

            lastMoveDown = Time.time;
        }

        // Fast down
        if (Input.GetKeyDown("space"))
        {
            while (InGrid())
            {
                transform.position += new Vector3(0, -1, 0);
                // Debug.Log(transform.position);
            }
            // Debug.Log("Final pos: " + transform.position);
            if (!InGrid())
            {
                Debug.Log("!InGrid: " + transform.position);
                transform.position += new Vector3(0, 1, 0);

                UpdateGameBoard();

                int rowsDeleted = GameBoard.DeleteAllFullRows();

                // Debug.Log(rowsDeleted);

                // Debug.Log("This turn");
                IncreaseTextUIScore(rowsDeleted);

                // DO NOT REMOVE, BREAKS GAME
                enabled = false;

                FindObjectOfType<ShapeSpawner>().SpawnShape();

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.shapeStop);
            }


        }

        // Rotate
        if (Input.GetKeyDown("w"))
        {
            transform.Rotate(0, 0, 90);

            // Debug.Log(transform.position);
            if (!InGrid())
            {
                transform.Rotate(0, 0, -90);
            }
            else
            {
                UpdateGameBoard();
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.rotateSound);
            }

        }
    }

    // Method to round vectors to prevent precision errors
    public Vector2 RoundVector(Vector2 vect)
    {
        return new Vector2(Mathf.Round(vect.x), Mathf.Round(vect.y));
    }

    // Check to see if each cube in prefab'd shapes are legal
    public bool InGrid()
    {
        int childCount = 0;

        foreach (Transform childBlock in transform)
        {
            Vector2 vect = RoundVector(childBlock.position);
            childCount++;

            // Debug.Log(childCount + " " + childBlock.position);
            // Debug.Log("-------------");

            if (!InBorder(vect))
            {
                return false;
            }

            if (GameBoard.gameBoard[(int)vect.x, (int)vect.y] != null &&
                GameBoard.gameBoard[(int)vect.x, (int)vect.y].parent != transform)
            {
                return false;
            }

        }
        return true;
    }

    // Check to see if object is legally in borders of game
    public static bool InBorder(Vector2 pos)
    {
        return ((int)pos.x <= 9 &&
                (int)pos.x >= 0 &&
                (int)pos.y >= 0 &&
                (int)pos.y <= 19);
    }

    // Determines if GameBoard spaces are valid and available for occupation
    public void UpdateGameBoard()
    {
        for (int y = 0; y < 20; ++y)
        {
            for (int x = 0; x < 10; ++x)
            {
                if (GameBoard.gameBoard[x, y] != null &&
                    GameBoard.gameBoard[x, y].parent == transform)
                {
                    GameBoard.gameBoard[x, y] = null;
                }
            }
        }

        // Logs each cube's location in console
        foreach (Transform childBlock in transform)
        {
            Vector2 vect = RoundVector(childBlock.position);

            GameBoard.gameBoard[(int)vect.x, (int)vect.y] = childBlock;

            // Debug.Log("Cube At: " + vect.x + " " + vect.y);

        }
        // Displays location to help debug
        // GameBoard.PrintArray();
    }

    void IncreaseTextUIScore(int rowsCleared)
    {
        var textUIComp = GameObject.Find("Score").GetComponent<Text>();

        int score = int.Parse(textUIComp.text);

        score += 10 * rowsCleared;
        difficulty += rowsCleared;

        if (difficulty - 5 >= 0)
        {
            IncreaseSpeed();
            difficulty = 0;
            // Debug.Break();
            // Debug.Log("Speed: " + Shape.speed + " Difficulty: " + Shape.difficulty);

        }

        textUIComp.text = score.ToString();
    }


}