# Les 5

- Accuracy berekenen
- Accuracy verbeteren
- Confusion matrix
- Data bronnen
- Troubleshooting

<br><br><br>

# Accuracy

Tot nu toe hebben we de nauwkeurigheid van de voorspelling van het `K-Nearest-Neighbour` en het `Neural Network` getest door live voor de webcam poses te classifyen, en te kijken of de voorspelling klopt. Maar we weten niet precies hoe vaak een correcte voorspelling gedaan wordt.

## Testdata 

Met MediaPipe kan je makkelijk een nieuwe dataset maken die je gaat gebruiken om te testen. Als je al heel veel posedata had verzameld, kan je ook je bestaande data splitsen in traindata en testdata.

### Splitsen in train en testdata

In dit voorbeeld shufflen we onze data en splitsen we het op in twee datasets. Het shufflen (`sort`) is nodig omdat anders alle `rocks` en `scissors` in de traindata komen en alle `papers` in de testdata. Het getal `0.8` betekent dat 80% van de data is om te trainen, en 20% is om te testen.

```js
const data = [
    {pose:[4,2,5,2,1,...], label:"rock"},
    {pose:[3,1,4,4,1,...], label:"rock"},
    ...
]
data.sort(() => (Math.random() - 0.5))

const train = data.slice(0, Math.floor(data.length * 0.8))
const test = data.slice(Math.floor(data.length * 0.8) + 1)
```

> *🚨 Let op dat zowel je traindata als je testdata voldoende voorbeelden van elke pose bevat.*

<br><br><br>

## Accuracy uitrekenen

We trainen het model nu alleen met de `train` dataset, zoals je ook gedaan hebt in les 6 en 7. Vervolgens ga je voor elk datapunt in je `test` dataset een voorspelling doen. Omdat je voor de `test` data ook het label beschikbaar hebt, kan je vergelijken of de voorspelling overeenkomt met het echte label. Hier zie je een voorbeeld met 1 pose uit de testdata:

```js
const testpose = {pose:[3,4,6], label:"rock"}
const prediction = await nn.classify(testpose.pose)
console.log(`Ik voorspelde: ${prediction[0].label}. Het correcte antwoord is: ${testpose.label}`)
```

Maak een variabele aan waarin je bijhoudt *hoe vaak* de voorspelling overeenkomt met het echte label. Toon als percentage hoeveel voorspellingen van het totaal goed zijn gegaan! 

```
let accuracy = correctpredictions / totaltestposes
```

<br><br><br>

### Expert level: Confusion matrix

Als je nóg preciezer wil kunnen bepalen hoe accuraat je voorspelling is kan je werken met een confusion matrix. Hierbij ga je kijken hoe goed elk van je classes voorspeld wordt. *In dit voorbeeld van `rock, paper, scissors` zie je dat rock 2 keer als paper werd voorspeld en 0 keer als scissors. Dat is goed! Maar scissors werd 10 keer als paper gezien, dus wellicht heb je meer `scissors` voorbeelden in je data nodig.*

| real ➡️ <br> predict ⬇️  | Rock | Paper | Scissors |
| --- | ---  |  --- |  ---- |
| Rock | 20 | 3 | 3 |
| Paper | 2 | 18 | 10 |
| Scissors | 0 | 1 | 19 |

<br><br><br>

## Accuracy verbeteren

Een algoritme heeft vaak instellingen waarmee je kan experimenteren *(zgn. hyperparameters)*. In deze les ga je kijken of jouw voorspellingen beter worden nadat je deze instellingen hebt aangepast. 

<br><br><br>

### K-Nearest-Neighbour

Je kan aangeven met *hoeveel* neighbours een punt vergeleken moet worden, dit is de `K` variabele. Experimenteer of je accuracy verbetert als je `K` aanpast.

```js
let k = 3
let machine = new knn.kNear(k)
```
### Neural networks

Een neural network heeft veel [parameters](https://learn.ml5js.org/#/reference/neural-network?id=arguments-for-ml5neuralnetworkoptions) die je kan aanpassen. 

- Het aantal `epochs` bepaalt hoe lang het model moet proberen zichzelf te verbeteren.
- De `learning rate` bepaalt hoe snel het model zichzelf corrigeert.
- De `hidden units` bepaalt hoe groot het default hidden layer is.

```js
const nn = ml5.neuralNetwork(options, dataLoaded);
const trainingOptions = {
    epochs: 32,
    learningRate: 0.2,
    hiddenUnits: 16,
}
nn.train(trainingOptions, finishedTraining);
```
### Expert level: Hidden layers

Via het `layers` argument kan je zelf meer hidden layers aan het neural network toevoegen!
De vorm van deze `hidden layers` bepaalt hoe complex de patronen in de data kunnen zijn, om toch nog herkend te worden door het neural network.

[Zie deze tutorial](../snippets/layers.md)



<br>
<br>
<br>

# Expert level: Data bronnen

We hebben tot nu toe ons algoritme (`K-Nearest-Neighbour` en `Neural Network`) getraind met posedata uit de webcam. De data bestaat uit een array van getallen, zoals `[4,3,5,2,1]`, vergezeld van een label, zoals `rock`. Voor het algoritme maakt het niets uit dat dit *poses* zijn, want elk soort data kan je weergeven als een array met een label. Hieronder een aantal voorbeelden.

| Subject | Data | Label |
| ------- | ---- | ----- |
| Animals |    `[fangs, eggs, legs]` | `mammal` |
| Mushrooms |  `[color, size, weight]` |`poisonous` |
| Titanic passenger | `[Female, age, class]` | `survived` |

#### Oefening

Zoek een [dataset voor classification](https://www.kaggle.com/datasets?tags=13302-Classification), en maak hier een model en voorspelling mee.

<br>
<br>
<br>

# Troubleshooting

Als je merkt dat de accuracy laag blijft, dan heeft dit waarschijnlijk met de kwaliteit van je data te maken:

- Er zijn te weinig voorbeelden van elke pose in de train én de test set.
- Je testset bevat andere poses dan je train set.
- De posedata is van lage kwaliteit doordat de poses niet duidelijk zijn, te ver weg van de webcam, of er is te weinig variatie in de poses.
- Er is iets mis gegaan bij het opslaan van de posedata. Niet elke pose heeft evenveel getallen, of je hebt getallen opgeslagen als strings. (bv. `pose="5,2,5,2"`)
- De dataset bevat irrelevante data. In het geval van posedata zou je kunnen kijken of de `z` waarde van een pose echt nodig is om een pose te herkennen.

<br>
<br>
<br>

## Documentatie

- [Prepareren van data met map en filter](https://github.com/HR-CMGT/PRG08-2020-2021/blob/main/snippets/csv.md).
- [ML5 Neural Networks in Javascript](https://learn.ml5js.org/#/reference/neural-network)
- [Werken met hidden layers in ML5](./snippets/layers.md)
- [Datasets voor classification](https://www.kaggle.com/datasets?tags=13302-Classification)
