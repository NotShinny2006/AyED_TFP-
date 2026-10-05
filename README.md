# API Catálogo — TFP Algoritmos y Estructuras de Datos

API REST que administra un catálogo de e-commerce modelado como **Árbol General**.
Trabajo Final Práctico.

## Integrantes
- Santiago Pais
- Berenguera Gonzalo

## Estado de avance

| Método (`Estrategia.cs`) | Estado |
|---|---|
| `Todos` | 🟩 Completado (100%) |
| `Buscar` | 🟩 Completado (100%) |
| `Agregar` | 🟩 Completado (100%) |
| `GetURLsSEO` | 🟩 Completado (100%) |
| `GetUrlSeoPorId` | 🟩 Completado (100%) |
| `ConsultaNiveles` | 🟩 Completado (100%) |

## Diagrama de clases

```mermaid
classDiagram
    class ArbolGeneral~T~ {
        -T dato
        -List~ArbolGeneral~T~~ hijos
        +getDatoRaiz() T
        +getHijos() List
        +agregarHijo(hijo)
        +eliminarHijo(hijo)
        +esHoja() bool
    }
    class ItemCat {
        +int Id
        +string Nombre
        +TipoElemento Tipo
        +string CodigoSKU
        +double Precio
    }
    class TipoElemento {
        <<enumeration>>
        Categoria
        Producto
    }
    class Cola~T~ {
        -List~T~ datos
        +encolar(elem)
        +desencolar() T
        +esVacia() bool
    }
    class Estrategia {
        +Agregar(arbol, dato, rutaAlPadre)
        +Buscar(arbol, texto) List
        +Todos(arbol) List
        +GetURLsSEO(arbol) List
        -GetURLsSEORecursivo(nodo, rutaActual, urls) void
        +GetUrlSeoPorId(arbol, id) string
        -GetUrlSEOPorIDRecursivo(nodo, id, rutaActual) string
        +ConsultaNiveles(arbol) List
    }
    ArbolGeneral "1" o-- "0..*" ArbolGeneral : hijos
    ArbolGeneral --> ItemCat : dato
    ItemCat --> TipoElemento
    Estrategia ..> ArbolGeneral : opera sobre
    Estrategia ..> Cola : usa para BFS
```

## Algoritmo de Agregar

**Supuesto de diseño:** `rutaAlPadre` viene con las categorías separadas por `/`
(ej: `"Electronica/Computadoras/Laptops"`), siguiendo el mismo formato que las
URLs SEO del enunciado. Si el profesor indica otro formato, se ajusta solo esta parte.

```mermaid
flowchart TD
    A[Agregar arbol, dato, rutaAlPadre] --> B[Separar rutaAlPadre por '/']
    B --> C[actual = raiz del arbol]
    C --> D{Quedan tramos de ruta?}
    D -- Si --> E{Existe hijo de 'actual' con ese nombre?}
    E -- Si --> F[actual = ese hijo]
    E -- No --> G[Crear nodo Categoria nuevo]
    G --> H[agregarHijo a 'actual']
    H --> F
    F --> D
    D -- No --> I[Crear nodo con 'dato']
    I --> J[agregarHijo a 'actual']
```
