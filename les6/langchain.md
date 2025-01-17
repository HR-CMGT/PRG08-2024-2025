# Les 6 - deel 1

- Zorg dat je [NodeJS 22](https://nodejs.org/en) hebt geïnstalleerd.
- Installeer [JSONFormatter](https://chromewebstore.google.com/detail/json-formatter/bcjindcccaagfpapjjmafapmmgkkhgoa) of een andere browser extensie waarmee je JSON kan bekijken.

<br><br><br>

## Opzetten server.js

Maak een `SERVER` map aan, hierin plaats je `server.js`, `.env` en `.gitignore`: 

```
SERVER
├── .env
├── .gitignore
└── server.js
```
De `.gitignore` file:

```sh
node_modules
.env
```

De `.env` file:

```sh
OPENAI_API_TYPE=___
OPENAI_API_VERSION=___
OPENAI_API_BASE=___
AZURE_OPENAI_API_KEY=___
DEPLOYMENT_NAME=___
ENGINE_NAME=___
INSTANCE_NAME=___
```

> *🚨 Deze waarden haal je uit de presentatie. Deel de API keys en de ENV file niet met anderen. Plaats de API keys niet online.*

<br><br><br>

## Hello World

Maak server.js aan:

```js
console.log("hello world")
console.log(process.env.AZURE_OPENAI_API_KEY)
```

Start de file met `node`, zorg dat de `.env` file ook wordt meegenomen:

```sh
node --env-file=.env --watch server.js
```
Test of het inlezen van je `.env` is gelukt!

> ⚠️ *TIP: plaats je volledige node commando `node --env-file=.env --watch server.js` in de `scripts` van je `package.json` zodat je niet telkens dit hele commando hoeft te typen.*

> ⚠️ *Als `--env-file=` niet werkt, moet je je versie van `node` updaten.*


<br><br><br>

## OpenAI aanroepen

[Langchain](https://js.langchain.com/docs/get_started/introduction) is de API die we in `server.js` gaan gebruiken om te werken met Azure OpenAI (ChatGPT). 

```sh
npm install langchain @langchain/core
npm install @langchain/openai
```
Om de OpenAI API's te kunnen aanroepen moet de `.env` file zijn ingelezen. Vervolgens kan je een vraag stellen aan het chat model!

```js
import { ChatOpenAI } from "@langchain/openai"

const model = new ChatOpenAI({
    azureOpenAIApiKey: process.env.AZURE_OPENAI_API_KEY, 
    azureOpenAIApiVersion: process.env.OPENAI_API_VERSION, 
    azureOpenAIApiInstanceName: process.env.INSTANCE_NAME, 
    azureOpenAIApiDeploymentName: process.env.ENGINE_NAME, 
})

async function tellJoke() {
    const joke = await model.invoke("Tell me a Javascript joke!")
    console.log(joke.content)
}
tellJoke()
```


<br><br><br>





### Troubleshooting

- Als je "api key not found" error krijgt ben je waarschijnlijk vergeten de `.env` file te lezen.
- Zet het `type` van je project op `module` in `package.json` om imports te kunnen gebruiken. 
- 📃 De documentatie van Langchain en OpenAI verandert regelmatig. Als de voorbeeldcode uit deze repository een warning geeft moet je de officiële documentatie nalezen.
- Bijvoorbeeld: `import { ChatOpenAI } from "langchain/chat_models/openai"` is recent veranderd naar `import { ChatOpenAI } from "@langchain/openai";`
- De Azure API Key kan veranderen tijdens de lessen. Als je een "not authorized" error krijgt kan je in Teams kijken of er een nieuwe key is.

<br><br><br>

## Optioneel: eigen OpenAI KEY gebruiken

Als je zelf een key hebt voor OpenAI mag je die ook gebruiken. Het aanmaken van het model ziet er dan iets anders uit. Afhankelijk van je abonnement kan je hier `gpt-3.5-turbo` of `gpt-4` gebruiken. [Je kan hier een eigen key aanvragen](https://platform.openai.com/docs/introduction).

```js
import { ChatOpenAI } from "@langchain/openai"

const model = new ChatOpenAI({
    modelName: "gpt-3.5-turbo",
    openAIApiKey: process.env.OPEN_AI_KEY
})
```

<br><br><br>

## Links

- [LangChain](https://js.langchain.com/docs/get_started/quickstart)
- [Langchain Azure instellingen](https://js.langchain.com/docs/integrations/chat/azure)
- [Node Express Hello World](https://expressjs.com/en/starter/hello-world.html)
- [JSON teruggeven vanuit Express](https://expressjs.com/en/5x/api.html#res.json)
- [Voorbeeld fetch met POST](https://jasonwatmore.com/post/2021/09/05/fetch-http-post-request-examples)

In de lessen benaderen we Azure OpenAI via LangChain. Je kan hier meer lezen over Azure en OpenAI.

- [Eigen OpenAI key aanvragen](https://platform.openai.com/docs/introduction)
- [OpenAI API](https://platform.openai.com/docs/introduction)
- [Azure REST API](https://learn.microsoft.com/en-gb/azure/ai-services/openai/reference)
