# Speech

- Spreken
- Spraakherkenning
- OpenAI Whisper

<br><br><bR>


## Browser spraak

Laat de browser een reactie uitspreken!

```javascript
let synth = window.speechSynthesis

function speak(text) {
    if (synth.speaking) {
        console.log('still speaking...')
        return
    }
    if (text !== '') {
        let utterThis = new SpeechSynthesisUtterance(text)
        synth.speak(utterThis)
    }
}

speak("Hello world")
```
<br><br><br>

## User interaction

⚠️ Speech werkt alleen als er een gebruikers interactie is geweest, bv. via een button:

```js
btn.addEventListener("click", () => {
  speak(`I think it's a hamster!`)
})
```
<br><br><br>

## Voices

Je kan verschillende voices gebruiken. Zie de [getvoices documentatie](https://developer.mozilla.org/en-US/docs/Web/API/SpeechSynthesis/getVoices).

In dit voorbeeld tonen we alle beschikbare stemmen. De beschikbare stemmen verschillen per OS (Mac,Windows,Linux) en per browser (Safari, Edge, Chrome).

```js
let voices = window.speechSynthesis.getVoices()
for(let voice of voices) {
    console.log(`${voice.name} (${voice.lang})`)
}
```
In dit voorbeeld filteren we alle Nederlandse stemmen uit de lijst en gebruiken de eerste als stem.
```js
let dutchVoices = voices.filter(voice => voice.lang === "nl-NL")
let utterThis = new SpeechSynthesisUtterance(text)
utterThis.voice = dutchVoices[0] // de eerste dutch voice uit de lijst
synth.speak(utterThis)
```

- [Op MacOS kan je je eigen stem toevoegen aan de beschikbare voices!](https://support.apple.com/en-gb/guide/mac-help/mchldfd72333/mac)
- Het duurt even voordat de voices beschikbaar zijn. Je kan dit checken met `synth.addEventListener("voiceschanged", () => {})`

<br>
<Br>
    
## Form

Je kan een inputField en de button toevoegen om het te testen met verschillende teksten

```html
<input type="text" id="inputfield">
<button id="playbutton">Play</button>
```

```javascript
let inputField = document.querySelector("#inputfield")
let playButton = document.querySelector("#playbutton")

playButton.addEventListener("click", () => {
    let text = inputField.value
    speak(text)
})
```


<br>
<br>
<br>

## Spraakherkenning

Je kan de gebruiker een tekst laten inspreken, de tekst wordt dan herkend. Let op, het effect verschilt enorm per browser. Je spraak kan naar de servers van Google of Apple gestuurd worden.

```js
var SpeechRecognition = SpeechRecognition || webkitSpeechRecognition;
var SpeechRecognitionEvent = SpeechRecognitionEvent || webkitSpeechRecognitionEvent;
// var aanpassen

const testBtn = document.querySelector('button')
testBtn.addEventListener('click', () => startListening())

function startListening() {
    testBtn.disabled = true

    let recognition = new SpeechRecognition()
    //recognition.lang = 'en-US'
    recognition.lang = 'nl-NL' // nl werkt niet in alle browsers
    recognition.interimResults = false
    recognition.maxAlternatives = 1
    recognition.start()

    recognition.addEventListener("result", (event) => checkResult(event))

    recognition.onspeechend = function () {
        recognition.stop()
        testBtn.disabled = false
    }

    recognition.onerror = function (event) {
        testBtn.disabled = false
        console.log(event.error)
    }
}

function checkResult(event) {
    let speechResult = event.results[0][0].transcript.toLowerCase()
    console.log('🚨' + speechResult)
    console.log('Confidence: ' + event.results[0][0].confidence)
}
```

<br><br><br>

# OpenAI Whisper

Je kan je mp3 file naar *OpenAI Whisper* sturen om spraak om te zetten naar tekst. Helaas werkt dit nog niet via Azure, dus je moet een eigen `OPENAI_API_KEY` in je `.env` plaatsen. Dit voorbeeld is in ***Langchain***:

```bash
npm install langchain @langchain/community openai
````

```js
import { OpenAIWhisperAudio } from "@langchain/community/document_loaders/fs/openai_whisper_audio";

const options = {
    apiKey: process.env.OPENAI_API_KEY
}
const loader = new OpenAIWhisperAudio("hello.mp3", options);
const docs = await loader.load();
console.log(docs[0].pageContent);
```
- [Bekijk hier het voorbeeld in de OpenAI docs](https://platform.openai.com/docs/guides/speech-to-text)

<br><br><br>

## Links

- [MDN speech synthesizer API](https://developer.mozilla.org/en-US/docs/Web/API/SpeechSynthesis)
- [MDN speech recognition API](https://developer.mozilla.org/en-US/docs/Web/API/Web_Speech_API/Using_the_Web_Speech_API)
- [MDN speech code examples](https://github.com/mdn/dom-examples/tree/main/web-speech-api)
- [Synthesizer API](https://developer.mozilla.org/en-US/docs/Web/API/OscillatorNode)
- [Web Audio API](https://developer.mozilla.org/en-US/docs/Web/API/Web_Audio_API)	
