using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintsGenerator : MonoBehaviour
{
    public GameObject car;
    public RenderTexture textureToCopyFormat;
    public GameObject NavAgent;
    public LayerMask obstacleLayerMask;
    public BoxMessage boxMessage;

    private int hint = 0;
    private float previousDistance;
    private float timeSinceLastDecrease;
    private Canvas canvas;
    private Button hintButton;
    private RawImage image;
    private Image farther;
    private Image closer;
    private Image distance;
    private Image azimuth;
    private Image description;
    private GameObject box;
    private float currentDistance;

    private int rank;
    private float timer;
    private bool firstBox = true;

    private int totalHintsCount = 0;

    void Start()
    {
        SetVariables();
        hintButton.gameObject.SetActive(false);

        Button buttonComponent = hintButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(HintButtonClicked);
    }

    void FixedUpdate()
    {
        if (box != null)
        {
            currentDistance = Vector3.Distance(car.transform.position, box.transform.position);
            if ((int)currentDistance >= (int)previousDistance)
            {
                timeSinceLastDecrease += Time.deltaTime;
                if (timeSinceLastDecrease >= timer && hint < 6)
                {

                    hintButton.gameObject.SetActive(true);

                }
            }
            else
            {
                timeSinceLastDecrease = 0f;
            }

            if (hint == 3 || hint == 4)
            {
                ShowHint();
            }

            previousDistance = currentDistance;
        }
    }

    private void SetVariables()
    {
        canvas = FindObjectOfType<Canvas>();
        description = canvas.transform.Find("Description").GetComponent<Image>();
        hintButton = canvas.transform.Find("HintButton").GetComponent<Button>();
        image = canvas.transform.Find("HintImage").GetComponent<RawImage>();
        farther = canvas.transform.Find("Farther").GetComponent<Image>();
        closer = canvas.transform.Find("Closer").GetComponent<Image>();
        distance = canvas.transform.Find("Distance").GetComponent<Image>();
        azimuth = canvas.transform.Find("Azimuth").GetComponent<Image>();
    }

    private void ShowHint()
    {
        switch (hint)
        {
            case 1:
                Vector3 origin = new Vector3(box.transform.position.x, 400f, box.transform.position.y);

                RaycastHit hit;
                if (Physics.Raycast(origin, Vector3.down, out hit, 60f, obstacleLayerMask))
                {
                    string tag = hit.collider.gameObject.transform.tag;
                    float height = hit.collider.gameObject.transform.position.y;

                    string message;

                    if (height < 25)
                    {
                        message = "Caixa está numa zona baixa, perto de ";
                    }
                    else
                    {
                        message = "Caixa está numa zona alta, perto de ";
                    }

                    switch (tag)
                    {
                        case "Beach":
                            message += "uma praia";
                            break;
                        case "WaterSquare":
                            message += "água";
                            break;
                        case "Mountain":
                            message += "montanhas";
                            break;
                        case "Trees":
                            message += "árvores";
                            break;
                        case "Orchard":
                            message += "um pomar";
                            break;
                        default:
                            break;
                    }

                    TextMeshProUGUI descriptionText = description.GetComponentInChildren<TextMeshProUGUI>();
                    descriptionText.text = message;
                    description.gameObject.SetActive(true);
                    Debug.Log(message);
                }

                break;
            case 2:
                GameObject cameraObject = new("HintCamera");
                Camera cameraComponent = cameraObject.AddComponent<Camera>();
                cameraComponent.transform.position = new Vector3(box.transform.position.x, box.transform.position.y + 200, box.transform.position.z);
                cameraComponent.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

                description.gameObject.SetActive(false);
                RenderTexture texture = new(textureToCopyFormat);
                cameraComponent.targetTexture = texture;
                image.texture = texture;
                image.gameObject.SetActive(true);
                break;
            case 3:
                image.gameObject.SetActive(false);

                if (currentDistance > previousDistance && Mathf.Abs(currentDistance - previousDistance) > 0.01)
                {
                    closer.gameObject.SetActive(false);
                    farther.gameObject.SetActive(true);
                }
                else
                {
                    closer.gameObject.SetActive(true);
                    farther.gameObject.SetActive(false);
                }

                break;
            case 4:
                closer.gameObject.SetActive(false);
                farther.gameObject.SetActive(false);
                TextMeshProUGUI distanceText = distance.GetComponentInChildren<TextMeshProUGUI>();
                distanceText.text = "Distance: " + (Mathf.RoundToInt(currentDistance)).ToString();
                TextMeshProUGUI azimuthText = azimuth.GetComponentInChildren<TextMeshProUGUI>();
                Vector3 direction = box.transform.position - car.transform.position;
                direction = car.transform.InverseTransformDirection(direction);
                float azimuthValue = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                if (azimuthValue < 0f)
                {
                    azimuthValue += 360f;
                }
                azimuthText.text = "Azimuth: " + (Mathf.RoundToInt(azimuthValue)).ToString();
                azimuth.gameObject.SetActive(true);
                distance.gameObject.SetActive(true);
                break;
            case 5:
                azimuth.gameObject.SetActive(false);
                distance.gameObject.SetActive(false);
                SetNavAgentToObject nav = gameObject.AddComponent<SetNavAgentToObject>();
                nav.destination = box;
                nav.car = car;
                nav.NavAgent = NavAgent;
                nav.SpawnAgent();
                break;
            default:
                break;
        }
    }

    private void HintButtonClicked()
    {
        hintButton.gameObject.SetActive(false);
        if (hint < 6)
        {
            hint += 1;
            totalHintsCount += 1;
        }
        ShowHint();
        timeSinceLastDecrease = 0f;
    }

    public void SetBox(GameObject box)
    {
        SetVariables();
        this.box = box;
        previousDistance = Vector3.Distance(car.transform.position, box.transform.position);
        timeSinceLastDecrease = 0f;

        SetRank();
        CleanCanvas();
        hint = 0;
    }

    public int GetTotalHintsCount()
    {
        return totalHintsCount;
    }

    public int GetRank()
    {
        return rank;
    }

    public void CleanCanvas()
    {
        hintButton.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        farther.gameObject.SetActive(false);
        closer.gameObject.SetActive(false);
        distance.gameObject.SetActive(false);
        azimuth.gameObject.SetActive(false);
        description.gameObject.SetActive(false);

    }

    private void SetRank()
    {
        int totalBoxCount = boxMessage.GetTotalBoxCount();
        if (totalBoxCount == 0)
        {
            if (firstBox)
            {
                rank = 3;
                firstBox = false;
            }
            else
            {
                rank = 0;
            }
        }
        else
        {
            rank = 5 - Mathf.RoundToInt(totalHintsCount / totalBoxCount);
        }
        timer = 60 / (rank + 1);
    }
}