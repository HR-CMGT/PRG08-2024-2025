# Les 3

Herhaling Pose Detection. In deze les blikken we terug op les 1 en 2, die gegeven zijn voor het TLE project. 

### Workflow

| Website | Doel |  
| ----------- | ------ |  
| MediaPipe Website basic | Webcam `x,y,z` data verzamelen en opslaan als JSON |
| Eindapplicatie | Feedback tonen op basis van live webcam poses | 

<br><br><br>

# Werken met Posedata

In MediaPipe ga je Posedata verzamelen. Dit ziet er als volgt uit:

```js

```

## Oefening

- Plaats een afbeelding op de plek van de hand.
- Bestuur een game met je hand.
- Maak een tekening met je hand.

<br><br><br>

# Poses herkennen

We gaan gemaakte poses herkennen met het KNN algoritme.
Dit algoritme verwacht een array van getallen als data, maar in mediapipe krijg je een array van objecten.

## Oefening

Het doel is dat je de `x,y,z` data van MediaPipe omzet naar een simpele array van getallen.

*MediaPipe*
```js
//
```

*Array voor KNN*
```js
[4,2,5,2,1,...]
```

<br><br><br>

## Werken met KNN

Nu we data verzameld hebben kunnen we met KNN kijken of een huidige pose die voor de webcam gedaan wordt, overeenkomt met een pose die we vantevoren hebben opgeslagen.

```js
//
```

<br>
<br>
<br>

# De frontend applicatie bouwen

Dit is je game of applicatie die door de eindgebruiker gebruikt gaat worden. Hierin wordt de live webcam getoond met poses. Je gaat nu ook weer posedata uit de webcam halen. Het doel is nu om te voorspellen welke pose de gebruiker aanneemt, dit doen we met KNN.

- Toon de webcam en lees posedata met mediapipe
- Maak een voorspelling van de live posedata

<br><br><br>

# Links

- [Les 1](../les1/README.md)
- [Les 2](../les2/README.md)