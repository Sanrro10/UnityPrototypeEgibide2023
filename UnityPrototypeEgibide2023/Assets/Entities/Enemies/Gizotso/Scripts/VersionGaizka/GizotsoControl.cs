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
        Chase,
        Dash
    }
    // Datos del enemigo
    [Header("Datos del enemigo")]
    [SerializeField] private Entities.PassiveEnemyData passiveEnemyData;
    public EstadoEnemigo estadoActual ;
    [Header("Valores de Movimiento")]
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
    [Header("Relativo al player")]
    [SerializeField] private RaycastHit2D hit;
    [SerializeField] bool vistoPlayer = false;
    [SerializeField] Transform playerTransform;
    [SerializeField] float distanciaAPlayer;
    public Animator gizotsoAnimator;
    [SerializeField] public Transform origenRayo;
    public bool mirandoDerecha = true;
    [SerializeField] float duracionClipActual;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float fuerzaSalto;
    private delegate void EstadoMetodo();
    private EstadoMetodo metodoEstadoActual;

    [Header("Dash")]
    public float velocidadRodar = 2.0f;
    public float distanciaLateralDash = 5.0f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 direccionRaycast = mirandoDerecha ? Vector2.left : Vector2.right;
        Gizmos.DrawRay(origenRayo.position, direccionRaycast * longitudRaycast);
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
        StopAllCoroutines();

        CambiarEstado(EstadoEnemigo.Idle, Idle);
        StartCoroutine("AlternarEstadoIdleWalk");
    }
    private IEnumerator AlternarEstadoIdleWalk()
    {
        while (estadoActual != EstadoEnemigo.Attack || estadoActual != EstadoEnemigo.Chase)
        {
            if (estadoActual != EstadoEnemigo.Attack || estadoActual != EstadoEnemigo.Chase|| estadoActual!= EstadoEnemigo.Dash)
            {
                Debug.LogError("El estado actual en alternar es " + estadoActual);
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
        yield return new WaitForSeconds(0.3f);
        ActivarTrigger("Walk");
        while (estadoActual == EstadoEnemigo.Chase)
        {
            if (playerTransform != null)
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
        //Debug.LogError("Estado actual es + " + estadoActual);
        if (Input.GetKeyDown(KeyCode.U))
        {
            CambiarEstado(EstadoEnemigo.Dash, RodarDash);
        } if (Input.GetKeyDown(KeyCode.O))
        {
            CambiarEstado(EstadoEnemigo.Death, Morir);
        }
             hit = Physics2D.Raycast(origenRayo.position, mirandoDerecha ? Vector2.left : Vector2.right, longitudRaycast);
        //if (hit.collider != null) { Debug.LogError("Detectado con rayo " + hit.collider.tag); }
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
        if (!other.gameObject.CompareTag("Player")) return;
        if (other.gameObject.layer == 6)
        {
            if (estadoActual != EstadoEnemigo.Dash)
            {
                StopAllCoroutines();
                CambiarEstado(EstadoEnemigo.Attack, Atacar);
                Debug.LogError("Triger enter llama a atacar ");

            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.LogError("Triger exit es " + other.tag);
        if (!other.gameObject.CompareTag("Player")) return;
        if (other.gameObject.layer == 6)
        {
            if (estadoActual != EstadoEnemigo.Dash)
            {
                if (distanciaAPlayer <= rangoDeteccion)
                {
                    if (distanciaAPlayer >= rangoAtaque)
                    {
                        CambiarEstado(EstadoEnemigo.Chase, Perseguir);
                    }
                    else
                    {
                        CambiarEstado(EstadoEnemigo.Attack, Atacar);
                    }

                }
                else
                {
                    Patrullar();
                    vistoPlayer = false;
                }
            }
        }
        
    }

    private void Idle()
    {
        // Debug.LogError("LLamado Idle");
       // StopAllCoroutines();
        ActivarTrigger("Idle");
    }
    private void Caminar()
    {
        //Debug.LogError("LLamado Caminar");
        //StopAllCoroutines();
        ActivarTrigger("Walk");
        StartCoroutine("Caminando");
    }
    private IEnumerator Caminando()
    {
        //Debug.LogError("LLamado Corutina Caminar");
       // CambiarDireccion();
        while (estadoActual == EstadoEnemigo.Walk)
        {
            float direccion = mirandoDerecha ? -1.0f : 1.0f;
            //Debug.LogError("La direccion es = " + mirandoDerecha);
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
            if (distanciaAPlayer <= rangoAtaque)
            {
                // Debug.LogError("distanciaAPlayer es " + distanciaAPlayer + " rangoAtaque es " + rangoAtaque); ;
                if (gizotsoAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    // Obtener el clip de animación actual
                    AnimationClip clip = gizotsoAnimator.GetCurrentAnimatorClipInfo(0)[0].clip;

                    // Obtener la duración del clip de animación
                    duracionClipActual = clip.length;

                    // Hacer algo con la duración (por ejemplo, imprimir en la consola)
                    Debug.LogError("Duración de la animación actual: " + duracionClipActual);
                }
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    CambiarEstado(EstadoEnemigo.Chase, Perseguir);
                    Debug.LogError("Hit collider es !=player de attack debería cambiar a Perseguir");


                }
                /*float direccion = playerTransform.position.x > transform.position.x ? 1.0f : -1.0f;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), gameObject.layer,true);
                rb.AddForce(Vector2.left*direccion * fuerzaSalto, ForceMode2D.Impulse);
                //rb.AddForce(new Vector2(transform.position.x*direccion * fuerzaSalto, transform.position.y * fuerzaSalto), ForceMode2D.Impulse);
                //yield return new WaitForSeconds(duracionClipActual);
                //ActivarTrigger("Attack1");
                //ActivarTrigger("Attack2");
                */

                //yield return new WaitForSeconds(2f);
                //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), gameObject.layer, false);

            }
            else
            {
                Debug.LogError("Distancia a player = " + (distanciaAPlayer <= rangoDeteccion));
                if (distanciaAPlayer <= rangoDeteccion)
                {
                    CambiarEstado(EstadoEnemigo.Chase, Perseguir);
                    Debug.LogError("Entrado en distanciaAPlayer <= rangoDeteccion de attack debería cambiar a Perseguir");
                }
                else
                {
                    Patrullar();
                    Debug.LogError("Entrado en patrullar de attack debería cambiar a patrullar");

                }
                //Debug.LogError("Entrado en patrullar de attack debería cambiar a patrullar");
            }
            //CambiarEstado(EstadoEnemigo.Chase, Perseguir);
            yield return null;
        }
      
    }
    private void Morir()
    {
        StopAllCoroutines();
        ActivarTrigger("Death");
        Invoke(nameof(DestroyThis), 3f);

    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
    private void Herir()
    {
        //Invoke("CambiarEstadoStunned", tiempoHurt);
        //ActivarTrigger("Hurt");
        CambiarEstado(EstadoEnemigo.Dash, RodarDash);
    }
    private void RodarDash()
    {
        StopAllCoroutines();
        StartCoroutine(RodarDashCoroutine());
    }
    private IEnumerator RodarDashCoroutine()
    {
        // Activar el trigger para la animación de rodar
        ActivarTrigger("Rodar");

        // Desactivar temporalmente las colisiones con el jugador
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), gameObject.layer, true);
        float direccion = mirandoDerecha ? -1.0f : 1.0f;
        // Calcular la nueva posición después de rodar
        Vector3 nuevaPosicion = transform.position +  new Vector3(distanciaLateralDash* direccion, 0f, 0f);
        Debug.LogError("Nueva posicion para dash = " + nuevaPosicion);
        // Moverse mientras rueda (puedes ajustar la lógica según tus necesidades)
        while ((direccion > 0 && transform.position.x < nuevaPosicion.x) ||
               (direccion < 0 && transform.position.x > nuevaPosicion.x))
        {
            float distanciaPorFrame = velocidadRodar * Time.deltaTime;
            transform.Translate(Vector3.right*direccion * distanciaPorFrame);
            yield return null;
        }

        // Posicionar al enemigo al otro lado del jugador
        transform.position = nuevaPosicion;

        // Activar las colisiones con el jugador nuevamente
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), gameObject.layer, false);

        // Cambiar a otro estado (si es necesario) después del dash
        if (playerTransform != null)
        {
            if(distanciaAPlayer <= rangoDeteccion)
            {
                CambiarEstado(EstadoEnemigo.Chase, Perseguir);
            }
        }
        else
        {
            Patrullar();
        }
        
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
            case EstadoEnemigo.Dash:
                metodoEstadoActual = RodarDash;
                break;
        }
        metodoEstadoActual?.Invoke();
        Debug.LogError("Estado actual en inicializar es " + estadoActual + "Y SE HA INVOCADO " +   metodoEstadoActual);
    }

}
