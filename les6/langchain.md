# Les 1 deel 2

## Werken met Langchain

Voeg een `.env` file toe aan je server project. Voeg de API keys uit de les toe.

```
SERVER
├── .env
├── .gitignore
└── server.js
```


```sh
OPENAI_API_TYPE=___
OPENAI_API_VERSION=___
OPENAI_API_BASE=___
AZURE_OPENAI_API_KEY=___
DEPLOYMENT_NAME=___
ENGINE_NAME=___
INSTANCE_NAME=___
```

> *🚨 Deel de API keys en de ENV file niet met anderen. Plaats de API keys niet online.*



#### API keys lezen

Met *Node 22* kan je via `--env-file` de .env variabelen doorgeven aan je server. Gebruik `--watch` zodat je niet telkens de server hoeft te herstarten als je iets aanpast.

```sh
node --env-file=.env --watch server.js
```
Test of het inlezen van je `.env` is gelukt:
```js
console.log(process.env.AZURE_OPENAI_API_KEY)
```
> ⚠️ *TIP: plaats je volledige node commando `node --env-file=.env --watch server.js` in de `scripts` van je `package.json` zodat je niet telkens dit hele commando hoeft te typen.*


<br><br><br>

## Langchain

[Langchain](https://js.langchain.com/docs/get_started/introduction) is de API die we in `server.js` gaan gebruiken om te werken met Azure OpenAI (ChatGPT). 

```sh
npm install langchain @langchain/core
npm install @langchain/openai
```
Om de OpenAI API's te kunnen aanroepen moet de `.env` file zijn ingelezen. Vervolgens kan je een vraag stellen aan het chat model. 

Je kan dit testen door een apart `test.js` bestand te maken waarin je alleen `langchain` uitprobeert. Als dit werkt kan je het integreren in je `server.js` file.

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


## Express

- Voeg de langchain code toe aan je `server.js`
- Je kan eventueel een nieuw endpoint `/chat` aanmaken in `server.js`
- Je kan nu de `prompt` die binnenkomt uit het formulier versturen naar `OpenAI`
- Het resultaat geef je weer terug aan je client als JSON in de `response`.
- In de client toon je het resultaat in de HTML pagina.

<img src="../images/form-example.png" width="900">

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
