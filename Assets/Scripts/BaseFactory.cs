using UnityEngine;

public class BaseFactory : BuildingFactory
{
    private GameObject prefab;

    public BaseFactory(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public override int Cost => 100;

    public override GameObject CrearEdificio(Vector3 posicion)
    {
        return Object.Instantiate(prefab, posicion, Quaternion.identity);
    }
}
