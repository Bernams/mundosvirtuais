using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this for UI components
using TMPro;

public class HintsGenerator : MonoBehaviour
{
    public float timer = 10f;
    public GameObject car; // The car object
    public RenderTexture textureToCopyFormat;
    public GameObject NavAgent;

    //private GameObject box;
    private bool wantsHint = false;
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
    private GameObject box;
    private float currentDistance;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        hintButton = canvas.transform.Find("HintButton").GetComponent<Button>();
        hintButton.gameObject.SetActive(false); //Começar com o botão desativado
        previousDistance = Vector3.Distance(car.transform.position, box.transform.position); // Guardar a primeira distancia entre o carro e a caixa
        timeSinceLastDecrease = 0f;

        // Add a click event listener to the button
        Button buttonComponent = hintButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(HintButtonClicked);
    }

    void FixedUpdate()
    {
        currentDistance = Vector3.Distance(car.transform.position, box.transform.position);
        if ((int)currentDistance >= (int)previousDistance)
        { 
            timeSinceLastDecrease += Time.deltaTime;
            if (timeSinceLastDecrease >= timer)
            {
                if (wantsHint)
                {
                   
                    ShowHint();
                    timeSinceLastDecrease = 0f;
                    
                   
                }
                else
                {
                    hintButton.gameObject.SetActive(true);
                }
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

    private void ShowHint()
    {
        switch (hint)
        {
            case 1:
                //dica 1
                wantsHint = false;
                break;
            case 2:
                GameObject cameraObject = new GameObject("HintCamera");
                Camera cameraComponent = cameraObject.AddComponent<Camera>();
                cameraComponent.transform.position = new Vector3(box.transform.position.x, box.transform.position.y + 200, box.transform.position.z);
                cameraComponent.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                RenderTexture texture = new RenderTexture(textureToCopyFormat);
                cameraComponent.targetTexture = texture;
                image = canvas.transform.Find("HintImage").GetComponent<RawImage>();
                image.texture = texture;
                image.gameObject.SetActive(true);
                wantsHint = false;
                break;
            case 3:
                image.gameObject.SetActive(false);
                farther = canvas.transform.Find("Farther").GetComponent<Image>();
                closer = canvas.transform.Find("Closer").GetComponent<Image>();

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

                wantsHint = false;
                //dica 3
                break;
            case 4:
                closer.gameObject.SetActive(false);
                farther.gameObject.SetActive(false);
                distance = canvas.transform.Find("Distance").GetComponent<Image>();
                azimuth = canvas.transform.Find("Azimuth").GetComponent<Image>();
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
                wantsHint = false;
                //dica 4
                break;
            case 5:
                azimuth.gameObject.SetActive(false);
                distance.gameObject.SetActive(false);
                SetNavAgentToObject nav = gameObject.AddComponent<SetNavAgentToObject>();
                nav.destination = box;
                nav.car = car;
                nav.NavAgent = NavAgent;
                nav.SpawnAgent();
                wantsHint = false;
                //dica 5
                break;
            default:
                break;
        }
    }

    private void HintButtonClicked()
    { 
        hintButton.gameObject.SetActive(false);
        wantsHint = true;
        if (hint < 5)
        {
            hint += 1;
        }
    }

    public void setBox(GameObject box)
    {
        this.box = box;
    }
}