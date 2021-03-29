# IntelliText

- Sistema de descripción de objetos generico, no sólo del modelo de PrefCAD.
- Implementación API, SOA, gRPC (por tanto, fuera de la factoria y en .net)
- Plantear la posibilidad de gestionar más de una base de datos por proceso

## Entradas

Tenemos varios tipos de entradas al proceso:

- Objetos a describir (JSON, XML, estructuras, ...)
- Datos estáticos (bd, kb, ...)
- Plantillas

Alguna de estas entradas se introducirán desde la KB. En todo caso acabarán estando en la base de datos.

Las fuentes de datos se identificarán mediante un identificador ("model.dimensions", "price.documentation",...). Este identificador permitiría determinar qué datos se extraén del objeto a describir.


## Extracción y Manipulación de datos

Mediante un componente ObjectDataProvider recibiremos la información relativa al objeto.
Estos datos pueden venir de fuentes y en formatos diferentes. Interesa que estos lleguen de manera homogénea o que tengamos mecanismos para adaptarlos.

Cada conjunto de datos tendrá un identificador (dimensions¡)

Se creará internamente una estructura de datos con _atomos_ de información incompletos. Usando lenguages de manipulación de datos y una o varias plantillas transformaríamos la estructura hasta tener los datos necesarios y completos.

Posibles lenguages a estudiar: linq, liquid, fluid, ...?

Esta estructura permitirá generar el documento diferencial o _Delta_.


## Representación

La representación se hará en ultima instancia a partir de la información generada. Se pueden exportar a unos pocos formatos o que se deje la visualización al componente consumidor.


# Gadgets existentes

## Texto automático y de producción

## Descripcion

## Materiales

## Color

## Plausibilidades

## Precios

## Custom

# Propuestas
