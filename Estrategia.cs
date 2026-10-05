
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tpfinal
{

	public class Estrategia
	{
		
		public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id)
        {
            return GetUrlSeoPorIdRecursivo(arbol, id, ""); 
        }
        private static string GetUrlSeoPorIdRecursivo(ArbolGeneral<ItemCat> nodo, int id, string rutaActual) 
    	{	if (nodo == null) return null;
			string nombreActual = ""; 
			string nuevaRuta = "";
			ItemCat dato = nodo.getDatoRaiz();
			if (dato != null)
			{	
				nombreActual = dato.Nombre.Replace(" ", ""); 
			}
			if (string.IsNullOrEmpty(rutaActual))
			{
    			nuevaRuta = nombreActual;
			}
			else
			{
    			nuevaRuta = rutaActual + "/" + nombreActual;
			}
			if (dato != null && dato.Id == id) 
			{
				return nuevaRuta;
			}
			foreach (var hijo in nodo.getHijos()) 
        		{
            	string resultado = GetUrlSeoPorIdRecursivo(hijo, id, nuevaRuta);
            	if (resultado != null)
            	{
                return resultado;
            	}
        		}
        	return null;
    		}

        public List<string> GetURLsSEO(ArbolGeneral<ItemCat> arbol) 
		{
			List<string> urls = new List<string>();
        	if (arbol == null) return urls;
        	GetURLsSEORecursivo(arbol, "", urls);
        	return urls;
		}
        private static void GetURLsSEORecursivo(ArbolGeneral<ItemCat> nodo, string rutaActual, List<string> urls) 
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

        if (nodo.esHoja())
        {
            urls.Add(nuevaRuta); 
        }
        else 
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

        Cola<ArbolGeneral<ItemCat>> cola = new Cola<ArbolGeneral<ItemCat>>(); 
        cola.encolar(arbol);
        cola.encolar(null); 

        List<string> nivelActual = new List<string>();

        while (!cola.esVacia())
        {
            ArbolGeneral<ItemCat> actual = cola.desencolar();

            if (actual == null)
            {
                resultado.Add(nivelActual); 
                nivelActual = new List<string>(); 

                if (!cola.esVacia())
                {
                    cola.encolar(null);
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

        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
        {
            List<ItemCat> resultado = new List<ItemCat>();
            RecolectarProductos(arbol, resultado);
            return resultado;
        }

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

        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar, TipoElemento? filtroTipo = null)
		{
			List<ItemCat> resultado = new List<ItemCat>();
            RecolectarCoincidencias(arbol, elementoABuscar, filtroTipo, resultado);
            return resultado;
		}

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
