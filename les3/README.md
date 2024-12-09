# Les 3

In deze les blikken we terug op les 1 en 2, die gegeven zijn voor het TLE project. 

### Herhaling 

| Les  | Beschrijving | Bestanden |
|------|--------------|---------- |
| [1](../les1/README.md)   | MediaPipe poses tonen in de console | [boilerplate](../les1/boilerplate/) |
| [2](../les2/README.md)   | MediaPipe Posedata omzetten naar versimpelde array | [JSON data](./data/data-rps.json) |
|    | KNN `learn` aanroepen met versimpelde posedata |[knear.js](../les2/knear.js) |
|    | KNN `classify` aanroepen met live webcam data | |

<br><br><br>

# Mediapipe Posedata in de console

Zet het mediapipe project op volgens de [boilerplate code uit les 1](../les1/boilerplate/) of start je eigen [MediaPipe project met Vite](https://github.com/HR-CMGT/PRG08-2024-2025/blob/main/les1/vite.md) (recommended).

In MediaPipe ga je Posedata verzamelen. Maak een HTML button die de huidige webcam pose in de console laat zien als je er op klikt: `console.log(results)`. Een enkele pose van één hand heeft 20 punten:

```js
[
     {x: 0.1, y: 0.3, z: 0.6},
     {x: 0.2, y: 0.7, z: 0.9},
     // ...in totaal 20 punten
]
```
<img src="../images/hand-landmarks.png" width="600"/>

<br><br><br>

## Oefening

Plaats een DOM element met id `myimage` in je HTML file, bovenop het canvas. Gebruik nu de positie van de duim om het html element mee te laten bewegen. Let daarbij op het volgende:

- De `x,y,z` getallen gaan van 0 tot 1. De `x,y` waarde `0,0` betekent linksboven en `1,1` betekent rechtsonder.
- Je moet de `x,y` waarden vermenigvuldigen met de breedte en hoogte van het video element om het op de juiste plek te plaatsen. 
- De `z` waarde is de afstand tot de camera. Dit kan je gebruiken voor de schaal van een element. Dichtbij is groter.

> *Als de duim bv. een `x,y` heeft van `0.2, 0.4` dan is de waarde in pixels `0.2 * videoWidth, 0.4 * videoHeight`.*

*voorbeeld*
```css
#myimage {
    position:absolute;
}
```
```js
let image = document.querySelector("#myimage")
let thumb = result.landmarks[0][4]
image.style.transform = `translate(${thumb.x * videoWidth}px, ${thumb.y * videoHeight}px)`
```
Plaats code in je `requestAnimationFrame` loop om het telkens te blijven herhalen.

<br><br><br>

# Handposes herkennen

We gaan handposes zoals `rock, paper, scissors` leren herkennen met het KNN algoritme. Dit algoritme verwacht een array van getallen als data, waarna je nieuwe data kan gaan herkennen. 

Voeg het bestand [knear.js](../les2/knear.js) toe aan je projectmap. Maak een testje met fake data om te zien of het algoritme werkt:


```js
import kNear from "./knear.js"

const k = 3
const machine = new kNear(k);

machine.learn([6, 5, 9, 4], 'cat')
machine.learn([12, 20, 19, 3], 'dog')

let prediction = machine.classify([12,18,17,12])
console.log(`I think this is a ${prediction}`)
```
<br><br><br>

## Data vereenvoudigen

Zoals je ziet verwacht het KNN algoritme data in deze vorm : `[2,4,2,3,2], "cat"` *(een array met getallen gevolgd door een label)*. De opdracht is nu om de data van MediaPipe om te zetten naar deze vereenvoudigde vorm. Dit kan je doen met een `for` loop of met de `map()` functie.
Zorg dat de `console.log()` uit de vorige opdracht nu de versimpelde data toont.

```js
// mediapipe data
[{x: 0.1, y: 0.3, z: 0.6}, {x: 0.2, y: 0.7, z: 0.9},...etc]

// versimpelde data
[0.1,0.3,0.6,0.2,0.7,0.9,...etc]
```

<br><br><br>

## Data opslaan als json

Je gaat nu je poses uit de vorige opdracht opslaan in een JSON bestand. Dit kan je handmatig doen door telkens een array uit de console te copy pasten. Je moet ook het label dat bij de pose hoort toevoegen. Je JSON file kan er dan als volgt uit gaan zien. [Zie ook dit voorbeeld bestand](./data/data-rps.json).


```js
[
    {points:[0.3.0.1.0.13.0.41.0.24.0,...], label:"rock"},
    {points:[0.3.0.1.0.13.0.41.0.24.0,...], label:"rock"},
    {points:[0.3.0.1.0.13.0.41.0.24.0,...], label:"rock"},
    // .. en nog meer poses voor paper en scissors
]
```

<br>
<br>
<br>

## De applicatie bouwen

We kunnen nu met het KNN algoritme herkennen of iemand een `rock,paper,scissors` gebaar maakt voor de webcam. Begin met het leren van de poses:

*json laden*
```js
fetch("mydata.json")
    .then(response => response.json())
    .then(data => train(data))
    .catch(error => console.log(error))
```
*data in knn*
```js
function train(poses) {
    for(let pose of poses) {
        // dubbel check of je hier een correct pose ziet:
        // {points:[2,4,5,3,...]}, "rock"}
        console.log(pose) 
        // geef de data aan de learn functie
        // machine.learn([2,4,5,3,...], "rock")
        machine.learn(pose.points, pose.label)
    }
}
```

Als het leren is gelukt kan je in je webcam applicatie de `classify` functie gaan aanroepen zodra je op de button klikt. Let op dat je alleen een array van getallen naar de `classify` functie stuurt. De console toont dan welke pose herkend wordt!

```js
function classifyPose(newpose) {
    let result = machine.classify(newpose)
    console.log(`I think this is a ${result}`)
}

classifyPose([2,3,4,5,6,7,...]) // voorbeeld
```






<br><br><br>

# Troubleshooting

- Je JSON file is geen geldige JSON. Dit kan je checken met een JSON validator.
- Je JSON file bevat geen labels, of de datapunten staan niet correct in een array met de vorm `[2,3,3,2,4]`
- Je hebt getallen opgeslagen als strings. (bv. `"5,2,5,2"`)
- De data in je `machine.learn()` aanroep moet een array zijn, gevolgd door een label. Dit mag dus geen object zijn.
- De array in je `machine.classify()` aanroep moet óók precies 60 getallen bevatten.

#### Veel voorkomende fouten

```js
// fout
machine.learn({pose:[2,4,5,3,...], label:"rock"})
let result = machine.classify({pose:[2,4,5,3,...]})

// goed
machine.learn([2,4,5,3,...], "rock")
let result = machine.classify([2,4,5,3,...])
```

<br>
<br>
<br>

## Links

- [Les 1](../les1/README.md)
- [Les 2](../les2/README.md)
- [kNear](https://github.com/NathanEpstein/KNear)
- [KNN Codepen Demo](https://codepen.io/Qbrid/pen/OwpjLX) en [uitleg](https://burakkanber.com/blog/machine-learning-in-js-k-nearest-neighbor-part-1/)
- [MediaPipe Examples](https://developers.google.com/mediapipe/solutions/examples)
- [MediaPipe Javascript Documentation](https://developers.google.com/mediapipe/api/solutions/js/tasks-vision)
- [MediaPipe Handpose](https://developers.google.com/mediapipe/api/solutions/js/tasks-vision.handlandmarker#handlandmarker_class)
- [KNN in React](../snippets/reactknn.md) 
- [MediaPipe in React](../snippets/react.md) 
