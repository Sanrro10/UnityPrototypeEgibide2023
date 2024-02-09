using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEvents : MonoBehaviour
{

    public ParticleSystem walkParticles;
    public ParticleSystem dashParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PldaayDashParticles()
    {
        dashParticles.Play();
    }

    public void PlayWalkParticles()
    {
        walkParticles.Play();
    }


}
