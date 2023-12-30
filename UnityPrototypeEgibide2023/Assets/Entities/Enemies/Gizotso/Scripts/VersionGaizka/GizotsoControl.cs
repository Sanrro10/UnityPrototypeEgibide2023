using System;
using System.Collections;
using UnityEngine;

public class GizotsoControl : MonoBehaviour
{
    public enum EstadoEnemigo
    {
        Idle,
        Walk,
        Attack,
        Death,
        Hurt,
        Stunned,
        Chase
    }

     EstadoEnemigo estadoActual ;

    public float velocidadCaminata = 2.0f;
    public float rangoDeteccion = 5.0f;
    public float rangoAtaque = 5.0f;
    public float tiempoCambioEstado = 3.0f;
    public float tiempoStunned = 2.0f;
    public float tiempoHurt = 1.0f;
    public float longitudRaycast = 5.0f;

    [Header("Límites de Movimiento")]
    public Transform limiteIzquierdo;
    public Transform limiteDerecho;
    [SerializeField]float posIzquiMax;
    [SerializeField] float posDereMax;
    [SerializeField] bool vistoPlayer;
    [SerializeField] Transform playerTransform;
    [SerializeField] float distanciaAPlayer;
    public Animator gizotsoAnimator;
    [SerializeField] Transform attackColliderTransform;
    private bool mirandoDerecha = true;
    [SerializeField] float duracionClipActual;

    private delegate void EstadoMetodo();
    private EstadoMetodo metodoEstadoActual;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 direccionRaycast = mirandoDerecha ? Vector2.left : Vector2.right;
        Gizmos.DrawRay(attackColliderTransform.position, direccionRaycast * longitudRaycast);
    }

    private void Start()
    {
        if (gizotsoAnimator == null)
        {
            Debug.LogError("El Animator no está asignado en el Inspector.");
        }
        posIzquiMax = limiteIzquierdo.position.x;
        posDereMax = limiteDerecho.position.x;
        // Inicializa el método del estado actual
        //InicializarMetodoEstado();
        CambiarEstado(EstadoEnemigo.Idle, Idle);
        Patrullar();
        // Comienza a alternar entre Idle y Walk después de un tiempo
        //Invoke("AlternarEstadoIdleWalk", 3.0f );
    }
    void Patrullar()
    {
        //if(estadoActual != EstadoEnemigo.Attack)
        //{
        //    InvokeRepeating("AlternarEstadoIdleWalk", 0.0f, tiempoCambioEstado);

        //}
        CambiarEstado(EstadoEnemigo.Walk, Caminar);
        StartCoroutine("AlternarEstadoIdleWalk");
    }
    private IEnumerator AlternarEstadoIdleWalk()
    {
        while (estadoActual != EstadoEnemigo.Attack || estadoActual != EstadoEnemigo.Chase)
        {
            if (estadoActual != EstadoEnemigo.Attack || estadoActual != EstadoEnemigo.Chase)
            {
                Debug.LogError("El estado en alternar es " + estadoActual);
                if (estadoActual == EstadoEnemigo.Idle)
                {
                    CambiarEstado(EstadoEnemigo.Walk, Caminar);
                }
                else if (estadoActual == EstadoEnemigo.Walk)
                {
                    CambiarEstado(EstadoEnemigo.Idle, Idle);
                }
                yield return new WaitForSeconds(tiempoCambioEstado);
            }
            else
            {
                CambiarEstado(EstadoEnemigo.Walk, Caminar);
            }
        }
        ////yield return null;
    }
    private void Perseguir()
    {
        StopAllCoroutines();
        StartCoroutine("Persiguiendo");
    }
    private IEnumerator Persiguiendo()
    {
        while (estadoActual == EstadoEnemigo.Chase)
        {
            if (distanciaAPlayer < rangoDeteccion)
            {
                //Debug.Log("Player transform - transform= " + Math.Abs(playerTransform.position.x - transform.position.x));
                // Mueve al enemigo hacia el jugador
                //CambiarEstado(EstadoEnemigo.Walk, Caminar);
                float direccion = playerTransform.position.x > transform.position.x ? 1.0f : -1.0f;
                transform.Translate(Vector2.right * direccion * velocidadCaminata * Time.deltaTime);

                // Cambia la dirección para que el enemigo siempre mire al jugador
                if (direccion > 0 && mirandoDerecha || direccion < 0 && !mirandoDerecha)
                {
                    CambiarDireccion();
                }
                yield return null;
            }
            else
            {
                Patrullar();
            }
        }
    }
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(attackColliderTransform.position, mirandoDerecha ? Vector2.left : Vector2.right, longitudRaycast);
        if (hit.collider != null) { Debug.LogError("Detectado con rayo " + hit.collider.tag); }
        if (playerTransform != null)
        {
            distanciaAPlayer = Math.Abs(playerTransform.position.x - transform.position.x);
        }
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            
            if (!vistoPlayer)
            {
                playerTransform = hit.collider.transform;
                posIzquiMax = hit.collider.transform.position.x;
                velocidadCaminata = 6;
                Debug.LogError("Detectado player");
                vistoPlayer = true;
                StopAllCoroutines();
                CambiarEstado(EstadoEnemigo.Chase, Perseguir);
            }
            //if (hit.distance <= rangoDeteccion)
            //{
            //    CambiarEstado(EstadoEnemigo.Chase, Perseguir);
            //}
            
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogError("Triger enter es " + other.tag);
        if (other.gameObject.layer == 6)
        {
            StopAllCoroutines();
            CambiarEstado(EstadoEnemigo.Attack, Atacar);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.LogError("Triger exit es " + other.gameObject.layer);
        if (other.gameObject.layer == 6)
        {
            Patrullar();
            vistoPlayer = false;
        }
        
    }

    private void Idle()
    {
        Debug.LogError("LLamado Idle");
        ActivarTrigger("Idle");
    }
    private void Caminar()
    {
        Debug.LogError("LLamado Caminar");
        ActivarTrigger("Walk");
        StartCoroutine("Caminando");
    }
    private IEnumerator Caminando()
    {
        Debug.LogError("LLamado Corutina Caminar");
       // CambiarDireccion();
        while (estadoActual == EstadoEnemigo.Walk)
        {
            float direccion = mirandoDerecha ? -1.0f : 1.0f;
            Debug.LogError("La direccion es = " + mirandoDerecha);
            velocidadCaminata = 2f;
            transform.Translate(Vector2.right * direccion * velocidadCaminata * Time.deltaTime);
            //Debug.Log("Pos deremax = " + posDereMax);
            //Debug.Log("Pos deremax = " + posIzquiMax);
            if (transform.position.x > posDereMax && !mirandoDerecha)
            {
                CambiarDireccion();
            }
            else if (transform.position.x < posIzquiMax && mirandoDerecha)
            {
                CambiarDireccion();
            }

            yield return null;
        }
    }

    private void CambiarDireccion()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void Atacar()
    {
        StopAllCoroutines();
        StartCoroutine("Atacando");
    }
    private IEnumerator Atacando()
    {
        ActivarTrigger("Attack");
        while (estadoActual == EstadoEnemigo.Attack)
        {
            if(distanciaAPlayer <= rangoAtaque)
            {
                
                if (gizotsoAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    // Obtener el clip de animación actual
                    AnimationClip clip = gizotsoAnimator.GetCurrentAnimatorClipInfo(0)[0].clip;

                    // Obtener la duración del clip de animación
                    duracionClipActual = clip.length;

                    // Hacer algo con la duración (por ejemplo, imprimir en la consola)
                    Debug.LogError("Duración de la animación actual: " + duracionClipActual);
                }
                //yield return new WaitForSeconds(duracionClipActual);
                //ActivarTrigger("Attack1");
                //ActivarTrigger("Attack2");
                yield return null;
            }
            else
            {
                Perseguir();
            }
        }
        
    }
    private void Morir()
    {
        ActivarTrigger("Death");
    }

    private void Herir()
    {
        Invoke("CambiarEstadoStunned", tiempoHurt);
        ActivarTrigger("Hurt");
    }

    private void Stun()
    {
        Invoke("CambiarEstadoIdle", tiempoStunned);
        ActivarTrigger("Stunned");
    }

    //private void CambiarEstadoIdle()
    //{
    //    CambiarEstado(EstadoEnemigo.Idle, Idle);
    //}

    private void CambiarEstadoStunned()
    {
        CambiarEstado(EstadoEnemigo.Stunned, Stun);
    }

    

    private void ActivarTrigger(string trigger)
    {
        if (gizotsoAnimator != null)
        {
            gizotsoAnimator.SetTrigger(trigger);
        }
    }

    private void CambiarEstado(EstadoEnemigo nuevoEstado, EstadoMetodo nuevoMetodo)
    {
        if (estadoActual != nuevoEstado)
        {
            estadoActual = nuevoEstado;
            metodoEstadoActual = nuevoMetodo;
            InicializarMetodoEstado();
        }
    }

    private void InicializarMetodoEstado()
    {
        // Inicializa el método del estado actual
        switch (estadoActual)
        {
            case EstadoEnemigo.Idle:
                metodoEstadoActual = Idle;
                break;
            case EstadoEnemigo.Walk:
                metodoEstadoActual = Caminar;
                break;
            case EstadoEnemigo.Attack:
                metodoEstadoActual = Atacar;
                break;
            case EstadoEnemigo.Death:
                metodoEstadoActual = Morir;
                break;
            case EstadoEnemigo.Hurt:
                metodoEstadoActual = Herir;
                break;
            case EstadoEnemigo.Stunned:
                metodoEstadoActual = Stun;
                break;
            case EstadoEnemigo.Chase:
                metodoEstadoActual = Perseguir;
                break;
        }
        metodoEstadoActual?.Invoke();
        Debug.LogError("Estado actual en inicializar es " + estadoActual + "Y SE HA INVOCADO " +   metodoEstadoActual);
    }

}
