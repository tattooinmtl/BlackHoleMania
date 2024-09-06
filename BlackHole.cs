using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [Header("Black Hole Properties")]
    [SerializeField] private float attractionForce = 1000f;
    [SerializeField] private float eventHorizonRadius = 5f;
    [SerializeField] private float singularityRadius = 0.5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 60f;
    [SerializeField] public float baseDamage = 10f;

    [Header("Growth Settings")]
    [SerializeField] private int maxUpgradeLevel = 32;
    [SerializeField] private float maxSize = 320f;
    [SerializeField] private float baseGrowRate = 1f;
    [SerializeField] private float growRateFactor = 0.1f;
    [SerializeField] private int pointsToGrow = 100;

    private float initialSize;
    private float targetSize;
    private float growthStartTime;
    private float growRate;

    [Header("Game Stats")]
    public int points = 0;
    public int objectsEaten = 0;
    public int upgradeCount = 0;
    public string lastEatenObjectName = "";

    [Header("Effects")]
    public ParticleSystem eatEffect;
    public AudioClip eatSound;

    private AudioSource audioSource;
    private Vector3 targetPosition;

    public static BlackHole Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        targetPosition = transform.position;
        initialSize = transform.localScale.x;
        targetSize = initialSize;
        growRate = baseGrowRate;
    }

    private void FixedUpdate()
    {
        AttractObjects();
    }

    private void Update()
    {
        HandleInput();
        MoveBlackHole();
        RotateBlackHole();

        // Smoothly grow towards the target size
        if (transform.localScale.x < targetSize)
        {
            float growthProgress = (Time.time - growthStartTime) / growRate;
            float newScale = Mathf.Lerp(initialSize, targetSize, growthProgress);
            transform.localScale = new Vector3(newScale, newScale, newScale);

            // Update other properties based on new scale
            eventHorizonRadius = newScale / 2f;
            singularityRadius = eventHorizonRadius * 0.1f;
            attractionForce *= (1 + (growRate * Time.deltaTime) / initialSize);
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                targetPosition.y = transform.position.y;
            }
        }
    }

    void MoveBlackHole()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void RotateBlackHole()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void AttractObjects()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, eventHorizonRadius);

        foreach (Collider obj in nearbyObjects)
        {
            if (obj.gameObject != gameObject && obj.CompareTag("Eatable"))
            {
                EatableObject eatableObj = obj.GetComponent<EatableObject>();
                if (eatableObj != null)
                {
                    Vector3 direction = transform.position - obj.transform.position;
                    float distance = direction.magnitude;

                    float forceMagnitude = attractionForce / (distance * distance);
                    Vector3 force = direction.normalized * forceMagnitude;

                    Rigidbody rb = obj.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddForce(force, ForceMode.Force);
                    }

                    float damageAmount = baseDamage * Time.fixedDeltaTime;
                    eatableObj.TakeDamage(damageAmount);
                    Debug.Log(obj.name + " took damage: " + damageAmount + ", current HP: " + eatableObj.GetCurrentHP());

                    if (eatableObj.IsDead() || distance <= singularityRadius)
                    {
                        EatObject(obj.gameObject);
                    }
                }
            }
        }
    }

    void EatObject(GameObject obj)
    {
        string objTag = obj.tag;
        int pointValue = TagController.Instance.GetValueForTag(objTag);
        points += pointValue;
        objectsEaten++;
        lastEatenObjectName = obj.name;

        TagController.Instance.IncrementTagCount(objTag);

        if (eatEffect != null)
        {
            Instantiate(eatEffect, obj.transform.position, Quaternion.identity);
        }

        if (eatSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(eatSound);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(pointValue);
        }

        if (GameController.Instance != null)
        {
            GameController.Instance.ObjectEaten(pointValue);
        }

        SlowEffect slowEffect = obj.GetComponent<SlowEffect>();
        if (slowEffect != null)
        {
            slowEffect.ApplySlow();
        }

        if (UIController.Instance != null)
        {
            UIController.Instance.UpdateBlackHoleStats(points, transform.localScale.x, lastEatenObjectName);
        }

        Destroy(obj);

        CheckForGrowth();
    }

    void CheckForGrowth()
    {
        if (GameController.Instance.score >= pointsToGrow && upgradeCount < maxUpgradeLevel)
        {
            StartGrowing();
            GameController.Instance.score -= pointsToGrow;
        }
    }

    void StartGrowing()
    {
        upgradeCount++;
        initialSize = transform.localScale.x;
        targetSize = initialSize + ((maxSize - initialSize) / maxUpgradeLevel);

        growthStartTime = Time.time;
        growRate = baseGrowRate + ((GameController.Instance.score - pointsToGrow) * growRateFactor);

        if (UIController.Instance != null)
        {
            UIController.Instance.UpdateBlackHoleStats(points, transform.localScale.x, lastEatenObjectName);
        }
    }

    public void ForceUpgrade()
    {
        StartGrowing();
    }
}