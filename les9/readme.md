# les 9

## Verbeteren chatbot

- Opmaak tonen
- Serverless hosting
- Externe API
- Tools
- Spraak 
- Expert links

<br><br><br>

## Opmaak tonen

Een LLM kan markdown tekst met kopjes, vette tekst en lijstjes terug geven. Daarnaast kan ook correct opgemaakte code terugkomen waarin de spaties belangrijk zijn. Om dit correct te tonen moet je een markdown converter gebruiken die het antwoord van een LLM kan tonen als markdown, in plaats van plain text.

#### Voorbeeld ShowdownJS

```js
let promptresult = fetch("Can you write a simple `hello world` react component?")
let converter = new showdown.Converter()
document.querySelector("#div").innerHTML = converter.makeHtml(promptresult);
```

- [ShowDown](https://showdownjs.com)
- [Marked](https://marked.js.org)

<br><Br><br>


## Serverless hosting

Je kan je project live zetten op een webserver, als die webserver `node` ondersteunt en een `node server` kan draaien. Dit kan je doen op je HR studentenhosting. Het nadeel is dat je zelf moet monitoren wanneer de server is uitgevallen.

Er zijn ook online ***node hosting*** providers te vinden zoals [vercel.com](https://vercel.com), [netlify.com](https://netlify.com), [render.com](https://render.com), [codesandbox.com](https://codesandbox.com), [github codespaces](https://github.com/features/codespaces), [huggingface spaces](https://huggingface.co/spaces), [stackblitz.com](https://stackblitz.com), [deno.com](https://deno.com), [amazon serverless webservices](https://aws.amazon.com/serverless/), etc. Deze maken gebruik van ***serverless functions*** zodat er geen live node server hoeft te draaien.

> 🔥 [Bekijk dit code voorbeeld voor het werken met serverless](../snippets/serverless.md)

<br><br><br>

## Externe API's toevoegen

Een LLM bevat geen live informatie over bijvoorbeeld het *recente nieuws, het weer, sportuitslagen, etc.* Dit kan je oplossen door zelf een API call te doen en het resultaat daarvan door het LLM te laten uitleggen. 

Dit is handig als je al vantevoren weet welke informatie de gebruiker nodig gaat hebben, bijvoorbeeld in een *weather app*.

```js
const weatherApiKey = 'YOUR_WEATHER_API_KEY';
const apiUrl = `https://api.openweathermap.org/data/2.5/weather?q=rotterdam&appid=${weatherApiKey}&units=metric`;

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

## Tools

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

Bij het aanmaken van een taalmodel kan je ook functies doorgeven die dit wel kunnen. Hierdoor kan je taalmodel uit zichzelf die functie aanroepen als het taalmodel vindt dat dit nodig is.

[Code voorbeeld voor het werken met tools in een taalmodel](../snippets/functions.md)

<br>

### Tool applicaties

-	Je haalt data op van een externe API (inspiratie: https://apilist.fun)
-	Je logt in op een beveiligde website (dit kan je eigen CRUD applicatie zijn)
-	Je stuurt een externe applicatie aan (bv. een app van https://ifttt.com, je [raspberry pi smarthome](https://www.home-assistant.io/installation/raspberrypi/), of je eigen javascript game)
-	Je doet een zoekopdracht op de LLM search engine https://tavily.com
-	Zeg tegen je [fotolijstje](https://www.youtube.com/watch?v=L5PvQj1vfC4) dat je vandaag een foto van een kat met een piratenhoedje wil zien.

<br>

### Tool voorbeelden

- [Code Voorbeeld: laat het taalmodel een berekening maken](../snippets/functions.md)
- [Een tool definiëren](https://js.langchain.com/docs/concepts/tools/)
- [Een tool automatisch laten uitvoeren door het taalmodel](https://js.langchain.com/docs/concepts/tool_calling/)
- [OpenAI LangChain Function Calling](https://js.langchain.com/docs/integrations/chat/openai)
- [OpenAI Agents in Langchain](https://js.langchain.com/docs/modules/agents/)



<br><br><br>

## Spraak

[De browser heeft spraak én spraakherkenning ingebouwd](https://github.com/HR-CMGT/PRG08-2024-2025/blob/main/snippets/speech.md). Voor client apps is dit de handigste manier om met spraak te werken, omdat de audio verwerking nu helemaal aan de client-side is. Het nadeel is dat de stemmen erg wisselen per browser.

### AI Spraak

- [Voorbeeldcode spraakherkenning en text-to-speech](../snippets/audio.md)
- [Whisper code snippets](https://github.com/HR-CMGT/PRG08-2024-2025/blob/main/snippets/aispeech.md)
- [Elevenlabs API](https://elevenlabs.io/docs/api-reference/text-to-speech/convert)

<br><br><br>









## Expert links

- [Annuleren van een OpenAI call](https://js.langchain.com/docs/modules/model_io/llms/cancelling_requests)
- [Tokens bijhouden](https://js.langchain.com/docs/modules/model_io/llms/token_usage_tracking)
- [Omgaan met errors](https://js.langchain.com/docs/modules/model_io/llms/dealing_with_api_errors)

