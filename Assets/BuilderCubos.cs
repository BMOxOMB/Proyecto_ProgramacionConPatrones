using UnityEngine;
/*
 *El script que maneja la construccion de los cubos, llamo a la interfaz de figuras con el metodo de spawn
 * abc
 */

public class BuilderCubos : IFigurasInterfaz
{
    public GameObject prefab; //El cubo desde el edito
    public float tamanhoMinimo = 200f, tamanhoMaximo = 500f; //Las dimensiones minimas del cubo
    private IColisionConfiguracion configuracionObstaculo;
    
    
    public BuilderCubos(GameObject cuboPrefab, float tamanhoMinimo, float tamanhoMaximo, IColisionConfiguracion configurador) //Constructor
    {
        this.prefab = cuboPrefab;
        this.tamanhoMinimo = tamanhoMinimo;
        this.tamanhoMaximo = tamanhoMaximo;
        this.configuracionObstaculo = configurador;
    }
    
    public void FigurasSpawn(Vector3 PosicionInicial, Transform parent = null) //Llamado al metodo de la interfaz
    {       
        //Generando las dimensiones aleatorias de los cubos
            float anchoCubo = Random.Range(tamanhoMinimo, tamanhoMaximo);
            float altoCubo = Random.Range(tamanhoMinimo, tamanhoMaximo);
            float fondoCubo = Random.Range(tamanhoMinimo, tamanhoMaximo);
        //Generando la instancia de la figura 
            GameObject nuevoCubo = Object.Instantiate(prefab, parent);
            //Cambiando las dimensiones por los randoms y poniendo las posiciones en el suelo
            nuevoCubo.transform.localScale = new Vector3(anchoCubo, altoCubo, fondoCubo);
            nuevoCubo.transform.position = new Vector3(PosicionInicial.x, PosicionInicial.y, PosicionInicial.z);
            
            configuracionObstaculo.Configurar(nuevoCubo);//Llamo a la interfaz con la figura correspondiente a este script
        }
    }