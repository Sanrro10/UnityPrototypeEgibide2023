using UnityEngine;

public class RotarSkybox : MonoBehaviour
{
    public float velocidadRotacion = 0.5f; // Velocidad de rotación del skybox

    private void Update()
    {
        // Obtener el material del skybox
        Material skyboxMaterial = RenderSettings.skybox;

        // Obtener el ángulo de rotación actual
        float rotation = skyboxMaterial.GetFloat("_Rotation");

        // Aumentar gradualmente el ángulo de rotación en el tiempo
        rotation += velocidadRotacion * Time.deltaTime;

        // Aplicar la rotación al material del skybox
        skyboxMaterial.SetFloat("_Rotation", rotation);
    }
}
