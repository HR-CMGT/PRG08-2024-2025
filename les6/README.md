# Week 6

- Herhaling data>training>model. 
- Werken met KNN in javascript
- Data verzamelen uit MediaPipe en opslaan als JSON.
- Data uit MediaPipe voorspellen met KNN
- Troubleshooting

<br><br><br>

## Introductie K-Nearest-Neighbour

Dit algoritme gebruikt afstanden tussen punten om te bepalen waar een punt bij hoort. Je leert de termen *Classification* en *Supervised Learning*.

In dit voorbeeld tekenen we de *weight* en *ear length* van katten en honden in een 2D grafiek als X en Y coördinaten:

![knn](../images/knn_catdog_icons.png)

Als we een nieuw punt tekenen in de grafiek, kunnen we via de **afstand tot de andere punten** bepalen of het nieuwe punt een kat of een hond is! Dit is wat het KNN algoritme doet. Zie ook dit [interactief voorbeeld op Codepen](https://codepen.io/Qbrid/pen/OwpjLX). 

<br>
<br>
<Br>

## Werken met KNN in Javascript

In de map van [week 6](./) staat [knear.js](./knear.js). Deze kun je downloaden en toevoegen aan jouw project. We raden je aan om deze versie van de library te gebruiken. We hebben error messages toegevoegd aan de library wanneer er met onjuiste data wordt gewerkt. 

Maak het algoritme aan in app.js
```sh
import kNear from "./knear.js"

const k = 3
const machine = new kNear(k);
```
<br><br><br>

## Classifying

Je gaat het KNN algoritme trainen met gelabelde data. In dit voorbeeld zie je twee datapunten. Vul hier alle data uit onderstaande tabel in! 

```javascript
machine.learn([6, 5, 9, 4], 'cat')
machine.learn([12, 20, 19, 3], 'dog')
```

| Body length | Height | Weight | Ear length |  Label |
| ----------- | ------ | ------ | ---------- |  ----- |
| 18 | 9.2 | 8.1 | 2 | 'cat' |
| 20.1 | 17 | 15.5 | 5 | 'dog' |
| 17 | 9.1 | 9 | 1.95 | 'cat' |
| 23.5 | 20 | 20 | 6.2 | 'dog' |
| 16 | 9.0 | 10 | 2.1 | 'cat' |
| 21 | 16.7 | 16 | 3.3 | 'dog' |

Als je met voldoende data getraind hebt, kan je een `classification` doen.

```javascript
let prediction = machine.classify([12,18,17,12])
console.log(`I think this is a ${prediction}`)
```
<br>
<br>
<br>

## Werken met MediaPipe data

Het volgende doel is om een handpose, bodypose of facepose uit MediaPipe te classificeren *(bijvoorbeeld het herkennen van "rock" "paper" "scissors" poses)*. Om dit te kunnen doen ga je de volgende stappen zelfstandig doorlopen:

### Pose data verzamelen

Verzamel handpose, bodypose of facepose data uit mediapipe.

Laat de webcam detectie lopen en toon de `x,y,z` coördinaten voor de pose in de console of in een html veld. 

> *🚨 Zorg dat de data uit één enkele array van getallen bestaat. De mediapipe posedata bestaat vaak uit meerdere nested arrays en objecten. Dit moet je vereenvoudigen.*
```js
[
     {x: 0.1, y: 0.3, z: 0.6},
     {x: 0.2, y: 0.7, z: 0.9},
     ...
]
```
Moet je omzetten naar
```js
[0.1, 0.3, 0.6, 0.2, 0.7, 0.9, ...]
```

Vervolgens geef je een **label** aan de data. Het geheel sla je op in een javascript array of in een JSON file. Hieronder een voorbeeld met één pose:

```js
[
    {pose:[3,34,6,3,...], label:"rock"}
]
```
Let op dat je voor elke pose die je wil leren *tientallen voorbeelden* nodig hebt.

<br><br><br>

### Pose data gebruiken

Nu kan je de posedata aan het KNN algoritme leren op dezelfde manier als bij het *cats and dogs* voorbeeld hierboven.

### Nieuwe poses herkennen

Maak een nieuw project waarin ook weer de MediaPipe pose detection met de webcam draait. Echter, nu ga je de poses proberen te herkennen met je getrainde model!

<br><br><br>

# Expert level

- Je kan KNN trainen in [React](../snippets/reactknn.md)
- Je kan [React](../snippets/react.md) gebruiken om de MediaPipe interface te bouwen.

<br><br><br>

# Troubleshooting

### Workflow

Bij het werken met KNN heb je meerdere projecten tegelijk open staan:

- Het project waarin je data verzamelt uit de webcam en er een label aan geeft. In dit project heb je een live webcam en teken je de posedata over het webcam beeld. Dit project slaat de data op om te kunnen gebruiken in je live applicatie. Natuurlijk kan je KNN alvast toevoegen om te kunnen testen of je poses kunt detecteren (in de console).
- De live applicatie waarin de verzamelde data ingeladen wordt om KNN mee te kunnen trainen. Het getrainde model is direct te gebruiken op nieuwe data (bijv inputdata van je webcam) in dit project. In het eindproduct hoef je niet altijd de pose als lijntjes over het webcam beeld heen te tekenen of de output van de webcam te laten zien.


### Fouten bij trainen

Het trainen van een model kan makkelijk mis gaan. De meest voorkomende oorzaken:

- De data is niet consistent. De inhoud van elk datapunt *(een array met getallen)* moet voor elk datapunt exact hetzelfde zijn. Als één pose uit 100 punten bestaat, dan moeten alle poses uit 100 punten bestaan.
- De labels kloppen niet of je bent labels vergeten.
- Er is iets mis gegaan bij het opslaan van de posedata. Niet elke pose heeft evenveel getallen, of je hebt getallen opgeslagen als strings. (bv. `pose="5,2,5,2"`)
- Je verzamelde data geef je niet in de juiste vorm door aan het algoritme.
- Je data in de classify aanroep heeft een andere vorm dan de data die je bij addData hebt gebruikt.

#### Veel voorkomende fouten

```js
// de pose is hier een object, maar het moet alleen een array met numbers zijn
machine.learn({pose:[2,4,5,3]}, "rock")

// hier gaat het trainen wel goed, maar bij classify is de data array ineens veel langer
machine.learn([2,3,4], "rock")
machine.learn([5,3,1], "paper")
let result = machine.classify([2,3,4,5,6,7])
```

<br>
<br>
<br>

## Links

- [kNear](https://github.com/NathanEpstein/KNear)
- [KNN Codepen Demo](https://codepen.io/Qbrid/pen/OwpjLX) en [uitleg](https://burakkanber.com/blog/machine-learning-in-js-k-nearest-neighbor-part-1/)
- [MediaPipe Examples](https://developers.google.com/mediapipe/solutions/examples)
- [MediaPipe Javascript Documentation](https://developers.google.com/mediapipe/api/solutions/js/tasks-vision)
- [MediaPipe Handpose](https://developers.google.com/mediapipe/api/solutions/js/tasks-vision.handlandmarker#handlandmarker_class)
- [KNN in React](../snippets/reactknn.md) 
- [MediaPipe in React](../snippets/react.md) 
