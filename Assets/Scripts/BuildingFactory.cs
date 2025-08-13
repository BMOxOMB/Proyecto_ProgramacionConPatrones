using UnityEngine;

public abstract class BuildingFactory
{
    public abstract int Cost { get; }
    public abstract GameObject CrearEdificio(Vector3 posicion);
}