# Les 6 - deel 1

In dit deel gaan we [langchain](https://js.langchain.com/docs/introduction/) gebruiken om vanuit code een Azure OpenAI taalmodel aan te roepen.

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
AZURE_OPENAI_API_VERSION=
AZURE_OPENAI_API_INSTANCE_NAME=
AZURE_OPENAI_API_KEY=
AZURE_OPENAI_API_DEPLOYMENT_NAME=
AZURE_EMBEDDING_DEPLOYMENT_NAME=
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
node --env-file=.env server.js
```
Test of het inlezen van je `.env` is gelukt!



<br><br><br>

## Langchain installeren

[Langchain](https://js.langchain.com/docs/get_started/introduction) is de API die we in `server.js` gaan gebruiken om te werken met Azure OpenAI (ChatGPT). 

```sh
npm install langchain @langchain/core @langchain/openai
```
*Let op dat je deze commando's in de server map uitvoert. De server map ziet er nu zo uit:*

```
SERVER
├── .env
├── .gitignore
├── package.json
├── node_modules
└── server.js
```

⚠️ *In `package.json` moet je aangeven dat je met modules werkt: `"type": "module",`*

<br><br><br>

## Azure OpenAI aanroepen

Je kan nu een chatbot aanmaken en met het `invoke()` commando een prompt sturen. Let op dat de `env` file ingelezen is: `node --env-file=.env server.js`

```js
import { AzureChatOpenAI } from "@langchain/openai";

const model = new AzureChatOpenAI({ temperature: 1 });

async function simpleTest() {
    const chat = await model.invoke("Why do cats meow?")
    console.log(chat.content)
}

await simpleTest()
```


<br><br><br>

### Tips

- Gebruik `--watch` zodat de server.js automatisch opnieuw uitgevoerd wordt na een aanpassing.
- Plaats je volledige node commando `node --env-file=.env --watch server.js` in de `scripts` van je `package.json` zodat je niet telkens dit hele commando hoeft te typen.*

### Troubleshooting

- Als `--env-file=` of `--watch` niet werkt, moet je je versie van `node` updaten.
- Als je "api key not found" error krijgt ben je waarschijnlijk vergeten de `.env` file te lezen.
- Let op dat al je bestanden (env, package.json, server.js) in dezelfde SERVER map staan.
- Zet het `type` van je project op `module` in `package.json` om imports te kunnen gebruiken. 
- 📃 De documentatie van Langchain en OpenAI verandert regelmatig. Als de voorbeeldcode uit deze repository een warning geeft moet je de officiële documentatie nalezen.
- De Azure API Key kan veranderen tijdens de lessen. Als je een "not authorized" error krijgt kan je in Teams kijken of er een nieuwe key is.
- In `package.json` moet je aangeven dat je met modules werkt: `"type": "module"`.

<br><br><br>

## Optioneel: OpenAI gebruiken

Als je zelf een key hebt voor OpenAI mag je die ook gebruiken. Het aanmaken van het model ziet er dan iets anders uit. Afhankelijk van je abonnement kan je nu ook `gpt-4o` gebruiken. [Je kan hier een eigen key aanvragen](https://platform.openai.com/docs/introduction).

```js
import { ChatOpenAI } from "@langchain/openai"

const model = new ChatOpenAI({
    modelName: "gpt-4o",
    openAIApiKey: process.env.OPENAI_API_KEY
})
```

<br><br><br>

## Links

- [LangChain](https://js.langchain.com/docs/get_started/quickstart)
- [Langchain Azure instellingen](https://js.langchain.com/docs/integrations/chat/azure/)
- [More chat models in langchain](https://js.langchain.com/docs/integrations/chat/)