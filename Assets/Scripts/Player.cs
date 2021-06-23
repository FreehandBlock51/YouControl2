using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MechanicBehaviour
{
    /// <summary>
    /// The player instance in the level.
    /// There should only be at most one <c>Player</c> Component in each Scene
    /// </summary>
    public static Player Main => GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    public Animator Animator => gameObject.GetComponent<Animator>();

    public string HorizontalAxisName = "Horizontal";
    public string VerticalAxisName = "Vertical";
    public string AttackButtonName = "Jump";
    [Min(1 / float.MaxValue)]
    public float speed = 5f;

    private Vector2 CurrentMovement = Vector2.zero;
    Vector2 spawn;

    public Canvas pauseCanvas;
    public Canvas HUDCanvas;
    public Canvas finishCanvas;
    bool paused;

    public Rigidbody2D Rigidbody => GetComponent<Rigidbody2D>();

    // Start is called before the first frame update
    void Start()
    {
        spawn = Rigidbody.position;
        respawning = false;

        pauseCanvas ??= GetComponentInChildren<Canvas>();
        if (pauseCanvas)
        {
            pauseCanvas.gameObject.SetActive(false);
        }
        if (HUDCanvas)
        {
            HUDCanvas.gameObject.SetActive(true);
        }
        if (finishCanvas)
        {
            finishCanvas.gameObject.SetActive(false);
        }
        paused = false;
    }

    public void Respawn() => respawning = true;

    Vector2 movement;
    bool respawning;


    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            Rigidbody.velocity = Vector2.zero;
            return;
        }
        if (respawning)
        {
            Rigidbody.MovePosition(spawn);
            Rigidbody.velocity = Vector2.zero;
            EntanglableObject.ResetEntanglement();
            respawning = false;
            return;
        }
        movement = new Vector2(Input.GetAxisRaw(HorizontalAxisName), Input.GetAxisRaw(VerticalAxisName)) * speed;
        Rigidbody.velocity = movement;
        
        Animate(movement);
    }

    void Animate(Vector2 movement)
    {
        if (Animator.runtimeAnimatorController)
        {
            Animator.SetFloat("X", movement.x);
            Animator.SetFloat("Y", movement.y);
        }
    }

    public void LoadMainMenu()
    {
        ResetLevel();
        SceneManager.LoadScene(0);
    }
    public void TogglePauseScreen()
    {
        SetPauseState(!paused);
    }

    public void SetPauseState(bool paused)
    {
        this.paused = paused;
        pauseCanvas.gameObject.SetActive(paused);
    }

    public static bool speedrunning = false;
    public static void ChangeMode(UnityEngine.UI.Toggle toggle) => speedrunning = toggle.isOn;

    public void Finish()
    {
        if (speedrunning)
        {
            int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
            try
            {
                if (SceneManager.GetSceneByBuildIndex(nextLevel) != null)
                {
                    SceneManager.LoadScene(nextLevel);
                }
            }
            catch
            {
                // continue normally
            }
        }
        SetPauseState(true);
        int maxLevel = SceneManager.GetActiveScene().buildIndex - 3;
        if (maxLevel >= 0 && maxLevel > PlayerPrefs.GetInt("Progress", -1))
        {
            PlayerPrefs.SetInt("Progress", maxLevel);
        }
        HUDCanvas.gameObject.SetActive(false);
        finishCanvas.gameObject.SetActive(true);
    }

    public void ResetLevel()
    {
        HUDCanvas.gameObject.SetActive(true);
        finishCanvas.gameObject.SetActive(false);
        Respawn();
        foreach (var e in Object.FindObjectsOfType<ResettableMonoBehaviour>())
        {
            e.ResetPos();
        }
        SetPauseState(false);
    }
}
