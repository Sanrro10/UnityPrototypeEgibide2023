using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaltzControlParticulas : MonoBehaviour
{
    [SerializeField]
    ParticleSystem sistParticulas;
    [SerializeField]
    ParticleSystem sistParticulasMuerte;

    private void Start()
    {
        sistParticulas.Stop();
        sistParticulasMuerte.Stop();

    }
    public void LanzarParticulasPolvo()
    {
        sistParticulas.Play();

    } public void LanzarParticulasMuerte()
    {
        sistParticulasMuerte.Play();

    }
}
