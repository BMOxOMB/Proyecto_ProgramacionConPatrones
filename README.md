# Proyecto_ProgramacionConPatrones
# Proyecto RTS en Unity (URP) — Programación con Patrones

> Prototipo educativo de RTS en Unity que combina patrones de diseño (**Builder**, **Command**, **Strategy**), control de cámara, selección múltiple de unidades, economía básica y sistema de IA/oleadas.

---

## Tabla de contenidos
- [Descripción general](#descripción-general)
- [Ambiente y mecánicas básicas](#ambiente-y-mecánicas-básicas)
- [Construcciones y economía](#construcciones-y-economía)
- [Unidades del jugador](#unidades-del-jugador)
- [Selección y control de unidades](#selección-y-control-de-unidades)
- [Enemigos y oleadas](#enemigos-y-oleadas)
- [Interfaz de recursos](#interfaz-de-recursos)
- [Otros scripts generales](#otros-scripts-generales)
- [Scripts de muestra del URP](#scripts-de-muestra-del-urp)
- [Personajes y componentes principales](#personajes-y-componentes-principales)
- [Funcionamiento del juego](#funcionamiento-del-juego)
- [Conclusión](#conclusión)

---

## Descripción general
Repositorio de un juego de estrategia en tiempo real (RTS) hecho con Unity. Estructura típica de Unity (`Assets`, `Packages`, `ProjectSettings`) y uso de **Universal Render Pipeline (URP)**.  
El objetivo del juego es construir y defender una base, recolectar recursos y crear unidades militares para combatir oleadas de enemigos controlados por IA.

---

## Ambiente y mecánicas básicas

### Mapa y obstáculos
- `BuilderFiguras`, `BuilderCubos`, `BuilderCilindro`, `ColisionObConfig`, `ColisionObInter`, `FigurasInter`, `DirectorMapa`: implementan un patrón **Builder** para generar cubos y cilindros aleatorios como obstáculos con colisiones y obstáculos de **NavMesh**.

### Recursos
- `DirectorRecursos`, `BuilderRecursos`, `GruposRecursos`, `TipoRecurso`: distribuyen grupos de recursos (Comida, Metal) por el mapa, asignándoles valor y tipo.

### Cámara RTS
- `RTSCameraController`: mueve la cámara con teclado, bordes de pantalla o arrastre, con opción de seguir un objetivo.

### Interfaz y utilidades
- `FaceCamera`: hace que un objeto mire siempre a la cámara.  
- `HealthTracker`: barra de vida con colores según porcentaje.  
- `Rotador`, `SimplePatrol`: scripts simples de rotación o patrulla lineal.  
- `Player`: ejemplo de slider de vida que disminuye al presionar **Espacio**.

---

## Construcciones y economía

- `Building` (clase base) y `BuildingFactory` (fábrica abstracta).  
- Fábricas concretas: `BaseFactory`, `RecolectorFactory`, `MilitarFactory`.

### Gestión y creación de edificios
- `BuildingManager`: cambia la fábrica con teclas **1–3** y coloca edificios al hacer clic si hay oro.  
- `BasePrincipal`: base con puntos de vida; si cae, es derrota.  
- `RecolectorRecursos`: genera oro periódicamente.  
- `EdificioMilitar`: entrena unidades a cambio de oro.  
- `GameManager`: **singleton** que maneja el oro y las condiciones de victoria/derrota.

---

## Unidades del jugador

### Clases e interfaces
- `UnidadMilitar`: unidad aliada que se mueve, recibe daño, cambia de estado y se registra globalmente.  
- Interfaces: `IUnidad`, `IUnidadEjecutable`, `IEstadoUnidadJugador`, `IComando`.  
- Estados:  
  - `EstadoIdle`: sin acción.  
  - `EstadoAtacarAuto`: busca enemigos cercanos y les inflige daño automático.

### Movimiento y ataque
- `UnitMovement`: usa `NavMeshAgent` para moverse con clic derecho.  
- `AttackController`: guarda objetivo y cambia materiales según estado (*idle*, *follow*, *attack*).  
- `UnitIdleState`, `UnitFollowState`, `UnitAttackState`: controlan animaciones y lógica de ataque vía `Animator`.

---

## Selección y control de unidades
- `UnitSelectionManager`: selección individual/múltiple, movimiento por clic y asignación de objetivos.  
- `UnitSelectionBox`: caja de selección al arrastrar el ratón.  
- `SeleccionMultiple`, `ControladorUnidades`: alternativas manuales para selección múltiple y emisión de órdenes.  
- `ComandoMover`: patrón **Command** que encapsula la orden de moverse.  
- `GrupoUnidades`: permite mover varias unidades como un solo conjunto.

---

## Enemigos y oleadas
- `OleadasManager`: genera oleadas incrementales de enemigos.  
- `EnemigoIA`: estados (`EstadoPatrullar`, `EstadoAtacar`), vida y estrategias (`EstrategiaMelee`, `EstrategiaRanged`).

### Interfaces y estados
- `IEstadoUnidadIA`, `IEstrategiaCombate`.  
- `EstadoPatrullar`: recorre puntos de patrulla.  
- `EstadoAtacar`: persigue unidades o la base.

### Otros componentes de IA
- `Enemy` y `UnidadMilitar` implementan `IDamageable`.  
- `EnemySpawner` (carpeta **AI**): *spawner* continuo o por oleadas.  
- `EnemyAIController` (en **AI**): FSM (Idle/Chase/Attack) que prioriza objetivos (unidades, centro urbano, otros edificios).  
- `Health`, `Targetable`, `Team` (en **AI**): componentes reutilizables para vida, equipo y selección de objetivos.

---

## Interfaz de recursos
- `UIRecursosTMP`: actualiza textos de oro, unidades, enemigos y número de oleada en pantalla.

---

## Otros scripts generales
- `Unit`, `Enemy`: implementaciones simples de unidades/damageables.  
- `UnidadesSpawner`: crea unidades desde un prototipo.  
- `GrupoUnidades`: maneja listas de unidades para ejecutar órdenes colectivas.

---

## Scripts de muestra del URP
En `Assets/Samples/Universal Render Pipeline/17.1.0/URP Package Samples` hay scripts de Unity que muestran características del URP (lens flare, copia de profundidad, distorsión, controladores de cámara, etc.).  
> **Nota:** estos scripts ilustran técnicas gráficas y **no** están ligados directamente a la lógica del juego.

---

## Personajes y componentes principales
1. **Jugador**: controla cámara, construye edificios y selecciona unidades.  
2. **Unidades aliadas (`UnidadMilitar`)**: patrullan, se mueven y atacan automáticamente al enemigo.  
3. **Edificios**: `BasePrincipal` (proteger), `RecolectorRecursos` (genera oro), `EdificioMilitar` (entrena unidades).  
4. **Enemigos (`EnemigoIA`)**: atacan en oleadas; persiguen unidades y edificios del jugador.  
5. **Recursos**: grupos de Comida y Metal distribuidos en el mapa; representados por objetos con valores numéricos.

---

## Funcionamiento del juego
1. **Generación del mundo**: al iniciar, se crean obstáculos aleatorios (cubos/cilindros) y grupos de recursos según tamaño del mapa.  
2. **Economía**: el jugador comienza con oro; los recolectores generan más; el oro se gasta para construir/entrenar.  
3. **Construcción**: teclas numéricas para elegir edificios y colocarlos en el terreno.  
4. **Unidades**: selección individual o múltiple y movimiento con clic derecho.  
5. **Combate**: ataque automático al detectar enemigos; los enemigos patrullan o atacan la base y a las unidades aliadas.  
6. **Oleadas**: `OleadasManager` lanza olas periódicas; al superar cierto número, se declara la victoria.  
7. **Final del juego**: perder la base = derrota; eliminar enemigos y resistir oleadas = victoria.

---

## Conclusión
Cada script cumple un rol dentro del flujo del juego: generación del escenario, administración de recursos, creación y control de unidades, combate, IA de enemigos y manejo de la interfaz.
