using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEvents : MonoBehaviour
{

    public ParticleSystem walkParticles;
    public ParticleSystem dashParticles;
    public ParticleSystem jumpParticles;
    

    public void PlayDashParticles()
    {
        dashParticles.Play();
    }

    public void PlayWalkParticles()
    {
        walkParticles.Play();
    }

    public void PlayJumpParticles()
    {
        jumpParticles.Play();
    }


}
