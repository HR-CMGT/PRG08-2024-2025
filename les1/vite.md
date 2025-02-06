# MediaPipe Vite project

Bouw de hand detection app met Vite. Door mediapipe te installeren in `node_modules` krijg je betere type checking in je code editor. Maak een vite project aan:

```sh
npm create vite@latest
cd mycoolproject
npm install
npm install @mediapipe/tasks-vision@0.10.18
```

> ⚠️ De voorbeeldcode uit de lessen gebruikt versie 0.10.18, omdat in versie 0.10.20 een [issue](https://github.com/google-ai-edge/mediapipe/issues/5828) heeft met de DrawingUtils.

<br>

Import de mediapipe modules

```js
import { HandLandmarker, FilesetResolver, DrawingUtils } from "@mediapipe/tasks-vision";
```
De rest van de applicatie blijft hetzelfde als in het [HTML boilerplate project](./boilerplate)
Let op dat de WASM file en het AI Model nog steeds via een CDN worden geladen. Start het Vite project:

```sh
npm run dev
```

<Br><br><br>

## Expert level

Je kan ook een [React MediaPipe](https://github.com/HR-CMGT/PRG08-2024-2025/blob/main/snippets/react.md) project bouwen.

<Br><br><br>

## Links

- [Javascript Reference](https://ai.google.dev/edge/api/mediapipe/js/tasks-vision#tasks_vision_package)
- [Documentatie](https://ai.google.dev/edge/mediapipe/solutions/vision/hand_landmarker/web_js)

