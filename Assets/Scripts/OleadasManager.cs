using UnityEngine;

public class OleadasManager : MonoBehaviour
{
    public GameObject prefabEnemigo;
    public Transform[] puntosSpawn;

    public float tiempoEntreOleadas = 10f;
    public int cantidadInicial = 2;

    private float temporizador;
    public int oleadaActual = 1;

    void Start()
    {
        temporizador = tiempoEntreOleadas;
    }

    void Update()
    {
        temporizador -= Time.deltaTime;

        if (temporizador <= 0f)
        {
            IniciarOleada();
            oleadaActual++;
            temporizador = tiempoEntreOleadas;
        }
    }

    void IniciarOleada()
    {
        int cantidadEnemigos = cantidadInicial + oleadaActual;

        Debug.Log("Iniciando Oleada #" + oleadaActual + " con " + cantidadEnemigos + " enemigos.");

        for (int i = 0; i < cantidadEnemigos; i++)
        {
            Transform punto = puntosSpawn[Random.Range(0, puntosSpawn.Length)];
            GameObject clon = Instantiate(prefabEnemigo, punto.position, Quaternion.identity);

            GameManager.Instance.RegisterEnemy();

            // Configurar IA del enemigo si es necesario
            EnemigoIA ia = clon.GetComponent<EnemigoIA>();
            if (ia != null)
            {
                ia.SetEstrategiaCombate(new EstrategiaMelee());
            }
        }
        if (oleadaActual > 5)
        {
            GameManager.Instance.Victoria();
        }

    }
}