# ML5 Neural Network in React

Je hebt data verzameld uit MediaPipe. Vervolgens train je een model. Dit model laad je in de React Applicatie zodat je voorspellingen kan doen.

## Trainen

*Het trainen van het model hoef je niet in React te doen*, aangezien daar geen UI voor nodig is. Als het trainen klaar is krijg je een `model`. Dit bestaat uit 3 files. Deze plaats je in de `public` folder van je `Vite` project:

```sh
public
    └── model
        ├── model.json
        ├── model_meta.json
        └── model.weights.bin
```

<br><br><br>

## React UI bouwen

De ML5 library geeft errors als je dit probeert te installeren met `npm i ml5`. De oplossing lijkt te zijn om toch gewoon een `<script>` tag te gebruiken in `index.html`. 

```html
<script src="https://unpkg.com/ml5@latest/dist/ml5.min.js"></script>
```
<br><br><br>

## Predictions

In de `useEffect` wordt het model geladen en opgeslagen in een `useRef`. Vanaf dat moment kan je predictions gaan doen. 

```js
import { useEffect, useRef, useState } from "react";

const testdata = [
    0.6788536310195923,
    0.7712749242782593,
    0.6037616729736328,
    0.725912868976593
]

export default function App() {

    const [prediction, setPrediction] = useState(undefined)
    const nn = useRef(null)  

    const makePrediction = () => {
        const input = testdata
        nn.current.classify(input, (error, result)=>{
            setPrediction(`I think this is ${result[0].label}`)
        })
    }

    useEffect(() => {
        console.log('ml5 version:', window.ml5.version)
        const network = window.ml5.neuralNetwork({ task: 'classification', debug: true })
        const modelDetails = {
            model: 'model/model.json',
            metadata: 'model/model_meta.json',
            weights: 'model/model.weights.bin'
        }
        network.load(modelDetails, () => {
            console.log('model loaded')
            nn.current = network
        })
    }, [])

    return (
        <>
            <button onClick={makePrediction}>Make Prediction</button>
            <p>The prediction is {prediction}</p>
        </>
    )
}
```
> *In React StrictMode kan in de dev server `useEffect` onverwacht twee keer worden aangeroepen. In de `build` versie krijg je deze error niet.*