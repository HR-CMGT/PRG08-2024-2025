# KNN in React

Voorbeeld van een React component met het KNN algoritme

```sh
npm install knear
```

Het model wordt getraind in `useEffect` en opgeslagen in een `useRef`.

```js
import { useState, useEffect, useRef, useCallback } from 'react'
import knn from 'knear'

export default function App() {
    const [prediction, setPrediction] = useState(undefined)
    const machine = useRef(new knn.kNear(3)) // of new kNear(3)    

    const makePrediction = () => {
        const result = machine.current.classify([3,5,4])
        setPrediction(result)
    }

    useEffect(() => {
        machine.current.learn([1, 2, 3], 'cat')
        machine.current.learn([0, 0, 0], 'cat')
        machine.current.learn([14, 10, 9], 'dog')
        machine.current.learn([9, 12, 13], 'dog')
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