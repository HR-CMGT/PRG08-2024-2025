# les 9

## Verbeteren chatbot

- Opmaak tonen
- Serverless hosting
- Externe API
- Tools
- Spraak 
- Extra modellen uitproberen
- Expert links

<br><br><br>

## Opmaak tonen

Een LLM kan markdown tekst met kopjes, vette tekst en lijstjes terug geven. Daarnaast kan ook correct opgemaakte code terugkomen waarin de spaties belangrijk zijn. Om dit correct te tonen moet je een markdown converter gebruiken die het antwoord van een LLM kan tonen als markdown, in plaats van plain text.

#### Code voorbeeld ShowdownJS

```js
const promptResult = await fetch("Can you write a simple `hello world` react component?");
const converter = new showdown.Converter();
document.querySelector("#div").innerHTML = converter.makeHtml(promptResult);
```

- [Live voorbeeld CMGT assistant](https://ai-assistent-mu.vercel.app)
- [ShowDown](https://showdownjs.com)
- [Marked](https://marked.js.org)


#### ShowdownJS in React

In React werkt dit niet, omdat JSX (net als Blade) htmlentities gebruikt bij variabelen om XSS te voorkomen. Je moet daarvoor expliciet aangeven dat je weet dat je iets 'dangerous' doet.

```js
// sla de response op in een state variable 'promptResult', en maak een converter aan (zie voorbeeld hierboven)
<div dangerouslySetInnerHTML={{ __html: converter.makeHtml(promptResult) }}></div>
```

<br><Br><br>


## Serverless hosting

Je kan je project live zetten op een webserver, als die webserver `node` ondersteunt en een `node server` kan draaien. Dit kan je doen op je HR studentenhosting. Het nadeel is dat je zelf moet monitoren wanneer de server is uitgevallen.

Er zijn ook online ***node hosting*** providers te vinden zoals [vercel.com](https://vercel.com), [netlify.com](https://netlify.com), [render.com](https://render.com), [codesandbox.com](https://codesandbox.com), [github codespaces](https://github.com/features/codespaces), [huggingface spaces](https://huggingface.co/spaces), [stackblitz.com](https://stackblitz.com), [deno.com](https://deno.com), [amazon serverless webservices](https://aws.amazon.com/serverless/), etc. Deze maken gebruik van ***serverless functions*** zodat er geen live node server hoeft te draaien.

> 🔥 [Bekijk dit code voorbeeld voor het werken met serverless](../snippets/serverless.md)

<br><br><br>

## Data meegeven van een externe API

Een LLM bevat geen live informatie over bijvoorbeeld het *recente nieuws, het weer, sportuitslagen, etc.* Dit kan je oplossen door zelf een API call te doen. Het resultaat van die call geef je dan als prompt aan het taalmodel. Dit is handig als je al vantevoren weet welke informatie de gebruiker nodig gaat hebben, bijvoorbeeld in een *weather app*.

```js
// vantevoren weerdata ophalen
const response = await fetch(`https://api.openweathermap.org/data/2.5/weather?q=rotterdam&appid=${YOUR_WEATHER_API_KEY}&units=metric`)
const weatherdata = await response.json()
// weerdata meegeven in prompt zodat het taalmodel er iets over kan zeggen
const result = await model.invoke(`Geef mij kledingadvies voor het weer ${weatherdata.weather[0].description} met een temperatuur van ${weatherdata.weather[0].temp}`)
console.log(result.content)
```

- [Lijst van coole API's](https://apilist.fun)

<br><br><br>

## Tools

Een nieuwe toevoeging aan taalmodellen is dat zij zelf kunnen bepalen wanneer een externe API call *(bv. ophalen van het weerbericht)* nodig is voor de prompt van de gebruiker. Je gaat dan niet meer zelf de weerdata ophalen, maar je geeft de functie die dat doet aan het taalmodel. Dit heet ***"tool calling"***. 

-	Berekeningen maken
-	Een email sturen
- Een afspraak in google calendar plaatsen
- Een nummer afspelen op spotify
-	Belasting aangifte doen
-	Het weerbericht tonen
-	Sportuitslagen tonen
- Koers van aandelen, bitcoin of valuta 
-	Op google zoeken
-	Het licht aan doen (smarthome)
-	Een robot besturen
-	CRUD functies (lezen en schrijven naar een database)
-	Een grafiek tekenen die echt klopt
-	Inloggen op een beveiligde website

[Bekijk het code voorbeeld voor het werken met tools in een taalmodel](../snippets/functions.md)

<br><br><br>

## Spraak

[De browser heeft spraak én spraakherkenning ingebouwd](https://github.com/HR-CMGT/PRG08-2024-2025/blob/main/snippets/speech.md). Voor client apps is dit de handigste manier om met spraak te werken, omdat de audio verwerking nu helemaal aan de client-side is. Het nadeel is dat de stemmen erg wisselen per browser / OS.

### AI Spraak

- [OpenAI Whisper code snippets](https://github.com/HR-CMGT/PRG08-2024-2025/blob/main/snippets/aispeech.md)
- [Elevenlabs](https://elevenlabs.io/docs/api-reference/text-to-speech/convert)
- [Spark](https://sparkaudio.github.io/spark-tts)
- [Sesame](https://huggingface.co/sesame/csm-1b)
- [Kokoro](https://kokorottsai.com)

<br><br><br>

### Extra modellen uitproberen

Huggingface biedt een optie om via je [HuggingFace account gratis een beperkt aantal calls naar allerlei taalmodellen](https://huggingface.co/docs/inference-providers/en/index) te doen. 
Je hoeft dan geen creditcard te hebben.

###DeepSeek 3 Langchain***

```js
import { ChatOpenAI } from "langchain/chat_models/openai";

const model = new ChatOpenAI({
  configuration: {
    baseURL: "https://router.huggingface.co/novita/v3/openai",
    apiKey: "hf_xxxxxxxxxxxxxxxxxxxxxxxx",  // jouw huggingface key
  },
  modelName: "deepseek/deepseek-v3-0324",
  maxTokens: 500,
  streaming: false,
});

```

***DeepSeek 3 fetch***

```js
async function askQuestion() {
    const key = "your_huggingface_key"

    const response = await fetch(
        "https://router.huggingface.co/novita/v3/openai/chat/completions",
        {
            method: "POST",
            headers: {
                Authorization: `Bearer ${key}`,
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                messages: [
                    {
                        role: "user",
                        content: "What is the capital of the Netherlands?",
                    },
                ],
                max_tokens: 500,
                model: "deepseek/deepseek-v3-0324",
                stream: false,
            }),
        }
    );

    const data = await response.json();
    console.log(data);

}
```




<br><bR><bR>


## Expert links

- [Annuleren van een OpenAI call](https://js.langchain.com/docs/modules/model_io/llms/cancelling_requests)
- [Tokens bijhouden](https://js.langchain.com/docs/modules/model_io/llms/token_usage_tracking)
- [Omgaan met errors](https://js.langchain.com/docs/modules/model_io/llms/dealing_with_api_errors)

