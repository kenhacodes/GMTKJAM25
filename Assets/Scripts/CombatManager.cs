using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private bool playerWins = false;

    public float CurrentCombatTimeLeft = 0.0f;
    public float MaxCombatTimeSeconds = 30.0f;
    private bool combatStarted = false;
    [SerializeField] public TMP_Text clockText;

    private Camera camera;

    public Vector3 centerOfRing = Vector3.zero;
    public float heightCameraOffsetRingCinematic = 7.0f;
    public float timeToCompleteRingCinematicSeconds = 2.0f;
    public float cameraRadius = 12.0f;

    public float combatCameraTimeToComplete = 10.0f;

    public GameObject RobotPrefab;
    public Vector3 playerStartPosition;
    public Vector3 enemyStartPosition;
    private RobotController PlayerRobot;
    private RobotController EnemyRobot;

    private float playerHealth;
    private float enemyHealth;

    private Image vsImage;
    private Image[] combatImages;

    public Sprite[] fightCountdown;

    public GameObject panelVSScreen;
    public GameObject panelCombat;
    public GameObject panelEndCombat;

    [SerializeField] private AudioSource soundFXObject;
    public AudioClip[] backgroundSounds;
    private SoundManager _soundManager;

    [SerializeField] public TMP_Text endCombatText;

    public CombatSceneFlow currentCombatState = CombatSceneFlow.Start;

    public enum CombatSceneFlow
    {
        Start,
        CinematicRingCircle,
        CinematicVS,
        CinematicStartCountdown,
        Combat,
        EndCombatScreen
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        camera.fieldOfView = 60.0f;
        clockText.text = "";

        _soundManager = FindObjectOfType<SoundManager>();
        if (_soundManager == null) Debug.Log("Sound Manager NOT FOUND!!!");

        StartCoroutine(RunCameraRingCinematic(390.0f));

        PlayerRobot = Instantiate(RobotPrefab, playerStartPosition, Quaternion.identity)
            .GetComponent<RobotController>();
        EnemyRobot = Instantiate(RobotPrefab, enemyStartPosition, Quaternion.identity)
            .GetComponent<RobotController>();

        EnemyRobot.player = false;
        EnemyRobot.InitRobot();

        EnemyRobot.tr.LookAt(PlayerRobot.tr);
        PlayerRobot.tr.LookAt(EnemyRobot.tr);

        combatImages = panelCombat.GetComponentsInChildren<Image>();

        StartCoroutine(PlayGeneralCrowd(true));
        StartCoroutine(PlayStompSfx(9.0f));

        UpdateUIPanels();
    }


    // Update is called once per frame
    void Update()
    {
        if (currentCombatState == CombatSceneFlow.Combat)
        {
            if (PlayerRobot.health <= 0.0f)
            {
                playerWins = true;
                EndMatch();
            }

            if (EnemyRobot.health <= 0.0f)
            {
                playerWins = false;
                EndMatch();
            }

            if (CurrentCombatTimeLeft <= 0.0f)
            {
                if (PlayerRobot.health > EnemyRobot.health) playerWins = true;
                else playerWins = false;
                EndMatch();
            }

            if (combatStarted) UpdateClock();
        }

        UpdateUIPanels();

        // Sound Update
        PlayerRobot.SFXSoundLevelVolume = _soundManager.masterVolume * _soundManager.sfxVolume;
        EnemyRobot.SFXSoundLevelVolume = _soundManager.masterVolume * _soundManager.sfxVolume;
    }

    void UpdateClock()
    {
        CurrentCombatTimeLeft -= Time.deltaTime;
        CurrentCombatTimeLeft = Mathf.Clamp(CurrentCombatTimeLeft, 0, MaxCombatTimeSeconds);

        // Round to nearest integer or format with 1 decimal if preferred
        int secondsLeft = Mathf.CeilToInt(CurrentCombatTimeLeft);
        clockText.text = secondsLeft.ToString();
    }


    private void EndMatch()
    {
        combatStarted = false;
        currentCombatState = CombatSceneFlow.EndCombatScreen;
        clockText.text = "";

        UpdateUIPanels();

        EnemyRobot.rb.velocity = Vector3.zero;
        PlayerRobot.rb.velocity = Vector3.zero;
        EnemyRobot.rb.angularVelocity = Vector3.zero;
        PlayerRobot.rb.angularVelocity = Vector3.zero;
        
        _soundManager.PlayMusic(_soundManager.musicLibrary[4]);

        if (playerWins)
        {
            FindTaggedChild(EnemyRobot.transform, "RobotModel").gameObject.SetActive(false);
        }
        else
        {
            FindTaggedChild(PlayerRobot.transform, "RobotModel").gameObject.SetActive(false);
        }

        StartCoroutine(EndMatchAnimation());
    }

    private IEnumerator PlayGeneralCrowd(bool fade)
    {
        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);
        audioSource.clip = backgroundSounds[0];
        audioSource.pitch = Random.Range(audioSource.pitch - 0.05f, audioSource.pitch + 0.05f);

        float finalVolume = _soundManager.bgSfxVolume * _soundManager.masterVolume * 0.7f;

        if (fade)
        {
            audioSource.volume = 0f;
            audioSource.Play();
            audioSource.DOFade(finalVolume, 3.0f); // Adjust fade duration as needed
        }
        else
        {
            audioSource.volume = finalVolume;
            audioSource.Play();
        }

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);

        yield return new WaitForSeconds(clipLength * 0.9f);
        StartCoroutine(PlayGeneralCrowd(false));
    }

    private IEnumerator PlayStompSfx(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("stomp!");
        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);
        audioSource.clip = backgroundSounds[1];
        audioSource.pitch = Random.Range(audioSource.pitch, audioSource.pitch);
        audioSource.volume = _soundManager.bgSfxVolume * _soundManager.masterVolume;
        float clipLength = audioSource.clip.length;
        audioSource.Play();
        Destroy(audioSource.gameObject, clipLength);
        float nexttime = Random.Range(6f, 15f);
        StartCoroutine(PlayStompSfx(nexttime));
    }

    private IEnumerator EndMatchAnimation()
    {
        while (currentCombatState == CombatSceneFlow.EndCombatScreen)
        {
            // Get winner's position (updated each frame)
            Vector3 positionWinner = playerWins ? PlayerRobot.tr.position : EnemyRobot.tr.position;
            positionWinner += Vector3.up * 0.7f;

            // Calculate dynamic target camera position behind winner
            Vector3 forwardDir = playerWins ? PlayerRobot.tr.forward : EnemyRobot.tr.forward;
            Vector3 targetCameraPos = positionWinner + (forwardDir * 4.0f) - Vector3.up * 0.4f;

            camera.fieldOfView = 40.0f;

            // Calculate target rotation to look at winner
            Quaternion targetRot = Quaternion.LookRotation(positionWinner - camera.transform.position);

            // Smoothly interpolate toward position and rotation
            camera.transform.position =
                Vector3.Lerp(camera.transform.position, targetCameraPos, Time.deltaTime * 2.0f);
            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRot, Time.deltaTime * 1.0f);

            yield return null;
        }
    }

    private Transform FindTaggedChild(Transform parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.CompareTag(tag)) return child;
        }

        return null;
    }

    private IEnumerator VSAnimation()
    {
        vsImage = panelVSScreen.GetComponentInChildren<Image>();

        Vector3 finalScaleImage = vsImage.transform.localScale;
        vsImage.transform.localScale = Vector3.zero;

        vsImage.transform.DOScale(finalScaleImage, 1.0f)
            .SetEase(Ease.OutBack);

        // Find robot models
        Transform playerModel = FindTaggedChild(PlayerRobot.transform, "RobotModel");
        Transform enemyModel = FindTaggedChild(EnemyRobot.transform, "RobotModel");

        GameObject clone1 = null;
        GameObject clone2 = null;

        if (playerModel != null)
        {
            clone1 = Instantiate(playerModel.gameObject);


            Vector3 startPos = camera.transform.position
                               + (camera.transform.forward * 2.2f)
                               - (camera.transform.up * 1.6f)
                               - (camera.transform.right * 5.0f);

            clone1.transform.position = startPos;
            clone1.transform.rotation = camera.transform.rotation;
            clone1.transform.Rotate(Vector3.up, -80.0f);
            clone1.transform.Rotate(Vector3.forward, 20.0f);
            clone1.transform.localScale = playerModel.localScale;

            Vector3 targetPos = startPos + (camera.transform.right * 3.8f);
            clone1.transform.DOMove(targetPos, 1.7f).SetEase(Ease.OutQuart);
        }

        if (enemyModel != null)
        {
            clone2 = Instantiate(enemyModel.gameObject);

            Vector3 startPos = camera.transform.position
                               + (camera.transform.forward * 2.2f)
                               - (camera.transform.up * 1.6f)
                               + (camera.transform.right * 5.0f);

            clone2.transform.position = startPos;
            clone2.transform.rotation = camera.transform.rotation;
            clone2.transform.Rotate(Vector3.up, 80.0f);
            clone2.transform.Rotate(Vector3.forward, -20.0f);
            clone2.transform.localScale = enemyModel.localScale;

            Vector3 targetPos = startPos - (camera.transform.right * 3.8f);
            clone2.transform.DOMove(targetPos, 1.7f).SetEase(Ease.OutQuart);
        }

        yield return new WaitForSeconds(4.0f);

        vsImage.transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.OutCubic);

        clone1.transform.DOMove(clone1.transform.position + (-camera.transform.right * 8.0f), 0.5f);
        clone2.transform.DOMove(clone2.transform.position + (camera.transform.right * 8.0f), 0.5f);

        Destroy(clone1, 1.0f);
        Destroy(clone2, 1.0f);

        yield return new WaitForSeconds(1.2f);

        vsImage.transform.localScale = finalScaleImage;

        StartCombatState();
    }

    void StartCombatState()
    {
        currentCombatState = CombatSceneFlow.Combat;
        PlayerRobot.health = PlayerRobot.maxHealth;
        EnemyRobot.health = EnemyRobot.maxHealth;
        CurrentCombatTimeLeft = MaxCombatTimeSeconds;
        combatStarted = false;
        combatImages[0].enabled = false;
        combatImages[1].enabled = false;
        UpdateUIPanels();
        StartCoroutine(AnimationCombatCountdownStart());
    }

    private IEnumerator AnimationCombatCountdownStart()
    {
        // 3
        combatImages[0].enabled = true;
        combatImages[0].sprite = fightCountdown[0];
        combatImages[0].transform.localScale = Vector3.zero;
        combatImages[0].transform.DOScale(Vector3.one * 5.0f, 1.0f)
            .SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.0f);

        // 2
        combatImages[0].sprite = fightCountdown[1];
        combatImages[0].transform.localScale = Vector3.zero;
        combatImages[0].transform.DOScale(Vector3.one * 4.7f, 1.0f)
            .SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.0f);

        // 1
        combatImages[0].sprite = fightCountdown[2];
        combatImages[0].transform.localScale = Vector3.zero;
        combatImages[0].transform.DOScale(Vector3.one * 5.4f, 1.0f)
            .SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.0f);

        // Fight!

        combatImages[0].sprite = fightCountdown[3];
        combatImages[0].transform.localScale = Vector3.zero;
        combatImages[0].transform.DOScale(Vector3.one * 6.5f, 0.65f)
            .SetEase(Ease.OutBack);

        combatImages[1].enabled = true;
        combatImages[1].sprite = fightCountdown[4];
        combatImages[1].transform.localScale = Vector3.zero;
        combatImages[1].transform.DOScale(Vector3.one * 7.0f, 0.75f)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(1.0f);
        combatImages[0].enabled = false;
        combatImages[1].enabled = false;
        combatStarted = true;
        StartCoroutine(CombatCamera());
    }

    private IEnumerator RunCameraRingCinematic(float degreesToRotate)
    {
        float elapsed = 0f;
        float duration = timeToCompleteRingCinematicSeconds;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Total angle to rotate (t from 0 to 1)
            float angle = t * degreesToRotate;
            float rad = angle * Mathf.Deg2Rad;

            // Calculate camera position in circle
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * cameraRadius;
            Vector3 cameraPosition = centerOfRing + offset + Vector3.up * heightCameraOffsetRingCinematic;

            camera.transform.position = cameraPosition;
            camera.transform.LookAt(centerOfRing + Vector3.up);

            yield return null;
        }

        camera.transform.LookAt(centerOfRing + Vector3.up);
        StartCinematicVS();
    }

    private IEnumerator CombatCamera()
    {
        float elapsed = 0f;

        while (combatStarted)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / combatCameraTimeToComplete;

            // Calculate circular orbit position
            float angle = t * 360.0f;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * cameraRadius;
            Vector3 desiredPosition = centerOfRing + offset + Vector3.up * heightCameraOffsetRingCinematic;

            camera.transform.position = Vector3.Lerp(camera.transform.position, desiredPosition, Time.deltaTime * 3f);

            float middlepoint = 0.7f;
            if (Vector3.Distance(PlayerRobot.tr.position, EnemyRobot.tr.position) > 4.0f)
            {
                middlepoint = 0.5f;
            }

            Vector3 lookPosition = ((PlayerRobot.tr.position - EnemyRobot.tr.position) * middlepoint +
                                    EnemyRobot.tr.position) + Vector3.up;

            // Smooth rotation
            Quaternion desiredRotation = Quaternion.LookRotation(lookPosition - camera.transform.position);
            camera.transform.rotation =
                Quaternion.Slerp(camera.transform.rotation, desiredRotation, Time.deltaTime * 3f);

            // Adjust FOV based on distance
            float distance = Vector3.Distance(PlayerRobot.tr.position, EnemyRobot.tr.position);
            float t2 = Mathf.InverseLerp(1f, 30f, distance);
            float targetFOV = Mathf.Lerp(40.0f, 60f, t2);
            if (Vector3.Distance(lookPosition, camera.transform.position) < 13.0f)
            {
                targetFOV = Mathf.Max(50, targetFOV);
            }

            if (Mathf.Abs((PlayerRobot.tr.position.y - EnemyRobot.tr.position.y)) > 5.0f)
            {
                targetFOV = Mathf.Max(60, targetFOV);
            }

            targetFOV = Mathf.Max(40, targetFOV);
            targetFOV = Mathf.Min(60, targetFOV);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFOV, Time.deltaTime * 5f);

            yield return null;
        }
    }


    private void StartCinematicVS()
    {
        currentCombatState = CombatSceneFlow.CinematicVS;
        UpdateUIPanels();
        StartCoroutine(VSAnimation());
        _soundManager.PlayMusic(_soundManager.musicLibrary[2]);
    }


    public void UpdateUIPanels()
    {
        panelEndCombat.SetActive(false);
        panelVSScreen.SetActive(false);
        panelCombat.SetActive(false);
        SetHealthBarVisibility(false);

        switch (currentCombatState)
        {
            case CombatSceneFlow.Start:
            {
                break;
            }
            case CombatSceneFlow.CinematicRingCircle:
            {
                break;
            }
            case CombatSceneFlow.CinematicVS:
            {
                panelVSScreen.SetActive(true);
                break;
            }
            case CombatSceneFlow.CinematicStartCountdown:
            {
                panelCombat.SetActive(true);
                break;
            }
            case CombatSceneFlow.Combat:
            {
                SetHealthBarVisibility(true);
                panelCombat.SetActive(true);
                break;
            }
            case CombatSceneFlow.EndCombatScreen:
            {
                panelEndCombat.SetActive(true);
                break;
            }
        }
    }

    private void SetHealthBarVisibility(bool active)
    {
        PlayerRobot.GetComponentInChildren<RobotHealth>().SetVisibility(active);
        EnemyRobot.GetComponentInChildren<RobotHealth>().SetVisibility(active);
    }

    // UI
    public void ContinueBtn()
    {
        _soundManager.PlayMusic(_soundManager.musicLibrary[0]);
        SceneManager.LoadScene("MainMenu");
    }
}