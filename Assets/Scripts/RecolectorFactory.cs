using UnityEngine;

public class RecolectorFactory : BuildingFactory
{
    private GameObject prefab;

    public RecolectorFactory(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public override int Cost => 25;

    public override GameObject CrearEdificio(Vector3 posicion)
    {
        return Object.Instantiate(prefab, posicion, Quaternion.identity);
    }
}