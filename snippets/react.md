# React MediaPipe

Voorbeeld van het werken met MediaPipe, Webcam en Canvas in React.

<br><br><br>

## Installatie

```sh
npm create vite@latest my-cool-project
cd my-cool-project
npm install
npm install react-webcam
npm install @mediapipe/tasks-vision
```
<br><br><br>


## App component

We gaan de `poseData` in een `state` bijhouden in `App`, zodat alle componenten de `poseData` kunnen gebruiken. Het `Posedetector` component krijgt een verwijzing naar de `handlePoseDataUpdate` functie. 

De app krijgt drie child components: een voor de webcam en het posemodel, een component voor canvas drawing, en een component voor tonen van `x,y,z` waarden. 

```jsx
import './App.css'
import Posedetector from './Posedetector'
import CanvasDrawing from './CanvasDrawing'
import Coordinates from './Coordinates'
import { useEffect, useState } from "react";

function App() {
  const [poseData, setPoseData] = useState([]);

  const handlePoseDataUpdate = (newPoseData) => {
    setPoseData(newPoseData);
  };

  return (
    <>
      <section className="videosection">
        <Posedetector onPoseDataUpdate={handlePoseDataUpdate}/>
        <CanvasDrawing poseData={poseData}/>
      </section>
      <Coordinates poseData={poseData}/>
    </>
  )
}

export default App
```
De webcam en canvas drawing staan samen in 1 section, omdat ze over elkaar heen getekend moeten worden. Voeg de CSS toe aan `App.css`:

```css
.videosection {
    background-color: rgb(70, 70, 70);
    height:270px;
}

.videosection video {
    position: absolute;
}

.videosection canvas {
    position: absolute;
    transform: rotateY(180deg);
}
```
<br><br><br>

## Posedetector component

In dit component gebruiken we de `react-webcam` en laden we het `tasks-vision` model voor detectie van handen.

```js
import Webcam from 'react-webcam'
import { useEffect, useRef, useState } from "react";
import { PoseLandmarker, HandLandmarker, FilesetResolver, DrawingUtils } from "@mediapipe/tasks-vision";

const videoConstraints = { width: 480, height: 270, facingMode: "user" }

function Posedetector({ onPoseDataUpdate }) {
    const [poseData, setPoseData] = useState([])
    const webcamRef = useRef(null)
    const landmarkerRef = useRef(null)

    // capture de webcam stream en ontvang posedata
    const capture = async() => {
        //...
    }

    // laad het landmarker model in de landmarkerRef
    useEffect(() => {
        //...
    }, [])


  return (
    <Webcam
      width="480"
      height="270"
      mirrored
      id="webcam"
      audio={false}
      videoConstraints={videoConstraints}
      ref={webcamRef}
    />
  )
}
export default Posedetector
```
Plaats de webcam en het canvas element boven elkaar. Omdat de webcam is gespiegeld moet je het x coordinaat ook spiegelen in de CSS.




<br><br><br>

## Landmarker laden

In het `PoseDetection` component laden we de landmarker in `useEffect`. Sla een referentie naar het model op met `useRef`. 

```js
 useEffect(() => {
    const createHandLandmarker = async () => {
        const vision = await FilesetResolver.forVisionTasks("https://cdn.jsdelivr.net/npm/@mediapipe/tasks-vision@0.10.0/wasm");
        const handLandmarker = await HandLandmarker.createFromOptions(vision, {
            baseOptions: {
                modelAssetPath: `https://storage.googleapis.com/mediapipe-models/hand_landmarker/hand_landmarker/float16/1/hand_landmarker.task`,
                delegate: "GPU"
            },
            runningMode: "VIDEO",
            numHands: 2
        });
        landmarkerRef.current = handLandmarker
        console.log("handlandmarker is created!")
        capture()
    };
    createHandLandmarker()
}, []);
```

<br><br><br>

## Start detectie landmarks

De `capture` functie update de state als er nieuwe punten zijn. Je kan de `capture` functie voor het eerst aanroepen nadat je model is geladen. Zodra er poses zijn wordt de state geupdate. 

```js
const capture = async() => {
    if (webcamRef.current && landmarkerRef.current && webcamRef.current.getCanvas()) {
        const video = webcamRef.current.video
        if (video.currentTime > 0) {
            const result = await landmarkerRef.current.detectForVideo(video, performance.now())
            if(result.landmarks) {
                // stuur de posedata naar het App component:
                onPoseDataUpdate(result.landmarks)
            }
        }
    }
    requestAnimationFrame(capture)
}
```


<br><br><br>

# Posedata testen

Het `App` component krijgt 60 keer per seconde data van 2 handen, dit zijn per hand 21 punten met elk een `x,y,z` coordinaat. Om te testen of dit goed binnenkomt maken we een `Coordinates` component die de getallen laat zien. Vanuit `App` geven we de `poseData` aan de child components.

```jsx
function Coordinates({ poseData }) {
    return (
        <section>
            <h1>Handen</h1>
            <div>
                {poseData.map((hand, i) => (
                    <div key={i}>
                        <h2>Hand {i + 1}</h2>
                        <ul>
                            {hand.map((landmark, j) => (
                                <li key={j}>
                                    {landmark.x}, {landmark.y}, {landmark.z}
                                </li>
                            ))}
                        </ul>
                    </div>
                ))}
            </div>
        </section>
    )
}
export default Coordinates
```
Geef de posedata door aan het component vanuit `App`:
```html
<Coordinates poseData={poseData}/>
```
<br><br><br>

# Posedata tekenen

Als het `Coonrdinates` component werkt weten we zeker dat we data van MediaPipe binnen krijgen. Dit gaan we tekenen in het `CanvasDrawing` component: Je hebt een `useRef` nodig om een referentie naar het canvas te krijgen. 

```jsx
import { useEffect, useRef, useState } from "react";
import { PoseLandmarker, HandLandmarker, FilesetResolver, DrawingUtils } from "@mediapipe/tasks-vision";

function CanvasDrawing({ poseData }) {
    const canvasRef = useRef(null)
    const drawingUtilsRef = useRef(null)

    useEffect(() => {
        const ctx = canvasRef.current.getContext('2d')
        drawingUtilsRef.current = new DrawingUtils(ctx)
    }, [])

    useEffect(() => {
        const ctx = canvasRef.current.getContext('2d')
        if(drawingUtilsRef.current) {
            ctx.clearRect(0, 0, 480, 270)
            for (const hand of poseData) {
                drawingUtilsRef.current.drawConnectors(hand, HandLandmarker.HAND_CONNECTIONS, {color: "#00FF00",lineWidth: 5});
                drawingUtilsRef.current.drawLandmarks(hand, { radius: 4, color: "#FF0000", lineWidth: 2 });
            }
        }
    }, [poseData]);

    return (
        <canvas ref={canvasRef} width="480" height="270"></canvas>
    )
}

export default CanvasDrawing
```


> *In React StrictMode kan in de dev server `useEffect` onverwacht twee keer worden aangeroepen. In de `build` versie krijg je deze error niet.*

<br><br><br>

## RequestAnimationFrame en React state

Bovenstaande code gebruikt een `state` om een grote hoeveelheid poseData heel vaak te verversen. Dit kan React langzaam maken omdat `state` niet persé voor animatie bedoeld is. Je kan ervoor kiezen om de regel `requestAnimationFrame(capture)` in `PoseDetector` minder vaak uit te voeren, bv. 10 keer per seconde:

```js
const targetFPS = 30; 
const interval = 1000 / targetFPS; 
let lastTime = 0; 

function Posedetector({ onPoseDataUpdate }) {
  const capture = async (time) => {
    if (webcamRef.current && landmarkerRef.current && webcamRef.current.getCanvas()) {
      if (time - lastTime >= interval) {
        lastTime = time
        // ... detection code
      }
    }
    requestAnimationFrame(capture)
  }
```

Een andere oplossing kan zijn om `useState` te vervangen door `useRef`. Je moet dan wel handmatig het canvas telkens verversen met nieuwe data uit `useRef`.


<br><br><br>
 
## Documentatie: 

- https://developers.google.com/mediapipe/solutions/setup_web
- https://www.npmjs.com/package/@mediapipe/tasks-vision
- https://ai.google.dev/edge/api/mediapipe/js/tasks-vision#tasks_vision_package
