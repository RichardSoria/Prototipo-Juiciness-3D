# Prototipo 3D - Mecánicas de Juiciness y Game Feel

Este repositorio contiene el código fuente y la arquitectura lógica de un prototipo en tercera persona desarrollado en **Unity**. El objetivo principal del proyecto es la implementación técnica de conceptos de *Juiciness* (jugosidad) y *Game Feel*, transformando operaciones lógicas invisibles en eventos altamente interactivos y responsivos para el usuario.

## 🛠️ Características Técnicas

### 1. Control de Personaje y Máquina de Estados
* **New Input System:** Gestión de eventos de entrada periférica optimizada mediante mapas de acción.
* **Sincronización de Animaciones:** Controlador basado en variables físicas (`isGrounded`, magnitudes de vector de movimiento) que disparan transiciones fluidas en el *Animator Component*.

### 2. Gestión Modular de Audio
* **Desacoplamiento de Canales:** Uso de múltiples componentes `AudioSource` en el Inspector. Un canal dedicado en bucle para el sonido de pasos (diferenciando dinámicamente entre caminata y carrera) y un canal analítico para eventos instantáneos (*One-Shot*) de ataque, salto y aterrizaje.
* **Audio Tridimensional Efímero:** Implementación de `AudioSource.PlayClipAtPoint` en objetos destructibles (monedas y minas). Esto garantiza el ciclo de vida del buffer de audio en coordenadas globales, evitando que el sonido se interrumpa abruptamente al ejecutar la rutina de destrucción (`Destroy(gameObject)`).

### 3. Deformación Procedimental (Squash & Stretch)
* **Animación por Código:** En lugar de usar clips de animación estáticos, el trampolín elástico calcula su deformación en tiempo real utilizando **Corrutinas** (`IEnumerator`).
* **Interpolación Lineal Avanzada:** Uso de `Vector3.Lerp` escalado con `Time.deltaTime` para modificar la matriz de transformación de escala de forma progresiva. El script calcula tres fases consecutivas (Aplaste, Estiramiento y Recuperación) respetando la ley física de conservación de volumen (multiplicación proporcional de ejes X, Y y Z).
* **Inyección de Fuerza:** Interconexión de scripts mediante paso de parámetros públicos, modificando la velocidad vertical del controlador del personaje en el mismo fotograma del impacto visual.

### 4. Feedback de Impacto Cinematográfico
* **Cinemachine Impulse:** Uso de componentes *Impulse Source* y *Impulse Listener* para transferir la energía simulada de una explosión matemática en una vibración armónica de la cámara principal.
* **Instanciación Dinámica:** Generación de prefabs de texto tipo cómic con un desfase matemático en el eje Y para evitar oclusiones con la geometría del entorno.

## 📁 Estructura de Scripts Principales

* `ThirdPersonController.cs`: Administra los vectores de movimiento, la máquina de estados del Animator y la lógica selectiva de clips de audio.
* `Coin.cs`: Gestiona la detección de hitboxes indulgentes, partículas de destello y destrucción de assets.
* `MinaTerrestre.cs`: Dispara la secuencia de partículas, textos de impacto de Cartoon FX, audio espacializado y el impulso sísmico de Cinemachine.
* `TrampolinSlime.cs`: Controla el bucle asíncrono de deformación geométrica por vectores y la transferencia de fuerza vertical al jugador.
