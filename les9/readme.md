# les 9

## Verbeteren chatbot

- Opmaak en code antwoorden van een LLM correct tonen
- Externe API's toevoegen zoals het weerbericht. Het LLM gebruiken om daar iets over te zeggen.
- Function calling
- Serverless functies voor live hosting op sites zoals Vercel.com.
- Spraak en spraakherkenning van de browser
- Spraak van OpenAI
- Expert links

<br><br><br>

## Opmaak en code tonen

Een LLM kan tekst met kopjes, vette tekst en lijstjes terug geven. Daarnaast kan ook correct opgemaakte code terugkomen waarin de spaties belangrijk zijn. Om dit correct te tonen moet je een markdown converter gebruiken die het antwoord van een LLM kan tonen als markdown, in plaats van plain text.

#### Voorbeeld ShowdownJS

```js
let promptresult = fetch("Can you write a simple `hello world` react component?")
let converter = new showdown.Converter()
document.querySelector("#div").innerHTML = converter.makeHtml(promptresult);
```

- [ShowDown](https://showdownjs.com)
- [Marked](https://marked.js.org)

<br><Br><br>

## Externe API's toevoegen

Een LLM bevat geen live informatie over bijvoorbeeld het *recente nieuws, het weer, sportuitslagen, etc.* Dit kan je oplossen door zelf een API call te doen en het resultaat daarvan door het LLM te laten uitleggen. In dit voorbeeld van een `weather app` krijg je advies over kleding die past bij het huidige weer:

```js
const weatherApiKey = 'YOUR_WEATHER_API_KEY';
const city = 'Rotterdam';
const apiUrl = `https://api.openweathermap.org/data/2.5/weather?q=${city}&appid=${weatherApiKey}&units=metric`;

async function getWeather() {
  const response = await fetch(apiUrl)
  return await response.json()
}

async function talkAboutWeather() {
  const data = await getWeather();
  console.log(`City: ${data.name}`);
  console.log(`Temperature: ${data.main.temp}°C`);
  console.log(`Weather: ${data.weather[0].description}`);

  const result = await model.invoke(`Geef mij kledingadvies voor het weer ${data.weather[0].description} met een temperatuur van ${data.main.temp}`)
  console.log(result.content)
}

// Laat het taalmodel praten over het weer
talkAboutWeather();
```

- [Lijst van coole API's](https://apilist.fun)

<br><br><br>

## Function calling (tools)

Je zal bij het gebruiken van taalmodellen gemerkt hebben dat deze niet óveral goed in zijn, bv:

-	Berekeningen maken
-	Belasting aangifte doen
-	Het weerbericht tonen
-	Sportuitslagen tonen
-	Op google zoeken
-	Het licht aan doen (smarthome)
-	Dobbelstenen gooien
-	Een robot besturen
-	CRUD functies (lezen en schrijven naar een database)
-	Een grafiek tekenen die echt klopt
-	Inloggen op een beveiligde website

Het zou natuurlijk heel handig zijn als een taalmodel begrijpt wanneer een van deze situaties van toepassing is, en dat het model dan zelf een javascript functie kan aanroepen waarin deze functionaliteit correct uitgevoerd kan worden.

Dit concept heet `function calling / tool calling / agents`. Het taalmodel krijgt nu toegang tot javascript functies in jouw project en gaat zelf bedenken wanneer deze functies aangeroepen moeten worden. 

[Bekijk hier een compleet code voorbeeld voor het werken met tools in een taalmodel](../snippets/functions.md)

<br>

## Ideëen voor applicaties

-	Je haalt data op van een externe API (inspiratie: https://apilist.fun)
-	Je logt in op een beveiligde website (dit kan je eigen CRUD applicatie zijn)
-	Je stuurt een externe applicatie aan (bv. een app van https://ifttt.com, je [raspberry pi smarthome](https://www.home-assistant.io/installation/raspberrypi/), of je eigen javascript game)
-	Je doet een zoekopdracht op de LLM search engine https://tavily.com
-	Zeg tegen je [fotolijstje](https://www.youtube.com/watch?v=L5PvQj1vfC4) dat je vandaag een foto van een kat met een piratenhoedje wil zien.

<br>

### Code voorbeelden

- [Code Voorbeeld: laat het taalmodel een berekening maken](../snippets/functions.md)
- [Een tool definiëren](https://js.langchain.com/docs/concepts/tools/)
- [Een tool automatisch laten uitvoeren door het taalmodel](https://js.langchain.com/docs/concepts/tool_calling/)
- [OpenAI LangChain Function Calling](https://js.langchain.com/docs/integrations/chat/openai)
- [OpenAI Agents in Langchain](https://js.langchain.com/docs/modules/agents/)



<br><br><br>

## Serverless functies

Je kan je project live zetten op een webserver, als die webserver `node` ondersteunt en ook continu de `node server` kan draaien. Dit kan je doen op je HR studentenhosting. Het nadeel is dat je zelf moet monitoren wanneer de server is uitgevallen.

Er zijn ook online ***node hosting*** providers te vinden zoals *`vercel.com`, `netlify.com`, `render.com`, `codesandbox.com`, `github codespaces`, `huggingface spaces`, `stackblitz.com`, `deno.com`, `amazon serverless webservices`, etc...*. Deze maken vaak gebruik van ***serverless functions*** zodat er geen live node server hoeft te draaien. 

#### Voorbeeld

Maak een vercel project als volgt aan:

```
PROJECT FOLDER
├── index.html
├── index.js
└── API
      └── hello.js
```
In `api/hello.js` plaats je de serverless code:

```js
import type { VercelRequest, VercelResponse } from '@vercel/node'

export default function handler(req, res) {
  const { name = 'World' } = req.query
  return res.json({
    message: `Hello ${name}!`,
  })
}
```
Vanuit `index.js` kan je de serverless function aanroepen:

```js
async function fetchGreeting(name) {
  try {
    const response = await fetch(`/api/hello?name=${name}`);
    const data = await response.json();
    console.log(data.message); // "Hello Action Henk!" 
  } catch (error) {
    console.error('Error fetching the API:', error);
  }
}

fetchGreeting('Action Henk');
```

#### Links

- [Hello World Serverless](https://vercel.com/templates/other/nodejs-serverless-function-express)
- [Vercel Serverless documentation](https://vercel.com/docs/functions)

<br><br><br>

## Spraak

De browser heeft spraak én spraakherkenning ingebouwd:

- [Spraak en spraakherkenning in de browser](https://github.com/HR-CMGT/PRG08-2024-2025/blob/main/snippets/speech.md)

### OpenAI Whisper

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


## Expert links

- [Annuleren van een OpenAI call](https://js.langchain.com/docs/modules/model_io/llms/cancelling_requests)
- [Tokens bijhouden](https://js.langchain.com/docs/modules/model_io/llms/token_usage_tracking)
- [Omgaan met errors](https://js.langchain.com/docs/modules/model_io/llms/dealing_with_api_errors)

