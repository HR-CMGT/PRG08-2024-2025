# MediaPipe Vite project

Bouw de hand detection app met Vite. 

- [documentatie](https://ai.google.dev/edge/mediapipe/solutions/vision/hand_landmarker/web_js)

## Project setup

```sh
npm create vite@latest
cd mycoolproject
npm install
npm install @mediapipe/tasks-vision
npm install @mediapipe/hands @mediapipe/drawing_utils
```
Je kan de HTML, CSS en JS grotendeels kopiëren uit de [boilerplate code](./boilerplate/). De belangrijkste verschillen zijn dat je nu `import` statements kan gebruiken voor `drawConnectors, drawLandmarks` en `HAND_CONNECTIONS`. De `<script>` tags kan je uit de `html` file verwijderen.

Door mediapipe te installeren in `node_modules` krijg je betere type checking in je code editor.

De landmarker zelf komt nog steeds van een CDN. De reden is dat er ook een WASM file en een AI MODEL geladen moeten worden. Dit kan je ook zelf hosten maar dat vereist een boel extra setup.

```js
import { HandLandmarker, FilesetResolver } from "https://cdn.jsdelivr.net/npm/@mediapipe/tasks-vision@0.10.0";
import { drawConnectors, drawLandmarks } from '@mediapipe/drawing_utils';
import { HAND_CONNECTIONS } from '@mediapipe/hands';
```


