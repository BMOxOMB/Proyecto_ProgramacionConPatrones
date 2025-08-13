using UnityEngine;
using TMPro;

public class UIRecursosTMP : MonoBehaviour
{
    public TMP_Text textoOro;
    public TMP_Text textoUnidades;
    public TMP_Text textoEnemigos;
    // public OleadasManager oleadas;
    // public TMP_Text textoOleada;
    void Update()
    {
        textoOro.text = "Oro: " + GameManager.Instance.oro;

        int cantidadU = GameObject.FindObjectsOfType<UnidadMilitar>().Length;
        textoUnidades.text = "Unidades: " + cantidadU;

        int cantidadE = GameObject.FindObjectsOfType<EnemigoIA>().Length;
        textoEnemigos.text = "Enemigos: " + cantidadE;
        
        // textoOleada.text = "Oleada: " + oleadas.oleadaActual;
    }
    
}