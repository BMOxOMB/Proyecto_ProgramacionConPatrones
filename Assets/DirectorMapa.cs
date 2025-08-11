using UnityEngine;
// Clase encargada de preparar y registrar los constructores de figuras al inicio del juego
public class DirectorMapa : MonoBehaviour
{

    [Header("Referencias")]
    // Referencia al objeto encargado de generar figuras en la escena
    public BuilderFiguras spawnerFiguras;

    [Header("Prefabs")]
    public GameObject prefabCubo; // Prefab del cubo que será usado por el BuilderCubos
    public GameObject prefabCilindro;// Prefab del cilindro que será usado por el BuilderCilindro
    
    // Método que se ejecuta antes de Start
    void Awake()
    {
        var configuracionObstaculo = new ColisionObConfig();
        // Crea un builder para las figuras, usando el prefab y el rango de tamaño
        var builderCubo = new BuilderCubos(prefabCubo, 200f, 500f,configuracionObstaculo);
        var builderCilindro = new BuilderCilindro(prefabCilindro, 150f, 300f,configuracionObstaculo);
        // Registra el builder de cubos en el sistema de generación de figuras
        spawnerFiguras.RegisterBuilder(builderCubo);
        // Registra el builder de cilindros también
        spawnerFiguras.RegisterBuilder(builderCilindro);
        
    }

}