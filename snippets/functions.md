# Function calling

Je kan een LLM gebruiken om functies in je code aan te roepen. Het resultaat van de LLM call is de naam van de functie die je moet aanroepen, zie dit voorbeeld:

```js
let result = model.invoke("wat is het weer in tokyo")
```
Het resultaat van de LLM call is nu een JSON object:
```js
function:{name:"getWeather", arguments:["tokyo"]}
```
Je gebruikt dit resultaat om functies in je eigen code uit te voeren, op basis van de taalopdracht van de eindgebruiker.

#### 🚫 Azure

> *Function calling (a.k.a. "tools") werkt in ChatGPT 3.5 met de laatste update. Omdat Azure een versie achter loopt werkt het nog niet met de Azure OpenAI keys.*

<br><br><br>

## Demo weerbericht

Dit is een zelfgeschreven functie in jouw javascript app.

```js
global.getCurrentWeather = (location) => {
    if (location.toLowerCase().includes("tokyo")) {
        return { location, temperature: "10 c" };
    } else if (location.toLowerCase().includes("san francisco")) {
        return { location, temperature: "72 f" };
    } else {
        return { location, temperature: "22 c" };
    }
}
```
Vervolgens geef je de *function signature* mee aan je `model.invoke()` call. Het LLM gaat nu proberen om met het prompt deze functie in te vullen.

```js
import { ChatOpenAI } from "@langchain/openai";

const chat = new ChatOpenAI({
    modelName: "gpt-3.5-turbo-1106",
    maxTokens: 128,
    openAIApiKey: process.env.OPENAI_API_KEY,
    response_format: { type: "json_object" },
}).bind({
    tools: [
        {
            type: "function",
            function: {
                name: "getCurrentWeather",
                description: "Get the current weather in a given location",
                parameters: {
                    type: "object",
                    properties: {
                        location: {
                            type: "string",
                            description: "The city and state, e.g. San Francisco, CA",
                        },
                    },
                    required: ["location"],
                },
            },
        },
    ],
    tool_choice: "auto", 
});
```
Als je nu een vraag stelt krijg je als antwoord alleen nog functienamen en functie parameters. Deze vind je in de `additional_kwargs` van het antwoord.
```js
const res = await chat.invoke([
    ["human", "What's the weather like in San Francisco, Tokyo, and Paris?"]
])
console.log(res.additional_kwargs.tool_calls)
```
<br><br>

## Functies aanroepen

Het chatmodel geeft alleen aan welke functies je met welke parameters moet aanroepen. Je haalt dit uit de `additional_kwargs.tool_calls` van het response.

Omdat het enigszins tricky is om een functie op basis van een *string response* uit te voeren moet je dubbel checken of dit wel echt een functie in jouw code is.

```js
for(let tool of res.additional_kwargs.tool_calls) {
    
    const fn = tool.function.name                          // de functie
    const args = JSON.parse(tool.function.arguments)       // de parameters

    if (fn in global && typeof global[fn] === "function" && args) {
        let res_weather = global[fn](args.location);
        console.log(`The weather in ${res_weather.location} is ${res_weather.temperature}`)
    } else {
        console.log("could not find " + fn);
    }
}
```

<br><Br>

## Agents

Een ***Agent*** is een LLM die een zelf bedachte function ook daadwerkelijk kan uitvoeren en zelf kan evalueren wat het resultaat is. 

- [OpenAI Agents in Langchain](https://js.langchain.com/docs/modules/agents/)

<br><br>

## Links

- [OpenAI Tools and function calling](https://platform.openai.com/docs/guides/function-calling)
- [OpenAI LangChain Function Calling](https://js.langchain.com/docs/integrations/chat/openai)