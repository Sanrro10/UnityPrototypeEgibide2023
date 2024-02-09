using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaltzControlParticulas : MonoBehaviour
{
    // Referencia a las partículas de andar
    [SerializeField] ParticleSystem sistParticulas;
    
    // Referencia a las partículas de muerte
    [SerializeField] ParticleSystem sistParticulasMuerte;

    // Por deferco las partículas están paradas
    private void Start()
    {
        sistParticulas.Stop();
        sistParticulasMuerte.Stop();

    }
    
    // Metodo para llamar a las partículas de andar
    public void LanzarParticulasPolvo()
    {
        sistParticulas.Play();
    } 
    
    // Metodo para llamar a las particulas de muerte
    public void LanzarParticulasMuerte()
    {
        sistParticulasMuerte.Play();
    }
}
