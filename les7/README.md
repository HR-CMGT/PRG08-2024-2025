# Les 7

## Prompt engineering

In de vorige les heb je een omgeving opgezet waarin je een een taalmodel kan aanroepen, en je hebt een user interface gebouwd zodat de gebruiker kan chatten met het taalmodel. In deze les gaan we kijken wat voor creatieve toepassingen we hiervoor kunnen bedenken, en wat een taalmodel wel en niet kan doen.

- Hoe slim is een taalmodel
- Prompt engineering
- Context window en chat history
- Toepassing bedenken

<br><br><br>

## Hoe slim is een taalmodel

Zie de presentatie

<br><br><br>

## Prompt engineering

De reden dat we zelf een chat applicatie opzetten, is dat we instructies aan het prompt kunnen toevoegen die passen bij het nut van de applicatie. Als de gebruiker in het invoerveld typt: "hoe werkt het zonnestelsel", dan kan je vanuit je applicatie nog wat extra instructies toevoegen voordat je de prompt naar het taalmodel stuurt. Bijvoorbeeld:

```js
let field = document.querySelector("#textfield")
let userPrompt = field.value
let engineeredPrompt = `You are a teacher for children at the age of 5 to 10. Explain your answers as short as possible. The question is ${userPrompt}`
let answer = await model.invoke(engineeredPrompt)
console.log(answer.content)
```

<br><br><br>

## Context window en chat history

In de voorgaande oefeningen hebben we telkens een enkele prompt naar het model gestuurd. Hierbij moet je er rekening mee houden dat de voorgaande discussie niet automatisch onthouden wordt door het model. Een vraag zoals *"Wat bedoel je daarmee*" gaat dus niet altijd goed. 

Als je een dialoog wil voeren moet je telkens de hele discussie meesturen met je prompt. Dit doe je in de vorm van een `array` waarin je de `chat history` bijhoudt. Per entry in de `chat` array geef je aan wie de uitspraak gedaan heeft. Het model snapt dan beter hoe de conversatie gevoerd moet worden. In dit voorbeeld zie je dat het antwoord van het LLM aan de array wordt toegevoegd, samen met de vervolgvraag.

```js
let messages = [
    ["system", "You are a neanderthal assistant. End every sentence with a different random grunt"],
    ["human", "How is the solar system composed?"],
]
// question 1
const chat1 = await model.invoke(messages)
console.log(chat1.content)

// question 2
messages.push(["assistant", chat1.content], ["human", "Can you explain that last word you said?"])
const chat2 = await model.invoke(messages)
console.log(chat2.content)
```

<br><br><br>

## Toepassing bedenken

- [Bekijk alvast opdracht 2 voor inspiratie voor een taalmodel toepassing!](https://github.com/HR-CMGT/PRG08-2024-2025/blob/main/opdracht2.md)
