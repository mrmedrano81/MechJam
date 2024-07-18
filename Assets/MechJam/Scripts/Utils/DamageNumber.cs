using UnityEngine;
using TMPro;
using System.Collections; // Assuming you're using TextMeshPro for the text display

public class DamageNumber : MonoBehaviour
{
    public float duration = 2f; // Duration for the damage number to stay visible
    public float floatSpeed = 2f; // Speed at which the damage number floats upwards
    public float fadeSpeed = 2f; // Speed at which the damage number fades out

    private TextMeshProUGUI textMesh;
    private Color originalColor;

    void Start()
    {
        // Get the TextMeshProUGUI component
        textMesh = GetComponent<TextMeshProUGUI>();
        originalColor = textMesh.color;

        // Start the floating and fading coroutine
        StartCoroutine(FadeOutAndDestroy());
    }

    // Set the damage amount to be displayed
    public void SetDamage(float damage)
    {
        textMesh.text = damage.ToString();
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Float the damage number upwards
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;

            // Fade out the damage number
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy the damage number object after the duration
        Destroy(gameObject);
    }
}
