# les 9

## Verbeteren chatbot

Deze les bevat onderwerpen voor het expert level van de opdracht.

- Opmaak en code antwoorden van een LLM correct tonen
- Externe API's toevoegen zoals het weerbericht. Het LLM gebruiken om daar iets over te zeggen.
- Serverless functies voor live hosting op sites zoals Vercel.com.
- Spraak en spraakherkenning van de browser
- Spraak van OpenAI
- Een LLM gebruiken als OS, bv. om een robot te besturen.

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

Een LLM bevat geen live informatie over bijvoorbeeld het *recente nieuws, het weer, sportuitslagen, etc.* Als je dus aan je taalmodel vraagt of je een paraplu mee moet nemen naar buiten, dan krijg je geen goed antwoord.

Dit kan je oplossen door zelf een API call te doen en het resultaat daarvan door het LLM te laten uitleggen. Je kan bv. de JSON van een weer-app aan een LLM geven en vragen om kledingadvies te geven. 

### Function calling en JSON

Je kan detecteren of een gebruiker vraagt om bepaalde informatie, waarna je het LLM zelf een functie laat uitvoeren. Bv. als de gebruiker praat over het weer, dan weet het taalmodel dat de "call weather api" functie uitgevoerd moet worden. Dit heet "***Tool / function calling***"

- [Tool / Function calling with Langchain](https://js.langchain.com/docs/how_to/tool_calling/)

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
    console.log(data.message); // Logs "Hello [name]!" from your API
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

## Browser spraak

- TODO

<br><br><br>

## OpenAI spraak

- TODO
