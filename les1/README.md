# Les 1 - Intl. week - Pose detection

## Pose Detection

- In les 1 en 2 gaan we met de webcam handgebaren leren herkennen in Javascript.
- Dit kan je toepassen in de TLE opdracht.



<br><br><br>

## MediaPipe

[MediaPipe](https://developers.google.com/mediapipe/solutions/examples) is een library van google, waarin een model is getraind om poses in webcam beelden te herkennen. 

| Hand | Body | Face |
| ---- | ---- | ---- |
| <img src="../images/hand_landmark_960.png" width="400"> | <img src="../images/pose_detector_960.png" width="400"> | <img src="../images/face_landmarker_960.png" width="400"> |
| [Demo](https://codepen.io/eerk/pen/oNKVWvY?editors=0111) | [Demo](https://codepen.io/eerk/pen/QWPEYxj?editors=0011) | [Demo](https://mediapipe-studio.webapps.google.com/studio/demo/face_landmarker) |

<br><br><br>

## MediaPipe project

- Bekijk de [Codepen demo](https://codepen.io/eerk/pen/oNKVWvY?editors=0011).
- Je kan deze [boilerplate](./boilerplate/) downloaden als startproject. Open `index.html` met een `live server`.
- 🔥 Of start je eigen [Vite project](./vite.md)




<br><br><br>

## Posedata ophalen

MediaPipe geeft **per hand** een array terug van vector data. Dit bestaat uit een *`x,y,z` coördinaat* voor elk botje in je hand. 

<img src="../images/hand-landmarks.png" width="600"/>

De handen staan in `result.landmarks[0]` en `result.landmarks[1]`. Je kan dit in de console loggen. De detectie code moet je telkens herhalen met behulp van `requestAnimationFrame`. 

*voorbeeld*
```js
let result = handLandmarker.detectForVideo(video, performance.now());
if(result.landmarks) {
    console.log(result)
}
```


*resultaat*
```js
{
    handednesses: [],     
    worldlandmarks: [],   
    landmarks:[
        [{x:2, y:3, z:1}, {x:2, y:3, z:1}, {x:2, y:3, z:1}, ...],   // first hand
        [{x:2, y:3, z:1}, {x:2, y:3, z:1}, {x:2, y:3, z:1}, ...],   // second hand
    ]
}
```

<br><br><br>

## Handen tekenen met DrawingUtils

Om de handen over de video heen te tekenen is er een `canvas` element boven op de video geplaatst. De `video` en het `canvas` element hebben `position:absolute` en `top:0, left:0`. Dit zorgt dat ze precies over elkaar heen vallen.

De [DrawingUtils]([https://developers.google.com/mediapipe/api/solutions/js/tasks-vision.drawingutils](https://ai.google.dev/edge/api/mediapipe/js/tasks-vision#tasks_vision_package) class geeft je twee functies waarmee je de punten van de hand, en de lijnen daartussen meteen in het canvas kan tekenen:


*code voor drawingUtils*
```js
const canvasElement = document.getElementById("output_canvas")
const canvasCtx = canvasElement.getContext("2d")
const drawUtils = new DrawingUtils(canvasCtx)

function drawResults() {
    canvasCtx.clearRect(0, 0, canvasElement.width, canvasElement.height);
    for(let hand of results.landmarks){
        drawUtils.drawConnectors(hand, HandLandmarker.HAND_CONNECTIONS, { color: "#00FF00", lineWidth: 5 });
        drawUtils.drawLandmarks(hand, { radius: 4, color: "#FF0000", lineWidth: 2 });
    }
}
```
*⚠️ Het tekenen van de handen is niet persé nodig om de coördinaten uit de webcam te lezen.*

- [Javascript reference DrawingUtils](https://ai.google.dev/edge/api/mediapipe/js/tasks-vision#tasks_vision_package) 
- [Hand Landmark Detection](https://mediapipe-studio.webapps.google.com/studio/demo/hand_landmarker)

<br><br><br>

# x,y,z coordinaten uitlezen

Je kan de landmarks data gebruiken voor je eigen creatieve toepassingen. We gaan oefenen met het plaasen van een element op de plek van de duim. 

In de afbeelding zie je hoe de landmarks arrays zijn opgedeeld. De duim van hand `0` is dus: `result.landmarks[0][4]`. Hand `0` is de eerste hand die gedetecteerd is. Je kan in `results.handednesses[0]` zien of dit de linkerhand of rechterhand is.

- De `x,y,z` getallen gaan van 0 tot 1. De `x,y` waarde `0,0` betekent linksboven en `1,1` betekent rechtsonder.
- Je moet de `x,y` waarden vermenigvuldigen met de breedte en hoogte van het video element om het op de juiste plek te plaatsen. 
- De `z` waarde is de afstand tot de camera. Dit kan je gebruiken voor de schaal van een element. Dichtbij is groter.

> *Als de duim bv. een `x,y` heeft van `0.2, 0.4` dan is de waarde in pixels `0.2 * videoWidth, 0.4 * videoHeight`.*

<img src="../images/hand-landmarks.png" width="600"/>

<br><br><br>

## Oefening

Plaats een DOM element met id `myimage` in je HTML file, onder het `<video>` en `<canvas>` element. Je gaat nu de positie van de duim gebruiken om het html element mee te laten bewegen. 

*voorbeeld*
```css
#myimage {
    position:absolute;
}
```
```js
let image = document.querySelector("#myimage")
let thumb = results.landmarks[0][4]
image.style.transform = `translate(${thumb.x * video.videoWidth}px, ${thumb.y * video.videoHeight}px)`
```
> *Bedenk zelf waar deze drie regels code moeten staan. Bedenk of je een `if` statement nodig hebt voor situaties waarin helemaal geen hand zichtbaar is. Bedenk of en hoe je de X positie van de afbeelding kan spiegelen*


<br><br><br>

# Opdracht

Bedenk een toepassing waarbij de positie van de hand(en) gebruikt wordt. Bekijk [Bob's MediaPipe Paleis](https://bpikaar.github.io/prg8-2023-2024/mediapipe/) als inspiratie.

- Tekenprogramma
- Fruit Ninja (elementen weg slaan)
- Drumstel 
- Game karakter besturen

> *Let op dat een handpose ook een `z` coordinaat heeft (de afstand tot de camera)*


<br><br><br>

## Inspiratie

|  |  |
|--|--|
|Bob's MediaPipe Paleis<br>https://bpikaar.github.io/prg8-2023-2024/mediapipe/|--|
| <img src="../images/posepong.png" width="400"><br>Handpositie gebruiken om pong paddles te besturen | <img src="../images/pose-squid.png" width="400"><br>Afstand en beweging gebruiken om squid-game na te bouwen |
| <img src="../images/paint.png" width="400"><br>Wijsvinger gebruiken als verfkwast, duim als gum |<img src="../images/drumgesture.png" width="400"><br>[Gestures gebruiken om drumcomputer te besturen](https://youtube.com/shorts/zQ8Il7xyVQk) | 


<br><br><br>



### Voorbeeldcode tekenprogramma

Je kan ook handmatig in het canvas element tekenen, dan heb je meer controle over wat je precies wil tekenen. Je kan hier ook kiezen om geen `clearRect()` te doen waardoor al je tekeningen over elkaar heen getekend worden. Je ziet hier een code voorbeeld voor het tekenen van een cirkel in het canvas element.

```js
let canvaselement = document.querySelector("canvas")
let canvasCtx = canvaselement.getContext("2d")
canvasCtx.fillStyle = "red"
canvasCtx.strokeStyle = "blue"
canvasCtx.lineWidth = 4

// teken 1 cirkel
let x = 0.2
let y = 0.5
canvasCtx.clearRect(0, 0, canvasElement.width, canvasElement.height); 
canvasCtx.beginPath()
canvasCtx.arc(canvasElement.width * x, canvasElement.height * y, 4, 0, 2 * Math.PI);
canvasCtx.stroke();
canvasCtx.fill()
```
> *TIP: Als je niet bekend bent met het `<canvas>` element kan je eerst [deze MDN tutorial volgen](https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial)*

<br><br><br>


## Links

- [Hand Landmark Detection](https://mediapipe-studio.webapps.google.com/studio/demo/hand_landmarker)
- [Javascript Reference](https://ai.google.dev/edge/api/mediapipe/js/tasks-vision#tasks_vision_package)
- [Canvas Drawing Tutorial](https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial)
- [MediaPipe Guide](https://developers.google.com/mediapipe/solutions/guide)
- [MediaPipe Javascript Documentation](https://developers.google.com/mediapipe/api/solutions/js/tasks-vision)
- [MediaPipe Examples](https://developers.google.com/mediapipe/solutions/examples)
- [Codepen Hand](https://codepen.io/eerk/pen/oNKVWvY?editors=0111)
- [Codepen Body](https://codepen.io/eerk/pen/QWPEYxj?editors=0011)
- [Hand as computer interface](https://medium.spatialpixel.com/turning-your-hand-into-a-keyboard-6b21d092cfd0)
- [Charlie Gerard pose experiments](https://charliegerard.dev/projects)

## React

- [MediaPipe in React](../snippets/react.md)
