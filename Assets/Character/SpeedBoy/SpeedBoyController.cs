using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedBoyController : MonoBehaviour, ICharacter
{
    [SerializeField]
    private float timeDelayBeforeDie;

    [Header("Reference Component")]
    [SerializeField]
    private SpeedBoyMovement speedBoyMovement;

    [SerializeField]
    private Animator myAnimator;

    [SerializeField]
    private GameObject animRoot;

    [Header("Effects")]
    [SerializeField]
    private ParticleSystem deadEffect;

    [SerializeField]
    private ParticleSystem cloakEffect;

    [SerializeField]
    private ParticleSystem dasingEffect;

    // Reset Point and facing direction is restored when player die
    [HideInInspector]
    public Vector3 ResetPointPosition { get; set; }
    [HideInInspector]
    public int SaveFacingDirection { get; set; }
    public CharacterState State { get; protected set; }

    // Private fields
    private Coroutine coroutineWaitingToDie;

    /*private void Awake()
    {
        int numberObject = FindObjectsOfType<SpeedBoyController>().Length;
        if (numberObject > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }*/

    private void Start()
    {
        // Reset Player
        animRoot.SetActive(true);
        State = CharacterState.ALIVE;
    }

    private void Update()
    {
        // TODO: Refactor this code 
        var state = SpeedBoyState.Instance;
        
        if(State == CharacterState.DEAD)
        {
            animRoot.SetActive(false);
            cloakEffect.gameObject.SetActive(false);
            dasingEffect.gameObject.SetActive(false);
        }
        else
        {
            animRoot.SetActive(!state.Punching && !state.PunchSlice);
            cloakEffect.gameObject.SetActive(!state.Punching && !state.PunchSlice);
            dasingEffect.gameObject.SetActive(state.PunchSlice || state.Punching);
        }
    }

    public void Die()
    {
        if (coroutineWaitingToDie != null) return;
        coroutineWaitingToDie = StartCoroutine(WaitingToDie());
    }

    private IEnumerator WaitingToDie()
    {
        // Dead ---------------------------------------------------------------------
        State = CharacterState.DEAD;
        speedBoyMovement.IsRunning = false;
        //Play Dead Effect
        deadEffect.Play();
        // Wait for delay time
        yield return new WaitForSeconds(timeDelayBeforeDie);
        
        // Reset the scence
        int currentScence = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScence);
        //

        // Reset -----------------------------------------------------------------------
        // Set player position back to the reset point
        speedBoyMovement.CombackToResetPoint(ResetPointPosition, SaveFacingDirection);
        State = CharacterState.ALIVE;
        speedBoyMovement.IsRunning = true;
        coroutineWaitingToDie = null;
    }
}
