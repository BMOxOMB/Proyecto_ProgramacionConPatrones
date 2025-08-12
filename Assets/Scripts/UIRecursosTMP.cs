using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class UIRecursosTMP : MonoBehaviour
{
    [Header("Refs UI")]
    [SerializeField] private TMP_Text textoOro;
    [SerializeField] private TMP_Text textoUnidades;
    [SerializeField] private TMP_Text textoEnemigos;
    [SerializeField] private TMP_Text textoOleada;

    [Header("Lógica")]
    [SerializeField] private OleadasManager oleadas;

    // Propiedades de solo lectura (opcional)
    public TMP_Text TextoOro => textoOro;
    public TMP_Text TextoUnidades => textoUnidades;
    public TMP_Text TextoEnemigos => textoEnemigos;
    public TMP_Text TextoOleada => textoOleada;
    public OleadasManager Oleadas => oleadas;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemigosVivosChange += ActualizarEnemigos;
            ActualizarEnemigos(GameManager.Instance.EnemigosVivos);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnEnemigosVivosChange -= ActualizarEnemigos;
    }

    private void Update()
    {
        if (GameManager.Instance != null && textoOro != null)
            textoOro.text = "Oro: " + GameManager.Instance.oro;

        if (textoUnidades != null)
            textoUnidades.text = "Unidades: " + UnidadMilitar.unidadesAliadas.Count;

        if (oleadas != null && textoOleada != null)
            textoOleada.text = "Oleada: " + oleadas.oleadaActual;
    }

    private void ActualizarEnemigos(int cantidad)
    {
        if (textoEnemigos != null)
            textoEnemigos.text = "Enemigos: " + cantidad;
    }
}
