using UnityEngine;
using UnityEngine.UI;

public class DiceController : MonoBehaviour
{
    public float throwForce = 10f;
    public float torqueForce = 10f;
    private Rigidbody rb;
    private bool isRolling = false;
    private bool isStopped = false;

    public Text resultText;

    // Значения сторон кубика
    public int[] sideValues = { 1, 2, 3, 4, 5, 6 };

    private int landedSideIndex = -1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from this game object.");
        }

        if (resultText == null)
        {
            Debug.LogError("Result Text is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isRolling)
        {
            ThrowDice();
        }

        if (isRolling && !isStopped)
        {
            if (IsDiceStopped())
            {
                isStopped = true;
                DisplayResult();
                isRolling = false;  // Разрешаем повторное подбрасывание
            }
        }
    }

    void ThrowDice()
    {
        isRolling = true;
        isStopped = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 throwDirection = new Vector3(
            Random.Range(-0.5f, 0.5f),
            1f,
            Random.Range(-0.5f, 0.5f)
        ).normalized;

        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * torqueForce, ForceMode.Impulse);
    }

    bool IsDiceStopped()
    {
        // Проверяем, остановился ли кубик
        return rb.velocity.magnitude < 0.1f && rb.angularVelocity.magnitude < 0.1f;
    }

    void DisplayResult()
    {
        if (landedSideIndex >= 0 && landedSideIndex < sideValues.Length)
        {
            int sideValue = sideValues[landedSideIndex];

            if (resultText != null)
            {
                resultText.text = "Dice landed on: " + sideValue;
            }
            else
            {
                Debug.Log("Result Text component is missing.");
            }
        }
        else
        {
            Debug.LogError("Invalid side index: " + landedSideIndex);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Surface"))
        {
            Vector3 normal = collision.contacts[0].normal;
            Debug.Log("Collision normal: " + normal);

            int sideIndex = GetSideIndex(normal);

            if (sideIndex >= 0 && sideIndex < sideValues.Length)
            {
                landedSideIndex = sideIndex;
                Debug.Log("Landed side index: " + landedSideIndex);
            }
            else
            {
                Debug.LogError("Invalid side index from collision normal.");
            }
        }
    }

    int GetSideIndex(Vector3 normal)
    {
        Vector3[] faceNormals = new Vector3[]
        {
            Vector3.up,      // Top
            Vector3.down,    // Bottom
            Vector3.left,    // Left
            Vector3.right,   // Right
            Vector3.forward, // Front
            Vector3.back     // Back
        };

        int sideIndex = 0;
        float maxDot = float.NegativeInfinity;

        for (int i = 0; i < faceNormals.Length; i++)
        {
            float dot = Vector3.Dot(normal, faceNormals[i]);
            if (dot > maxDot)
            {
                maxDot = dot;
                sideIndex = i;
            }
        }

        return sideIndex;
    }
}
