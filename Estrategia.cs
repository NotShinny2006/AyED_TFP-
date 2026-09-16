
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tpfinal
{

	public class Estrategia
	{
		
        //Metodo para obtener la url de un elemento por su id
		public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id)
        {
            return GetUrlSeoPorIdRecursivo(arbol, id, ""); //se unicializa una string vacia ya que es la primera llamada recursiva para obtener la ruta del elemento
        }
        private static string GetUrlSeoPorIdRecursivo(ArbolGeneral<ItemCat> nodo, int id, string rutaActual) //metodo recursivo privado
    	{	if (nodo == null) return null;
			string nombreActual = ""; //nombre del nodo que se recorre actualmente
			string nuevaRuta = "";
			ItemCat dato = nodo.getDatoRaiz();
			if (dato != null)
			{	
				nombreActual = dato.Nombre.Replace(" ", ""); //elimino los espacios del nombre del dato para agregarlo a la url
			}
			if (string.IsNullOrEmpty(rutaActual))
			{
    			nuevaRuta = nombreActual;
			}
			else
			{
    			nuevaRuta = rutaActual + "/" + nombreActual;
			}
			if (dato != null && dato.Id == id) //si el id del dato coincide con el id del elemento a buscar devuelve la ruta (condicion de corte).
			{
				return nuevaRuta;
			}
			foreach (var hijo in nodo.getHijos()) //si tiene hijos, realiza una pila de llamadas recursivas (DFS o busqueda en profundidad).
        		{
            	string resultado = GetUrlSeoPorIdRecursivo(hijo, id, nuevaRuta);
            	if (resultado != null)
            	{
                return resultado;
            	}
        		}
        	return null;
    		}

		//Obtener todas las URLs amigables recorriendo desde un arbol (pasado como parametro) hasta cada hoja
        public List<string> GetURLsSEO(ArbolGeneral<ItemCat> arbol) 
		{
			List<string> urls = new List<string>();
        	if (arbol == null) return urls;
        	GetURLsSEORecursivo(arbol, "", urls);
        	return urls;
		}
        private static void GetURLsSEORecursivo(ArbolGeneral<ItemCat> nodo, string rutaActual, List<string> urls) //metodo recursivo privado
    {
        string nombreActual = "";
        if (nodo.getDatoRaiz() != null)
        {
            nombreActual = nodo.getDatoRaiz().Nombre.Replace(" ", "");
        }
        string nuevaRuta = "";
        if (string.IsNullOrEmpty(rutaActual))
        {
            nuevaRuta = nombreActual;
        }
        else
        {
            nuevaRuta = rutaActual + "/" + nombreActual;
        }

        if (nodo.esHoja())//si no tiene hijos (condicion de corte), se agrega a la lista de urls
        {
            urls.Add(nuevaRuta); 
        }
        else //si tiene hijos, se obtienen los nodos hijos para ser implementados recursivamente hasta cumplir la condicion de corte, implementado así una busqueda en profundidad o DFS
        {
            foreach (var hijo in nodo.getHijos())
            {
                GetURLsSEORecursivo(hijo, nuevaRuta, urls);
            }
        }
    }

        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol)
		{
            List<List<string>> resultado = new List<List<string>>();
        if (arbol == null) return resultado;

        Cola<ArbolGeneral<ItemCat>> cola = new Cola<ArbolGeneral<ItemCat>>(); //Utilizamos la clase Cola para un eficiente acceso por nivel o BFS
        cola.encolar(arbol);
        cola.encolar(null); // Elemento que demarca el fin de nivel

        List<string> nivelActual = new List<string>();

        while (!cola.esVacia())
        {
            ArbolGeneral<ItemCat> actual = cola.desencolar();

            if (actual == null)
            {
                resultado.Add(nivelActual); // Se agrega la lista interna con los nodos del mismo nivel a la lista general
                nivelActual = new List<string>(); // Se crea una nueva lista interna con el nuevo nivel

                if (!cola.esVacia())
                {
                    cola.encolar(null);// Si todavia existen nodos, se encarga de demarcar y/o cerrar el próximo nivel entrante para no generar bucles infinitos con el while
                }
            }
            else
            {
                if (actual.getDatoRaiz() != null)
                {
                    nivelActual.Add(actual.getDatoRaiz().Nombre);
                }

                foreach (var hijo in actual.getHijos())
                {
                    cola.encolar(hijo);
                }
            }
        }

        return resultado;
    }

        //Devuelve todos los productos del catalogo, no especifica absolutamente nada, manda todo de una//
        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
        {
            List<ItemCat> resultado = new List<ItemCat>();
            RecolectarProductos(arbol, resultado);
            return resultado;
        }
        //Metodo para recolectar todos los productos, de paso los aloja momentaneamente asi el return funciona//
        private void RecolectarProductos(ArbolGeneral<ItemCat> nodo, List<ItemCat> resultado)
        {
            if (nodo.getDatoRaiz().Tipo == TipoElemento.Producto)
            {
                resultado.Add(nodo.getDatoRaiz());
            }

            foreach (var hijo in nodo.getHijos())
            {
                RecolectarProductos(hijo, resultado);
            }

        }

        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre)
        {
            string[] categorias = rutaAlPadre.Split("/");
            ArbolGeneral<ItemCat> actual = arbol;

            foreach (var nombreCategoria in categorias)
            {
                ArbolGeneral<ItemCat> hijoExistente = BuscarHijoPorNombre(actual, nombreCategoria);

                if (hijoExistente == null)
                {
                    ItemCat nuevaCategoria = new ItemCat(nombreCategoria, TipoElemento.Categoria);
                    hijoExistente = new ArbolGeneral<ItemCat>(nuevaCategoria);
                    actual.agregarHijo(hijoExistente);
                }

                actual = hijoExistente;
            }

            if (BuscarHijoPorNombre(actual, dato.Nombre) == null)
            {
                actual.agregarHijo(new ArbolGeneral<ItemCat>(dato));
            }
        }

        //metodo para buscar hijo por nombre
        private ArbolGeneral<ItemCat> BuscarHijoPorNombre(ArbolGeneral<ItemCat> nodo, string nombre)
        {
            foreach (var hijo in nodo.getHijos())
            {
                if (hijo.getDatoRaiz().Nombre == nombre)
                {
                    return hijo;
                }
            }

            return null;
        }

        //buscar(): Basicamente, a diferencia de todos() este busca filtrado por categorias y productos.//
        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar, TipoElemento? filtroTipo = null)
		{
			List<ItemCat> resultado = new List<ItemCat>();
            RecolectarCoincidencias(arbol, elementoABuscar, filtroTipo, resultado);
            return resultado;
		}

        //ACTUALIZACIÓN: Ahora tiene un filtro extra de tipos.//
        private void RecolectarCoincidencias(ArbolGeneral<ItemCat> nodo, string texto, TipoElemento? filtroTipo, List<ItemCat> resultado)
        {
            bool CoincideNombre = nodo.getDatoRaiz().Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase);
            bool coincideTipo = filtroTipo == null || nodo.getDatoRaiz().Tipo == filtroTipo;

            if (CoincideNombre && coincideTipo)
            {
                resultado.Add(nodo.getDatoRaiz());
            }

            foreach (var hijo in nodo.getHijos())
            {
                RecolectarCoincidencias(hijo, texto, filtroTipo, resultado);
            }
        }
            
    }
}
