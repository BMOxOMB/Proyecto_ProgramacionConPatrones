using UnityEngine;
/*
 *El script que maneja la construccion de los cilindros, llamo a la interfaz de figuras con el metodo de spawn
 *abc
 */
public class BuilderCilindro : IFigurasInterfaz
{

    public GameObject prefab;//El cilindro desde el editor
    public float tamanhoMinimo = 150f, tamanhoMaximo = 300f; //Las dimensiones minimas del editor
    private IColisionConfiguracion configuracionObstaculo;

    public BuilderCilindro(GameObject prefab, float tamanhoMinimo, float tamanhoMaximo, IColisionConfiguracion configurador)//Constructor
    {
        this.prefab = prefab;
        this.tamanhoMinimo = tamanhoMinimo;
        this.tamanhoMaximo = tamanhoMaximo;
        this.configuracionObstaculo = configurador;
    }

    public void FigurasSpawn(Vector3 PosicionInicial, Transform parent = null)//Llamado al metodo de la interfaz
    {
        //Generando las dimensiones aleatorias de los cubos
        float radio = Random.Range(tamanhoMinimo, tamanhoMaximo);
        float alto = Random.Range(tamanhoMinimo, tamanhoMaximo);
        //Generando la instancia de la figura 
        GameObject cilindro = Object.Instantiate(prefab, parent);
        //Cambiando las dimensiones por los randoms y poniendo las posiciones en el suelo
        cilindro.transform.localScale = new Vector3(radio, alto, radio);
        cilindro.transform.position = new Vector3(PosicionInicial.x, PosicionInicial.y, PosicionInicial.z);
        
        configuracionObstaculo.Configurar(cilindro);//Llamo a la interfaz con la figura correspondiente a este script
        
    }

}