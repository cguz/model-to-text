# IntelliText

- Sistema de descripci�n de objetos generico, no s�lo del modelo de PrefCAD.
- Implementaci�n API, SOA, gRPC (por tanto, fuera de la factoria y en .net)
- Plantear la posibilidad de gestionar m�s de una base de datos por proceso

## Entradas

Tenemos varios tipos de entradas al proceso:

- Objetos a describir (JSON, XML, estructuras, ...)
- Datos est�ticos (bd, kb, ...)
- Plantillas

Alguna de estas entradas se introducir�n desde la KB. En todo caso acabar�n estando en la base de datos.

Las fuentes de datos se identificar�n mediante un identificador ("model.dimensions", "price.documentation",...). Este identificador permitir�a determinar qu� datos se extra�n del objeto a describir.


## Extracci�n y Manipulaci�n de datos

Mediante un componente ObjectDataProvider recibiremos la informaci�n relativa al objeto.
Estos datos pueden venir de fuentes y en formatos diferentes. Interesa que estos lleguen de manera homog�nea o que tengamos mecanismos para adaptarlos.

Cada conjunto de datos tendr� un identificador (dimensions�)

Se crear� internamente una estructura de datos con _atomos_ de informaci�n incompletos. Usando lenguages de manipulaci�n de datos y una o varias plantillas transformar�amos la estructura hasta tener los datos necesarios y completos.

Posibles lenguages a estudiar: linq, liquid, fluid, ...?

Esta estructura permitir� generar el documento diferencial o _Delta_.


## Representaci�n

La representaci�n se har� en ultima instancia a partir de la informaci�n generada. Se pueden exportar a unos pocos formatos o que se deje la visualizaci�n al componente consumidor.


# Gadgets existentes

## Texto autom�tico y de producci�n

## Descripcion

## Materiales

## Color

## Plausibilidades

## Precios

## Custom

# Propuestas
